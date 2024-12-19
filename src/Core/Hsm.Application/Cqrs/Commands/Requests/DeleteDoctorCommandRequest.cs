using EventFlux;
using Hsm.Application.Cqrs.Commands.Responses;

namespace Hsm.Application.Cqrs.Commands.Requests
{
    public class DeleteDoctorCommandRequest : IEventRequest<DeleteDoctorCommandResponse>
    {
        public Guid Id { get; set; }

        public DeleteDoctorCommandRequest(Guid ıd)
        {
            Id = ıd;
        }
    }
}
