using Confluent.Kafka;
using Events.api.Background;
using Events.api.Components.MessageBroker;
using Events.api.Components.MessageBroker.Implementation;
using Events.api.Models;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging(logBuilder =>
    {
        logBuilder.ClearProviders();
        logBuilder.SetMinimumLevel(LogLevel.Debug);
        logBuilder.AddConsole();
        logBuilder.AddNLog("NLog.config");
    }
);

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));
KafkaSettings kafkaSettings = builder.Configuration.GetSection("Kafka").Get<KafkaSettings>()!;
builder.Services.AddOptions<KafkaSettings>()
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddTransient<IEventProducer, KafkaEventProducer>();
builder.Services.AddTransient<IEventConsumer, KafkaMovieEventConsumer>();
builder.Services.AddTransient<IEventConsumer, KafkaUserEventConsumer>();
builder.Services.AddTransient<IEventConsumer, KafkaPaymentEventConsumer>();


builder.Services.AddHealthChecks()
    .AddKafka(new ProducerConfig
    {
        BootstrapServers = kafkaSettings!.BootstrapServers
    });

// Add services to the container.
builder.Services.AddHostedService<ConsumerBackgroudService>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHealthChecks("/api/events/health", new HealthCheckOptions() { ResponseWriter = (hc, hr) => hc.Response.WriteAsJsonAsync(new { status = hr.Status == HealthStatus.Healthy }) });

app.MapControllers();

app.Run();
