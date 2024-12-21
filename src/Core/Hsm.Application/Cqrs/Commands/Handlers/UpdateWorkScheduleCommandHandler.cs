using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.WorkSchedule;
using Hsm.Domain.Models.Response;
using ModelMapper;

namespace Hsm.Application.Cqrs.Commands.Handlers
{
    public class UpdateWorkScheduleCommandHandler(IWriteRepository<WorkSchedule> _writeRepo, IReadRepository<WorkSchedule> _readRepo) : IEventHandler<UpdateWorkScheduleCommandRequest, UpdateWorkScheduleCommandResponse>
    {
        public async Task<UpdateWorkScheduleCommandResponse> Handle(UpdateWorkScheduleCommandRequest @event)
        {
            bool existWorkSchedule = await _readRepo.AnyAsync(w => w.Id == @event.UpdateWorkScheduleDto.Id);

            if(existWorkSchedule)
            {
                WorkSchedule workSchedule = ModelMap.Map<UpdateWorkScheduleDto, WorkSchedule>(@event.UpdateWorkScheduleDto);
                workSchedule.SetUpdatedDateUTC(DateTime.UtcNow);

                bool updateResponse = await _writeRepo.UpdateAsync(workSchedule);

                return updateResponse is true ? new(ApiResponseModel<bool>.CreateSuccess(true)) : new(ApiResponseModel<bool>.CreateFailure("Update process is not succesfully"));
            }

            return new(ApiResponseModel<bool>.CreateNotFound());
        }
    }
}
