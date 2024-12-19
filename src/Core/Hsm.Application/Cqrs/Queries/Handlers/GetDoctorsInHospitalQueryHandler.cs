using Base.Repository.Abstractions;
using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Queries.Requests;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Application.Extensions;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Doctor;
using Hsm.Domain.Models.Dtos.Hospital;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using ModelMapper;

namespace Hsm.Application.Cqrs.Queries.Handlers
{
    public class GetDoctorsInHospitalQueryHandler(IUnitOfWork<Hospital> _unitOfWork) : IEventHandler<GetDoctorsInHospitalQueryRequest, GetDoctorsInHospitalQueryResponse>
    {
        public async Task<GetDoctorsInHospitalQueryResponse> Handle(GetDoctorsInHospitalQueryRequest @event)
        {
            IReadRepository<Hospital> _readRepository = _unitOfWork.GetReadRepository();

            ITable table = _unitOfWork.GetTable();

            Hospital? hospital = await _readRepository.GetAsync(h => h.Id == @event.HospitalId);

            if (hospital is null)
                return new(ApiResponseModel<PageResponse<DoctorModel>>.CreateNotFound("Hospital not found"));

            PageResponse<Doctor> doctors = await hospital.Doctors.AsQueryable().GetPage(@event.PageNumber, @event.PageSize);

            PageResponse<DoctorModel> pageDoctorModels = ModelMap.Map<PageResponse<Doctor>, PageResponse<DoctorModel>>(doctors);

            return new(ApiResponseModel<PageResponse<DoctorModel>>.CreateSuccess(pageDoctorModels));
        }
    }
}
