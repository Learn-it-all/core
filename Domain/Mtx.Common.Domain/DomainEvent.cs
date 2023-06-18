using System;
using System.Collections.Generic;
using System.Linq;

namespace Mtx.Common.Domain
{
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
}
