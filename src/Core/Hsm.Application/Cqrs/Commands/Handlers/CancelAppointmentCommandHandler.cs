using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Response;

namespace Hsm.Application.Cqrs.Commands.Handlers
{
    public class CancelAppointmentCommandHandler(IReadRepository<Appointment> _readRepo, IWriteRepository<Appointment> _writeRepo) : IEventHandler<CancelAppointmentCommandRequest, CancelAppointmentCommandResponse>
    {
        public async Task<CancelAppointmentCommandResponse> Handle(CancelAppointmentCommandRequest @event)
        {
            Appointment? existAppointment = await _readRepo.GetAsync(a => a.Id == @event.AppointmentId);

            if (existAppointment is not null)
            {
                existAppointment.SetIsActive(false);

                bool updateResponse = await _writeRepo.UpdateAsync(existAppointment);

                return updateResponse is true ? new(ApiResponseModel<bool>.CreateSuccess(updateResponse)) : new(ApiResponseModel<bool>.CreateFailure());
            }

            return new(ApiResponseModel<bool>.CreateNotFound());
        }
    }
}
