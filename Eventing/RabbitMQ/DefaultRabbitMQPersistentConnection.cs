using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;

namespace Eventing.RabbitMQ
{
    public class DefaultRabbitMQPersistentConnection : IRabbitMQPersistentConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private bool _disposed;
        private readonly int _retryCount = 5;

        public DefaultRabbitMQPersistentConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

        public IModel CreateModel()
        {
            if (!IsConnected)
                throw new InvalidOperationException("No RabbitMQ connections are available.");

            return _connection.CreateModel();
        }

        public bool TryConnect()
        {
            Console.WriteLine("🔄 RabbitMQ bağlantısı deneniyor (Polly ile)...");

            RetryPolicy policy = Policy
                .Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // Exponential backoff
                    (ex, time) =>
                    {
                        Console.WriteLine($"❌ Hata: {ex.Message}. {time.TotalSeconds}s sonra tekrar denenecek...");
                    });

            try
            {
                policy.Execute(() =>
                {
                    _connection = _connectionFactory.CreateConnection();
                });

                return IsConnected;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                _connection?.Dispose();
            }
            catch { }

            _disposed = true;
        }
    }
}
