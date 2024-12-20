using Base.Repository.Abstractions;
using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Queries.Requests;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Application.Extensions;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Hospital;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using ModelMapper;

namespace Hsm.Application.Cqrs.Queries.Handlers
{
    public class GetAllHospitalQueryHandler(IUnitOfWork<Hospital> _unitOfWork) : IEventHandler<GetAllHospitalQueryRequest, GetAllHospitalQueryResponse>
    {
        public async Task<GetAllHospitalQueryResponse> Handle(GetAllHospitalQueryRequest @event)
        {
            IReadRepository<Hospital> _readRepository = _unitOfWork.GetReadRepository();

            ITable table = _unitOfWork.GetTable();

            PageResponse<Hospital> hospitals = await table.Table.Set<Hospital>().Include(h => h.City).GetPage(@event.PageNumber, @event.PageSize);

            PageResponse<HospitalModel> pageDoctorModels = ModelMap.Map<PageResponse<Hospital>, PageResponse<HospitalModel>>(hospitals);

            return new(ApiResponseModel<PageResponse<HospitalModel>>.CreateSuccess(pageDoctorModels));
        }
    }
}
