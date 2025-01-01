using EfCore.Repository;
using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Queries.Requests;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.WorkSchedule;
using Hsm.Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hsm.Application.Cqrs.Queries.Handlers
{
    public class SearchWorkScheduleQueryHandler(IReadRepository<WorkSchedule> _readRepo) : IEventHandler<SearchWorkScheduleQueryRequest, SearchWorkScheduleQueryResponse>
    {
        public async Task<SearchWorkScheduleQueryResponse> Handle(SearchWorkScheduleQueryRequest @event)
        {
            Specification<WorkSchedule> specification = new();
            specification.AsNoTracking = false;
            specification.Includes = query => query.Include(h => h.Doctor).ThenInclude(d => d.Hospital).ThenInclude(h => h.Clinicals).Include(h => h.Appointments);

            if (@event.SearcWorkScheduleDto.ClinicId != null)
                specification.Conditions.Add(a => a.Doctor.Hospital.Clinicals.Any(c => c.Id == @event.SearcWorkScheduleDto.ClinicId && c.IsActive == true) && a.IsActive == true);

            if (@event.SearcWorkScheduleDto.District != null)
                specification.Conditions.Add(a => a.Doctor.Hospital.Address.District == @event.SearcWorkScheduleDto.District && a.IsActive == true);

            if (@event.SearcWorkScheduleDto.DoctorId != null)
                specification.Conditions.Add(a => a.Doctor.Id == @event.SearcWorkScheduleDto.DoctorId && a.IsActive == true);

            if (@event.SearcWorkScheduleDto.HospitalId != null)
                specification.Conditions.Add(a => a.Doctor.Hospital.Id == @event.SearcWorkScheduleDto.HospitalId && a.IsActive == true);

            if (@event.SearcWorkScheduleDto.Province != null)
                specification.Conditions.Add(a => a.Doctor.Hospital.Address.City == @event.SearcWorkScheduleDto.Province && a.IsActive == true);

            specification.Conditions.Add(wc => wc.StartDate >= DateTime.UtcNow);
            specification.OrderBy = wc => wc.OrderBy(st => st.StartDate);

            Expression<Func<WorkSchedule, SearchWorkScheduleModel>> selectExpression = wc => new SearchWorkScheduleModel
            {
                DoctorId = wc.DoctorId,
                DoctorName = wc.Doctor.GetFullName(),
                HospitalAddress = Address.Create(wc.Doctor.Hospital.Address.Street, wc.Doctor.Hospital.Address.City, wc.Doctor.Hospital.Address.State, wc.Doctor.Hospital.Address.PostalCode, wc.Doctor.Hospital.Address.Country, wc.Doctor.Hospital.Address.District),
                HospitalName = wc.Doctor.Hospital.Name,
                PolicinicalName = wc.Doctor.Hospital.Clinicals.FirstOrDefault().Name,
                NearestWorkScheduleModel = new()
                {
                    Id = wc.Id,
                    StartDate = wc.StartDate,
                    EndDate = wc.EndDate,
                    Name = wc.Name,
                    IsActive = wc.IsActive,
                    RowVersion = wc.RowVersion
                }
            };

            List<SearchWorkScheduleModel> searchWorkScheduleModel = await _readRepo.GetListAsync(specification, selectExpression);

            var nearestSchedulesByDoctor = searchWorkScheduleModel
                .GroupBy(wc => wc.DoctorId)
                .Select(group => group.OrderBy(wc => wc.NearestWorkScheduleModel.StartDate).FirstOrDefault())
                .ToList();

            if (nearestSchedulesByDoctor is null)
                return new(ApiResponseModel<List<SearchWorkScheduleModel>>.CreateNotFound("WorkSchedule is not found"));

            return new(ApiResponseModel<List<SearchWorkScheduleModel>>.CreateSuccess(nearestSchedulesByDoctor));
        }
    }
}
