namespace Events.api.Components.MessageBroker
{
    public interface IEventProducer
    {

        Task PublishUserEventAsync(string @event);
        Task PublishMovieEventAsync(string @event);
        Task PublishPaymentEventAsync(string @event);

    }
}
