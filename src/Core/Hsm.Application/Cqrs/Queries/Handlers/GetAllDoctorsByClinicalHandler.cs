using EfCore.Repository;
using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Queries.Requests;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Clinic;
using Hsm.Domain.Models.Dtos.Doctor;
using Hsm.Domain.Models.Dtos.WorkSchedule;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hsm.Application.Cqrs.Queries.Handlers
{
    public class GetAllDoctorsByClinicalHandler(IReadRepository<Clinical> _readRepo) : IEventHandler<GetAllDoctorsByClinicalRequest, GetAllDoctorsByClinicalResponse>
    {
        public async Task<GetAllDoctorsByClinicalResponse> Handle(GetAllDoctorsByClinicalRequest @event)
        {
            Specification<Clinical> specification = new();
            specification.AsNoTracking = false;
            specification.Skip = @event.PageSize;
            specification.Take = @event.PageNumber;
            specification.Includes = (query => query.Include(w => w.Hospital).ThenInclude(h => h.Doctors).Include(c => c.Hospital.Address));
            specification.Conditions.Add(c => c.Id == @event.ClinicalDoctorDto.ClinicalId && c.Hospital.Address.City == @event.ClinicalDoctorDto.Province && c.Hospital.Address.District == @event.ClinicalDoctorDto.District);

            Expression<Func<Clinical, ClinicalDoctorModel>> selectExpression = clinical => new ClinicalDoctorModel
            {
                DoctorModels = clinical.Hospital.Doctors.Select(doctor => new DoctorModel
                {
                    Id = doctor.Id,
                    IsActive = doctor.IsActive,
                    LastName = doctor.LastName,
                    FirstName = doctor.FirstName,
                    RowVersion = doctor.RowVersion,
                    Schedule = doctor.Schedule,
                    Specialty = doctor.Specialty,
                    AppUserId = doctor.AppUserId,
                }).ToList()
            };

            ClinicalDoctorModel clinicalDoctorModel = await _readRepo.GetAsync(specification, selectExpression);

            if (clinicalDoctorModel is null)
                return new(ApiResponseModel<PageResponse<DoctorModel>>.CreateNotFound("Doctors not found"));

            PageResponse<DoctorModel> pageResponse = new(clinicalDoctorModel.DoctorModels, new()
            {
                PageNumber = @event.PageNumber,
                PageSize = @event.PageSize,
                TotalRowCount = clinicalDoctorModel.DoctorModels.Count()
            });

            return new(ApiResponseModel<PageResponse<DoctorModel>>.CreateSuccess(pageResponse));
        }
    }
}
