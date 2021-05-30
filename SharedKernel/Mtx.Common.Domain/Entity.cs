using System;
using System.Collections.Generic;
using MediatR;

namespace Mtx.Common.Domain
{
    public abstract class Entity
    {
        private List<INotification> _domainEvents = new();

        public List<INotification> DomainEvents => _domainEvents;

        public void AddDomainEvent(INotification eventItem)

        {

            _domainEvents.Add(eventItem);

        }

        public void RemoveDomainEvent(INotification eventItem)

        {

            if (_domainEvents is null) return;

            _domainEvents.Remove(eventItem);

        }
    }

}
