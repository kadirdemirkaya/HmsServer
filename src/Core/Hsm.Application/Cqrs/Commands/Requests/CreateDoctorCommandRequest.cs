using EventFlux;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Models.Dtos.Doctor;

namespace Hsm.Application.Cqrs.Commands.Requests
{
    public class CreateDoctorCommandRequest : IEventRequest<CreateDoctorCommandResponse>
    {
        public CreateDoctorDto CreateDoctorModel { get; set; }

        public CreateDoctorCommandRequest(CreateDoctorDto createDoctorModel)
        {
            CreateDoctorModel = createDoctorModel;
        }
    }
}
