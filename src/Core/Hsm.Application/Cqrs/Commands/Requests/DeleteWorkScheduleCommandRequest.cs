using EventFlux;
using Hsm.Application.Cqrs.Commands.Responses;

namespace Hsm.Application.Cqrs.Commands.Requests
{
    public class DeleteWorkScheduleCommandRequest : IEventRequest<DeleteWorkScheduleCommandResponse>
    {
        public Guid WorkScheduleId { get; set; }

        public DeleteWorkScheduleCommandRequest(Guid workScheduleId)
        {
            WorkScheduleId = workScheduleId;
        }
    }
}
