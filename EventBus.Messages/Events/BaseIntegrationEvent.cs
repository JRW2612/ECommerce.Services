namespace EventBus.Messages.Events
{
    public abstract class BaseIntegrationEvent
    {
        public Guid CorrelationId { get; set; }
        public DateTime CreatedAt { get; set; }

        public BaseIntegrationEvent()
        {
            CorrelationId = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        public BaseIntegrationEvent(Guid correlationId, DateTime createdAt)
        {
            CorrelationId = correlationId;
            CreatedAt = createdAt;
        }
    }
}
