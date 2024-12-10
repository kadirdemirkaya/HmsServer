using EventFlux;

namespace HsmServer.UnitTest.Events
{
    public class PublishEventRequest : IEventRequest
    {
        public string Data { get; set; }
    }

    public class PublishEventTestHandler : IEventHandler<PublishEventRequest>
    {
        public async Task Handle(PublishEventRequest @event)
        {
            Console.WriteLine(@event.Data);
        }
    }

    public class PublishEventExampleHandler2 : IEventHandler<PublishEventRequest>
    {
        public async Task Handle(PublishEventRequest @event)
        {
            Console.WriteLine(@event.Data);
        }
    }
}
