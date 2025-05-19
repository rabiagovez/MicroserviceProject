namespace Eventing.Events
{
    public abstract class IntegrationEvent
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    }
}
