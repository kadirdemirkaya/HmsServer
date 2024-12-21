using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Response;

namespace Hsm.Application.Cqrs.Commands.Handlers
{
    public class DeleteWorkScheduleCommandHandler(IWriteRepository<WorkSchedule> _writeRepo, IReadRepository<WorkSchedule> _readRepo) : IEventHandler<DeleteWorkScheduleCommandRequest, DeleteWorkScheduleCommandResponse>
    {
        public async Task<DeleteWorkScheduleCommandResponse> Handle(DeleteWorkScheduleCommandRequest @event)
        {
            WorkSchedule? workSchedule = await _readRepo.GetAsync(w => w.Id == @event.WorkScheduleId);

            if (workSchedule is not null)
            {
                workSchedule.SetIsActive(false);

                bool response = await _writeRepo.UpdateAsync(workSchedule);

                return response is true ? new(ApiResponseModel<bool>.CreateSuccess(response)) : new(ApiResponseModel<bool>.CreateFailure());
            }

            return new(ApiResponseModel<bool>.CreateNotFound());
        }
    }
}
