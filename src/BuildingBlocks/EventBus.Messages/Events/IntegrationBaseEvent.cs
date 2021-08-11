using System;

namespace EventBus.Messages.Events
{
    public class IntegrationBaseEvent
    {
        public IntegrationBaseEvent() : this(Guid.NewGuid(), DateTime.UtcNow)
        {
            
        }

        public IntegrationBaseEvent(Guid id, DateTime createdDate)
        {
            Id = id;
            CreatedDate = createdDate;
        }

        public Guid Id { get; private set; }

        public DateTime CreatedDate { get; private set; }
    }
}
