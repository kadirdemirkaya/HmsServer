using EventFlux;
using Hsm.Application.Cqrs.Commands.Responses;

namespace Hsm.Application.Cqrs.Commands.Requests
{
    public class CancelAppointmentCommandRequest : IEventRequest<CancelAppointmentCommandResponse>
    {
        public Guid AppointmentId { get; set; }

        public CancelAppointmentCommandRequest(Guid appointmentId)
        {
            AppointmentId = appointmentId;
        }
    }
}
