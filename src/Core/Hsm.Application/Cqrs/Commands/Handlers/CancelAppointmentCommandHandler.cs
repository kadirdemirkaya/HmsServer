using EfCore.Repository;
using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace Hsm.Application.Cqrs.Commands.Handlers
{
    public class CancelAppointmentCommandHandler(IReadRepository<Appointment> _readRepo, IWriteRepository<Appointment> _writeRepo) : IEventHandler<CancelAppointmentCommandRequest, CancelAppointmentCommandResponse>
    {
        public async Task<CancelAppointmentCommandResponse> Handle(CancelAppointmentCommandRequest @event)
        {
            Specification<Appointment> specification = new();
            specification.AsNoTracking = false;
            specification.Conditions.Add(a => a.Id == @event.AppointmentId);
            specification.Conditions.Add(a => a.IsActive == true);
            specification.Includes = query => query.Include(h => h.WorkSchedule);

            Appointment? existAppointment = await _readRepo.GetAsync(specification);

            if (existAppointment is not null)
            {
                existAppointment.SetIsActive(false);

                existAppointment.WorkSchedule.SetIsActive(true);

                bool updateResponse = await _writeRepo.UpdateAsync(existAppointment);

                return updateResponse is true ? new(ApiResponseModel<bool>.CreateSuccess(updateResponse)) : new(ApiResponseModel<bool>.CreateFailure());
            }

            return new(ApiResponseModel<bool>.CreateNotFound());
        }
    }
}
