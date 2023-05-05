using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Mtx.Common.Domain
{
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
