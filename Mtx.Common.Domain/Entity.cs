using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using MediatR;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;

namespace Mtx.Common.Domain
{
    public abstract class Entity 
    {
        [Newtonsoft.Json.JsonIgnore]//ignore when writing to Cosmos
        private List<INotification> _domainEvents = new();

        public Guid Id { get;private set; } = Guid.NewGuid();

        [System.Text.Json.Serialization.JsonIgnore] //ignore on http response
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
