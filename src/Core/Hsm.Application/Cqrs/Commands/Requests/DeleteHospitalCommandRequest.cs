using EventFlux;
using Hsm.Application.Cqrs.Commands.Responses;

namespace Hsm.Application.Cqrs.Commands.Requests
{
    public class DeleteHospitalCommandRequest : IEventRequest<DeleteHospitalCommandResponse>
    {
        public Guid HospitalId { get; set; }

        public DeleteHospitalCommandRequest(Guid hospitalId)
        {
            HospitalId = hospitalId;
        }
    }
}
