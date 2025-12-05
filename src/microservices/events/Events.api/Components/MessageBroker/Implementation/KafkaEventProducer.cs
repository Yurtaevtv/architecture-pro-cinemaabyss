using Confluent.Kafka;
using Events.api.Models;
using Microsoft.Extensions.Options;

namespace Events.api.Components.MessageBroker.Implementation
{
    public class KafkaEventProducer : IEventProducer
    {
        private readonly IOptions<KafkaSettings> _settings;
        private readonly IProducer<string, string> _producer;


        public KafkaEventProducer(IOptions<KafkaSettings> settings)
        {
            _settings = settings;

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = _settings.Value.BootstrapServers,
            };

            _producer = new ProducerBuilder<string, string>(producerConfig).Build();

        }


        public Task PublishUserEventAsync(string @event)
        {
            return _producer.ProduceAsync("user-events",
                new Message<string, string>() { Key = Guid.NewGuid().ToString("N"), Value = @event });
        }

        public Task PublishMovieEventAsync(string @event)
        {
            return _producer.ProduceAsync("movie-events",
                new Message<string, string>() { Key = Guid.NewGuid().ToString("N"), Value = @event });
        }

        public Task PublishPaymentEventAsync(string @event)
        {
            return _producer.ProduceAsync("payment-events",
                new Message<string, string>() { Key = Guid.NewGuid().ToString("N"), Value = @event });
        }
    }
}
