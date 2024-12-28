using EfCore.Repository;
using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Queries.Requests;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Doctor;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hsm.Application.Cqrs.Queries.Handlers
{
    public class GetDoctorQueryHandler(IReadRepository<Doctor> _readRepo) : IEventHandler<GetDoctorQueryRequest, GetDoctorQueryResponse>
    {
        public async Task<GetDoctorQueryResponse> Handle(GetDoctorQueryRequest @event)
        {
            Specification<Doctor> specification = new();
            specification.AsNoTracking = false;
            specification.Includes = query => query.Include(d => d.Hospital).ThenInclude(h => h.Clinicals);
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

            if (@event.GetDoctorDto.HospitalId != null)
                specification.Conditions.Add(d => d.HospitalId == @event.GetDoctorDto.HospitalId);
            if (@event.GetDoctorDto.ClinicalId != null)
                specification.Conditions.Add(d => d.Hospital.Clinicals.Any(c => c.Id == @event.GetDoctorDto.ClinicalId));

            List<DoctorModel> doctorModels = await _readRepo.GetListAsync(specification, selectExpression);

            if (doctorModels is null)
                return new(ApiResponseModel<List<DoctorModel>>.CreateNotFound("Doctors not found"));

            return new(ApiResponseModel<List<DoctorModel>>.CreateSuccess(doctorModels));
        }
    }
}
