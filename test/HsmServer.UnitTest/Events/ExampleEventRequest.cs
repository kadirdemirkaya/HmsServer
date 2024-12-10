using EventFlux;

namespace HsmServer.UnitTest.Events
{
    public class ExampleEventRequest : IEventRequest<ExampleEventResponse>
    {
        public int Num { get; set; }
    }

    public class ExampleEventResponse : IEventResponse
    {
        public string Res { get; set; }
    }

    public class ExampleEventHandler : IEventHandler<ExampleEventRequest, ExampleEventResponse>
    {
        public async Task<ExampleEventResponse> Handle(ExampleEventRequest @event)
        {
            return new() { Res = @event.Num.ToString() };
        }
    }
}
