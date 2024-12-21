using EventFlux;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Models.Dtos.WorkSchedule;

namespace Hsm.Application.Cqrs.Commands.Requests
{
    public class UpdateWorkScheduleCommandRequest : IEventRequest<UpdateWorkScheduleCommandResponse>
    {
        public UpdateWorkScheduleDto UpdateWorkScheduleDto { get; set; }

        public UpdateWorkScheduleCommandRequest(UpdateWorkScheduleDto updateWorkScheduleDto)
        {
            UpdateWorkScheduleDto = updateWorkScheduleDto;
        }
    }
}
