namespace Eventing.Interfaces
{
    public interface IIntegrationEventHandler<in TEvent> where TEvent : class
    {
        Task Handle(TEvent @event);
    }
}
