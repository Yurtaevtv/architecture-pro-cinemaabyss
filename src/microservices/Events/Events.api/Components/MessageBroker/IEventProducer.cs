namespace Events.api.Components.MessageBroker
{
    public interface IEventProducer
    {

        Task PublishEventAsync(string @event);

    }
}
