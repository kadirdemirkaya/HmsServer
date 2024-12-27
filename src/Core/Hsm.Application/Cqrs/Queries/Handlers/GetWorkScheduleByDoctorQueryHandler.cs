using EfCore.Repository;
using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Queries.Requests;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Appointment;
using Hsm.Domain.Models.Dtos.WorkSchedule;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hsm.Application.Cqrs.Queries.Handlers
{
    public class GetWorkScheduleByDoctorQueryHandler(IReadRepository<Doctor> _readRepo) : IEventHandler<GetWorkScheduleByDoctorQueryRequest, GetWorkScheduleByDoctorQueryResponse>
    {
        public async Task<GetWorkScheduleByDoctorQueryResponse> Handle(GetWorkScheduleByDoctorQueryRequest @event)
        {
            Specification<Doctor> specification = new();
            specification.AsNoTracking = false;
            specification.Conditions.Add(a => a.Id == @event.DoctorId && a.IsActive == true);
            specification.Includes = query => query.Include(d => d.WorkSchedules).ThenInclude(w => w.Appointments);
            Expression<Func<Doctor, DoctorWorkScheduleModel>> selectExpression = doctor => new DoctorWorkScheduleModel
            {
                DoctorModel = new()
                {
                    Id = doctor.Id,
                    AppUserId = doctor.AppUserId,
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName,
                    IsActive = doctor.IsActive,
                    Schedule = doctor.Schedule,
                    Specialty = doctor.Specialty,
                    RowVersion = doctor.RowVersion,
                },
                DoctorWorkScheduleAppointmentsModels = doctor.WorkSchedules.Select(ws => new DoctorWorkScheduleAppointmentsModel
                {
                    WorkScheduleId = ws.Id,
                    IsActive = ws.IsActive,
                    Name = ws.Name,
                    StartDate = ws.StartDate,
                    EndDate = ws.EndDate,
                    RowVersion = ws.RowVersion,
                    AppointmentModel = ws.Appointments.Where(w => w.IsActive == true).Select(ap => new AppointmentModel
                    {
                        Id = ap.Id,
                        IsActive = ap.IsActive,
                        AppointmentTime = ap.AppointmentTime,
                        RowVersion = ap.RowVersion,
                        UserId = ap.UserId
                    }).FirstOrDefault()
                }).ToList()
            };

            DoctorWorkScheduleModel doctorWorkScheduleModel = await _readRepo.GetAsync(specification, selectExpression);

            return new(ApiResponseModel<DoctorWorkScheduleModel>.CreateSuccess(doctorWorkScheduleModel));
        }
    }
}
