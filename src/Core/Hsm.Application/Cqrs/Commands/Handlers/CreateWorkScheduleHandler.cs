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
    public class CreateWorkScheduleHandler(IWriteRepository<WorkSchedule> _writeRepo) : IEventHandler<CreateWorkScheduleRequest, CreateWorkScheduleResponse>
    {
        public async Task<CreateWorkScheduleResponse> Handle(CreateWorkScheduleRequest @event)
        {
            WorkSchedule workSchedule = ModelMap.Map<CreateWorkScheduleDto, WorkSchedule>(@event.WorkScheduleDto);

            await _writeRepo.AddAsync(workSchedule);

            bool response = await _writeRepo.SaveChangesAsync();

            return response is true ? new(ApiResponseModel<bool>.CreateSuccess(response)) : new(ApiResponseModel<bool>.CreateFailure());
        }
    }
}
