using Base.Repository.Abstractions;
using EfCore.Repository;
using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.WorkSchedule;
using Hsm.Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using ModelMapper;

namespace Hsm.Application.Cqrs.Commands.Handlers
{
    public class UpdateWorkScheduleCommandHandler(IBaseWriteRepository<WorkSchedule> _writeRepo, IReadRepository<WorkSchedule> _readRepo) : IEventHandler<UpdateWorkScheduleCommandRequest, UpdateWorkScheduleCommandResponse>
    {
        public async Task<UpdateWorkScheduleCommandResponse> Handle(UpdateWorkScheduleCommandRequest @event)
        {
            Specification<WorkSchedule> specification = new();
            specification.Conditions.Add(h => h.Id == @event.UpdateWorkScheduleDto.Id);
            specification.Includes = query => query.Include(p => p.Appointments).Include(w => w.Doctor);

            WorkSchedule workSchedule = await _readRepo.GetAsync(specification);

            workSchedule.SetId(@event.UpdateWorkScheduleDto.Id);
            workSchedule.SetName(@event.UpdateWorkScheduleDto.Name);
            workSchedule.SetIsActive(@event.UpdateWorkScheduleDto.IsActive);
            workSchedule.SetStartDate(@event.UpdateWorkScheduleDto.StartDate);
            workSchedule.SetEndDate(@event.UpdateWorkScheduleDto.EndDate);
            workSchedule.SetRowVersion(@event.UpdateWorkScheduleDto.RowVersion);
            workSchedule.UpdateAppointment(@event.UpdateWorkScheduleDto.AppointmentModels);


            bool updateResponse = await _writeRepo.UpdateAsync(workSchedule);

            if (!updateResponse)
                return new(ApiResponseModel<bool>.CreateFailure("Update process is not succesfully"));

            await _writeRepo.SaveChangesAsync();

            return new(ApiResponseModel<bool>.CreateSuccess(true));
        }
    }
}
