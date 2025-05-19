using Eventing.Events;
using Eventing.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Eventing.RabbitMQ
{
    public class EventBusRabbitMQ : IEventBus
    {
        private readonly IRabbitMQPersistentConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;

        public EventBusRabbitMQ(IRabbitMQPersistentConnection connection, IServiceProvider serviceProvider)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _serviceProvider = serviceProvider;
            if (!_connection.IsConnected)
                _connection.TryConnect();

            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare("OrderExchange", ExchangeType.Fanout, durable: true);

        }

        public void Publish(IntegrationEvent @event)
        {
            var eventName = @event.GetType().Name;

            var message = JsonSerializer.Serialize(@event, @event.GetType());

            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(
            exchange: "OrderExchange",
            routingKey: "",
            basicProperties: null,
            body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event, @event.GetType())));
        }


        public void Subscribe<T, TH>(string queueName) where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(queue: queueName, exchange: "OrderExchange", routingKey: "");


            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var integrationEvent = JsonSerializer.Deserialize<T>(message, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                using var scope = _serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<TH>();
                await handler.Handle(integrationEvent!);
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

    }
}
