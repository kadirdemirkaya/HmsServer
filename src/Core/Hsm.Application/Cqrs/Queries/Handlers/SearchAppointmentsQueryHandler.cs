using EfCore.Repository;
using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Queries.Requests;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Appointment;
using Hsm.Domain.Models.Dtos.Clinic;
using Hsm.Domain.Models.Dtos.Doctor;
using Hsm.Domain.Models.Dtos.Hospital;
using Hsm.Domain.Models.Dtos.WorkSchedule;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hsm.Application.Cqrs.Queries.Handlers
{
    public class SearchAppointmentsQueryHandler(IReadRepository<Hospital> _readRepo) : IEventHandler<SearchAppointmentsQueryRequest, SearchAppointmentsQueryResponse>
    {
        public async Task<SearchAppointmentsQueryResponse> Handle(SearchAppointmentsQueryRequest @event)
        {
            Specification<Hospital> specification = new();
            specification.AsNoTracking = false;
            specification.Skip = @event.PageSize;
            specification.Take = @event.PageNumber;
            specification.Conditions.Add(a => a.Address.City == @event.SearchAppointmentDto.Province);
            specification.Includes = query => query.Include(h => h.City).Include(d => d.Clinicals).Include(d => d.Doctors).ThenInclude(w => w.WorkSchedules);

            if (@event.SearchAppointmentDto.District is not null)
                specification.Conditions.Add(a => a.Address.District == @event.SearchAppointmentDto.District);
            if (@event.SearchAppointmentDto.HospitalId is not null)
                specification.Conditions.Add(a => a.Id == @event.SearchAppointmentDto.HospitalId);
            if (@event.SearchAppointmentDto.ClinicId is not null)
                specification.Conditions.Add(a => a.Clinicals.Any(c => c.Id == @event.SearchAppointmentDto.ClinicId));
            if (@event.SearchAppointmentDto.DoctorId is not null)
                specification.Conditions.Add(a => a.Doctors.Any(d => d.Id == @event.SearchAppointmentDto.DoctorId));

            Expression<Func<Hospital, HospitalWithDoctorsModel>> selectExpression = hospital => new HospitalWithDoctorsModel
            {
                Id = hospital.Id,
                Address = Address.Create(hospital.Address.State, hospital.Address.City, hospital.Address.State, hospital.Address.PostalCode, hospital.Address.Country, hospital.Address.District),
                CityModel = new()
                {
                    Id = hospital.City.Id,
                    IsActive = hospital.City.IsActive,
                    Name = hospital.City.Name,
                    RowVersion = hospital.City.RowVersion
                },
                RowVersion = hospital.RowVersion,
                Name = hospital.Name,
                IsActive = hospital.IsActive,
                ContactNumber = hospital.ContactNumber,
                ClinicalModels = hospital.Clinicals.Select(clinic => new ClinicalModel
                {
                    Id = clinic.Id,
                    Name = clinic.Name,
                    HospitalId = clinic.HospitalId
                }).ToList(),
                DoctorModels = hospital.Doctors.Select(doctor => new DoctorModel
                {
                    Id = doctor.Id,
                    AppUserId = doctor.AppUserId,
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName,
                    IsActive = doctor.IsActive,
                    RowVersion = doctor.RowVersion,
                    Schedule = doctor.Schedule,
                    Specialty = doctor.Specialty,
                }).ToList()
            };

            List<HospitalWithDoctorsModel>? hospitalWithDoctorsModels = await _readRepo.GetListAsync(specification, selectExpression);

            if (hospitalWithDoctorsModels is null || !hospitalWithDoctorsModels.Any())
                return new(ApiResponseModel<PageResponse<HospitalWithDoctorsModel>>.CreateNotFound("Doctors not found"));

            PageResponse<HospitalWithDoctorsModel> pageResponse = new(hospitalWithDoctorsModels, new()
            {
                PageNumber = @event.PageNumber,
                PageSize = @event.PageSize,
                TotalRowCount = hospitalWithDoctorsModels.Count()
            });

            return new(ApiResponseModel<PageResponse<HospitalWithDoctorsModel>>.CreateSuccess(pageResponse));
        }
    }
}
