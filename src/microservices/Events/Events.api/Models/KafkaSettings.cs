using System.ComponentModel.DataAnnotations;

namespace Events.api.Models
{
    public class KafkaSettings
    {
        [Required]
        public string? BootstrapServers { get; init; }

        [Required]
        public string? DeviceTelemetryTopic { get; init; } = "events-telemetry";
    }
}
