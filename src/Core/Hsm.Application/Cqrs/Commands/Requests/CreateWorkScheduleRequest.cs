using EventFlux;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Models.Dtos.WorkSchedule;

namespace Hsm.Application.Cqrs.Commands.Requests
{
    public class CreateWorkScheduleRequest : IEventRequest<CreateWorkScheduleResponse>
    {
        public CreateWorkScheduleDto WorkScheduleDto { get; set; }

        public CreateWorkScheduleRequest(CreateWorkScheduleDto workScheduleDto)
        {
            WorkScheduleDto = workScheduleDto;
        }
    }
}
