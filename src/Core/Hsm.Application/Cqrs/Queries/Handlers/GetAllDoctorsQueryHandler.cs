using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Queries.Requests;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Application.Extensions;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Doctor;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using ModelMapper;

namespace Hsm.Application.Cqrs.Queries.Handlers
{
    public class GetAllDoctorsQueryHandler(IUnitOfWork<Doctor> _unitOfWork) : IEventHandler<GetAllDoctorsQueryRequest, GetAllDoctorsQueryResponse>
    {
        public async Task<GetAllDoctorsQueryResponse> Handle(GetAllDoctorsQueryRequest @event)
        {
            PageResponse<Doctor> doctor = await _unitOfWork.GetTable().Table.Set<Doctor>().Include(d => d.Hospital).GetPage(@event.PageNumber, @event.PageSize);

            PageResponse<DoctorModel> doctorModel = ModelMap.Map<PageResponse<Doctor>, PageResponse<DoctorModel>>(doctor);

            return new(ApiResponseModel<PageResponse<DoctorModel>>.CreateSuccess(doctorModel));
        }
    }
}
