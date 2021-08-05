using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using MediatR;

namespace Mtx.Common.Domain
{
    public abstract class Entity 
    {
        private List<INotification> _domainEvents = new();

        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonIgnore]
        public List<INotification> DomainEvents => _domainEvents;

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }

        public void AddDomainEvent(INotification eventItem)

        {

            _domainEvents.Add(eventItem);

        }

        public void RemoveDomainEvent(INotification eventItem)

        {

            if (!_domainEvents.Any()) return;

            _domainEvents.Remove(eventItem);

        }
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
