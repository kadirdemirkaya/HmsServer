using EventFlux;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Models.Dtos.Doctor;

namespace Hsm.Application.Cqrs.Commands.Requests
{
    public class UpdateDoctorCommandRequest : IEventRequest<UpdateDoctorCommandResponse>
    {
        public UpdateDoctorDto UpdateDoctorDto { get; set; }

        public UpdateDoctorCommandRequest(UpdateDoctorDto updateDoctorDto)
        {
            UpdateDoctorDto = updateDoctorDto;
        }
    }
}
