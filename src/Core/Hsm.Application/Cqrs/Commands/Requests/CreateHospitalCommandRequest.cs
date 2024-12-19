using EventFlux;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Models.Dtos.Hospital;

namespace Hsm.Application.Cqrs.Commands.Requests
{
    public class CreateHospitalCommandRequest : IEventRequest<CreateHospitalCommandResponse>
    {
        public CreateHospitalDto CreateHospitalDto { get; set; }

        public CreateHospitalCommandRequest(CreateHospitalDto createHospitalDto)
        {
            CreateHospitalDto = createHospitalDto;
        }
    }
}
