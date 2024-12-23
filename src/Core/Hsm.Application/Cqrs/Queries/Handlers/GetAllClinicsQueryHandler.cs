using EfCore.Repository.Abstractions;
using EfCore.Repository.Concretes;
using EventFlux;
using Hsm.Application.Cqrs.Queries.Requests;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Application.Extensions;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Clinic;
using Hsm.Domain.Models.Dtos.Doctor;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;
using ModelMapper;

namespace Hsm.Application.Cqrs.Queries.Handlers
{
    public class GetAllClinicsQueryHandler(IUnitOfWork<Clinical> _unitOfWork) : IEventHandler<GetAllClinicsQueryRequest, GetAllClinicsQueryResponse>
    {
        public async Task<GetAllClinicsQueryResponse> Handle(GetAllClinicsQueryRequest @event)
        {
            PageResponse<Clinical> clinicals = await _unitOfWork.GetTable().Table.Set<Clinical>().GetPage(@event.PageNumber, @event.PageSize);

            PageResponse<ClinicalModel> clinicalModels = ModelMap.Map<PageResponse<Clinical>, PageResponse<ClinicalModel>>(clinicals);

            return new(ApiResponseModel<PageResponse<ClinicalModel>>.CreateSuccess(clinicalModels));
        }
    }
}
