using Eventing.Events;

namespace Eventing.Interfaces
{
    public interface IEventBus
    {
        void Publish(IntegrationEvent @event);
        //void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
        void Subscribe<T, TH>(string queueName) where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
    }
}
