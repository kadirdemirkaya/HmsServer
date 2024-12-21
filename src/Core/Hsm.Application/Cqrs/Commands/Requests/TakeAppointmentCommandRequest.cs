using EventFlux;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Models.Dtos.Appointment;

namespace Hsm.Application.Cqrs.Commands.Requests
{
    public class TakeAppointmentCommandRequest : IEventRequest<TakeAppointmentCommandResponse>
    {
        public TakeAppointmentDto TakeAppointmentDto { get; set; }

        public TakeAppointmentCommandRequest(TakeAppointmentDto takeAppointmentDto)
        {
            TakeAppointmentDto = takeAppointmentDto;
        }
    }
}
