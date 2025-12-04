using System.Text.Json;
using Confluent.Kafka;
using Events.api.Models;
using Microsoft.Extensions.Options;

namespace Events.api.Components.MessageBroker.Implementation
{
    public abstract class KafkaEventConsumer : IEventConsumer
    {
        private readonly IConsumer<string, string> _consumer;

        protected readonly IOptions<KafkaSettings> _settings;
        protected readonly ILogger<KafkaEventConsumer> _logger;

        protected abstract string Topic { get; }

        public KafkaEventConsumer(IOptions<KafkaSettings> settings,
            ILogger<KafkaEventConsumer> logger)
        {
            _settings = settings;

            var config = new ConsumerConfig
            {
                BootstrapServers = settings.Value.BootstrapServers,
                GroupId = Topic,
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnableAutoCommit = true
            };
            _logger = logger;
            _consumer = new ConsumerBuilder<string, string>(config).Build();
        }


        public async Task StartConsumingAsync(CancellationToken token)
        {
            _consumer.Subscribe(Topic);
            _logger.LogInformation("Kafka WebSocket consumer started. Topics: {Topics}", Topic);

            while (!token.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(token);

                    if (consumeResult?.Message?.Value != null)
                    {
                        _logger.LogInformation(JsonSerializer.Serialize(consumeResult?.Message));
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Error consuming message from Kafka");
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error in Kafka consumer");
                }
            }

        }

        public void Dispose()
        {
            _consumer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
