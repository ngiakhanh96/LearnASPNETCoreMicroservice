using System;
using MassTransit.Topology;

namespace EventBus.Messages.Events
{
    [ExcludeFromTopology]
    public abstract class IntegrationBaseEvent
    {
        protected IntegrationBaseEvent() : this(Guid.NewGuid(), DateTime.UtcNow)
        {
            
        }

        protected IntegrationBaseEvent(Guid id, DateTime createdDate)
        {
            Id = id;
            CreatedDate = createdDate;
        }

        public Guid Id { get; private set; }

        public DateTime CreatedDate { get; private set; }
    }
}
