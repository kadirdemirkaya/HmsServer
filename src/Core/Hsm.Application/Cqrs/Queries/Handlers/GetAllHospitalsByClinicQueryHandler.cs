using EfCore.Repository;
using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Queries.Requests;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Clinic;
using Hsm.Domain.Models.Dtos.Doctor;
using Hsm.Domain.Models.Dtos.Hospital;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hsm.Application.Cqrs.Queries.Handlers
{
    public class GetAllHospitalsByClinicQueryHandler(IReadRepository<Clinical> _readRepo) : IEventHandler<GetAllHospitalsByClinicQueryRequest, GetAllHospitalsByClinicQueryResponse>
    {
        public async Task<GetAllHospitalsByClinicQueryResponse> Handle(GetAllHospitalsByClinicQueryRequest @event)
        {
            Specification<Clinical> specification = new();
            specification.AsNoTracking = false;
            specification.Skip = @event.PageSize;
            specification.Take = @event.PageNumber;
            specification.Conditions.Add(c => c.Hospital.Address.City == @event.ClinicalHospitalsDto.Province && c.Hospital.Address.District == @event.ClinicalHospitalsDto.District);
            specification.Includes = (query => query.Include(w => w.Hospital).ThenInclude(h => h.City));
            Expression<Func<Clinical, HospitalWithDoctorsModel>> selectExpression = clinical => new HospitalWithDoctorsModel
            {
                ClinicalModel = new()
                {
                    Id = clinical.Id,
                    Name = clinical.Name,
                    HospitalId = clinical.HospitalId
                },
                Address = Address.Create(clinical.Hospital.Address.Street, clinical.Hospital.Address.City, clinical.Hospital.Address.State, clinical.Hospital.Address.PostalCode, clinical.Hospital.Address.Country, clinical.Hospital.Address.District),
                CityModel = new()
                {
                    Id = clinical.Hospital.City.Id,
                    IsActive = clinical.Hospital.City.IsActive,
                    Name = clinical.Hospital.City.Name,
                    RowVersion = clinical.Hospital.City.RowVersion
                },
                ContactNumber = clinical.Hospital.ContactNumber,
                IsActive = clinical.Hospital.IsActive,
                RowVersion = clinical.Hospital.RowVersion,
                Id = clinical.Hospital.Id,
                Name = clinical.Hospital.Name,
                DoctorModels = clinical.Hospital.Doctors.Select(doctor => new DoctorModel
                {
                    Id = doctor.Id,
                    AppUserId = doctor.AppUserId,
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName,
                    IsActive = doctor.IsActive,
                    RowVersion = doctor.RowVersion,
                    Schedule = doctor.Schedule,
                    Specialty = doctor.Specialty
                }).ToList()
            };

            List<HospitalWithDoctorsModel> clinicals = await _readRepo.GetListAsync(specification, selectExpression);

            PageResponse<HospitalWithDoctorsModel> pageResponse = new(clinicals, new()
            {
                PageNumber = @event.PageNumber,
                PageSize = @event.PageSize,
                TotalRowCount = clinicals.Count()
            });

            return new(ApiResponseModel<PageResponse<HospitalWithDoctorsModel>>.CreateSuccess(pageResponse));
        }
    }
}
