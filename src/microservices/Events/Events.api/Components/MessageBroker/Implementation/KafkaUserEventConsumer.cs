using Events.api.Models;
using Microsoft.Extensions.Options;

namespace Events.api.Components.MessageBroker.Implementation
{
    public class KafkaUserEventConsumer(
        IOptions<KafkaSettings> settings,
        ILogger<KafkaEventConsumer> logger)
        : KafkaEventConsumer(settings, logger)
    {
        protected override string Topic => "user-events";
    }
}
