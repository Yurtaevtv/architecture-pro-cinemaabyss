using Events.api.Models;
using Microsoft.Extensions.Options;

namespace Events.api.Components.MessageBroker.Implementation
{
    public class KafkaMovieEventConsumer(
        IOptions<KafkaSettings> settings,
        ILogger<KafkaEventConsumer> logger)
        : KafkaEventConsumer(settings, logger)
    {
        protected override string Topic => "movie-events";
    }
}
