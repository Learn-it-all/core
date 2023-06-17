using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mtx.Common.Domain
{
	public interface ISourceType
	{
		public DateTimeOffset OccurredOn { get; }

		public DateTimeOffset ValidOn { get; }
	}
	public abstract record DomainEvent : ISourceType
	{
		//Type discriminator for CosmosDB
		private string Type => GetType().Name;

		public DateTimeOffset OccurredOn { get; }

		public DateTimeOffset ValidOn { get; }

		public int Version { get; }

		public static DomainEvent Null = new NullDomainEvent();

		public static IEnumerable<DomainEvent> All(params DomainEvent[] domainEvents) => All(domainEvents.AsEnumerable());

		public static IEnumerable<DomainEvent> All(IEnumerable<DomainEvent> domainEvents) => domainEvents.Where(e => !e.IsNull());

		public static IEnumerable<DomainEvent> None() => Enumerable.Empty<DomainEvent>();

		public virtual bool IsNull() => false;

		protected DomainEvent()
			: this(0)
		{
		}

		protected DomainEvent(int version) : this(DateTimeOffset.Now, version)
		{
		}

		protected DomainEvent(DateTimeOffset validOn, int version)
		{
			OccurredOn = validOn;
			ValidOn = validOn;
			Version = version;
		}

		internal record NullDomainEvent : DomainEvent
		{
			public override bool IsNull() => true;
		}
	}
	public abstract class SourcedEntity<TSource>
	{
		private readonly List<TSource> applied;
		private readonly int currentVersion;

		public List<TSource> Applied => applied;

		public int NextVersion => currentVersion + 1;

		public int CurrentVersion => currentVersion;

		protected SourcedEntity()
		{
			applied = new List<TSource>();
			currentVersion = 0;
		}

		protected SourcedEntity(IEnumerable<TSource> stream, int streamVersion)
			: this()
		{
			foreach (var source in stream)
			{
				DispatchWhen(source);
			}

			currentVersion = streamVersion;
		}

		protected void Apply(TSource source)
		{
			applied.Add(source);
			DispatchWhen(source);
		}

		protected void DispatchWhen(TSource source) => ((dynamic)this).When((dynamic)source!);
	}

	public abstract class Entity
	{
		[JsonIgnore]
		private List<INotification> _domainEvents = new();

		public Guid Id { get; private set; } = Guid.NewGuid();

		//property Type is used only to serialize the object as a cosmos db document.
		//This is the simplest possible way to achieve it at the time of writing.
		//The type attribute is required by Cosmos when different documents are stored on the same container
		[JsonProperty]
		private string Type => GetType().Name;

		[JsonIgnore]
		public List<INotification> DomainEvents => _domainEvents;

		public DateTime CreatedDate { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string? CreatedBy { get; set; }
		public string? ModifiedBy { get; set; }

		public void AddDomainEvent(INotification eventItem) => _domainEvents.Add(eventItem);

		public void RemoveDomainEvent(INotification eventItem) => _domainEvents.Remove(eventItem);
	}
	public interface IEntity
	{
		object Id { get; }
		DateTime CreatedDate { get; set; }
		DateTime? ModifiedDate { get; set; }
		string? CreatedBy { get; set; }
		string? ModifiedBy { get; set; }
	}

	public interface IEntity<T> : IEntity
	{
		new T Id { get; }
	}
}
