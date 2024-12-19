using EventFlux;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Models.Dtos.Hospital;

namespace Hsm.Application.Cqrs.Commands.Requests
{
    public class UpdateHospitalCommandRequest : IEventRequest<UpdateHospitalCommandResponse>
    {
        public UpdateHospitalDto UpdateHospitalDto { get; set; }

        public UpdateHospitalCommandRequest(UpdateHospitalDto updateHospitalDto)
        {
            UpdateHospitalDto = updateHospitalDto;
        }
    }
}
