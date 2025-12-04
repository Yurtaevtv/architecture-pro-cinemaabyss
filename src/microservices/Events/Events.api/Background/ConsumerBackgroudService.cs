using Confluent.Kafka;
using Events.api.Components.MessageBroker;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Events.api.Background
{
    public class ConsumerBackgroudService(IEventConsumer consumer) : BackgroundService
    {
        private CancellationTokenSource _cancellationTokenSource;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);

            return Task.Run(() => consumer.StartConsumingAsync(_cancellationTokenSource.Token), stoppingToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();
            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            consumer.Dispose();
            GC.SuppressFinalize(this);
            base.Dispose();
        }
    }
}
