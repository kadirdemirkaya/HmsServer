using EfCore.Repository;
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
using System.Linq.Expressions;

namespace Hsm.Application.Cqrs.Queries.Handlers
{
    public class GetDoctorsInHospitalQueryHandler(IReadRepository<Doctor> _readRepo) : IEventHandler<GetDoctorsInHospitalQueryRequest, GetDoctorsInHospitalQueryResponse>
    {
        public async Task<GetDoctorsInHospitalQueryResponse> Handle(GetDoctorsInHospitalQueryRequest @event)
        {
            Specification<Doctor> specification = new();
            specification.AsNoTracking = false;
            specification.Skip = @event.PageSize;
            specification.Take = @event.PageNumber;
            specification.Conditions.Add(d => d.HospitalId == @event.HospitalId);
            specification.Includes = query => query.Include(d => d.AppUser);
            Expression<Func<Doctor, DoctorModel>> selectExpression = doctor => new DoctorModel
            {
                Id = doctor.Id,
                AppUserId = doctor.AppUserId,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                IsActive = doctor.IsActive,
                RowVersion = doctor.RowVersion,
                Schedule = doctor.Schedule,
                Specialty = doctor.Specialty
            };

            List<DoctorModel> doctorModels = await _readRepo.GetListAsync(specification, selectExpression);

            if (doctorModels is null)
                return new(ApiResponseModel<PageResponse<DoctorModel>>.CreateNotFound("Doctors not found"));

            PageResponse<DoctorModel> pageResponse = new(doctorModels, new()
            {
                PageNumber = @event.PageNumber,
                PageSize = @event.PageSize,
                TotalRowCount = doctorModels.Count()
            });

            return new(ApiResponseModel<PageResponse<DoctorModel>>.CreateSuccess(pageResponse));
        }
    }
}
