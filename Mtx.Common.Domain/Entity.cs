using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace Mtx.Common.Domain
{
    public abstract class Entity
    {
        private List<INotification> _domainEvents = new();

        public Guid Id { get; private set; } = Guid.NewGuid();

        public List<INotification> DomainEvents => _domainEvents;

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

}
