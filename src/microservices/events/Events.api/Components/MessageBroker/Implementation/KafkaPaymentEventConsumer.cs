using Events.api.Models;
using Microsoft.Extensions.Options;

namespace Events.api.Components.MessageBroker.Implementation
{
    public class KafkaPaymentEventConsumer(
                    IOptions<KafkaSettings> settings, 
                    ILogger<KafkaEventConsumer> logger) 
                : KafkaEventConsumer(settings, logger)
    {
        protected override string Topic => "payment-events";
    }
}
