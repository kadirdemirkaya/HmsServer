using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Queries.Requests;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Application.Extensions;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Doctor;
using Hsm.Domain.Models.Dtos.WorkSchedule;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using ModelMapper;

namespace Hsm.Application.Cqrs.Queries.Handlers
{
    public class GetAllWorkScheduleQueryHandler(IUnitOfWork<WorkSchedule> _unitOfWork) : IEventHandler<GetAllWorkScheduleQueryRequest, GetAllWorkScheduleQueryResponse>
    {
        public async Task<GetAllWorkScheduleQueryResponse> Handle(GetAllWorkScheduleQueryRequest @event)
        {
            PageResponse<WorkSchedule> pageWorkSchedule = await _unitOfWork.GetTable().Table.Set<WorkSchedule>().Include(w => w.Doctor).Include(w => w.Appointments).GetPage(@event.PageNumber, @event.PageSize);

            PageResponse<WorkScheduleModel> workScheduleModel = ModelMap.Map<PageResponse<WorkSchedule>, PageResponse<WorkScheduleModel>>(pageWorkSchedule);

            return new(ApiResponseModel<PageResponse<WorkScheduleModel>>.CreateSuccess(workScheduleModel));
        }
    }
}
