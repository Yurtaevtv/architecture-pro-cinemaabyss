namespace Events.api.Components.MessageBroker
{
    public interface IEventConsumer : IDisposable
    {

        Task StartConsumingAsync(CancellationToken token);

    }
}
