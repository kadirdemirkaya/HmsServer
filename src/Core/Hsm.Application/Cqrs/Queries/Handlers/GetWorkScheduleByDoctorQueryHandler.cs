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
            specification.Conditions.Add(a => a.Id == @event.DoctorId);
            specification.Conditions.Add(a => a.IsActive == true);
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
                    //AppointmentModels = ws.Appointments.Select(appointment => new AppointmentModel
                    //{
                    //    Id = appointment.Id,
                    //    AppointmentTime = appointment.AppointmentTime,
                    //    IsActive = appointment.IsActive,
                    //    RowVersion = appointment.RowVersion,
                    //    UserId = appointment.UserId
                    //}).ToList()
                }).ToList()
            };

            DoctorWorkScheduleModel doctorWorkScheduleModel = await _readRepo.GetAsync(specification, selectExpression);

            List<DoctorWorkScheduleAppointmentsModel> doctorWorkScheduleGroupModel = new();

            if (doctorWorkScheduleModel is not null && doctorWorkScheduleModel.DoctorWorkScheduleAppointmentsModels.Count() > 0)
            {
                #region test
                //doctorWorkScheduleGroupModel = doctorWorkScheduleModel
                //  .DoctorWorkScheduleAppointmentsModels
                //  .OrderBy(x => x.StartDate)
                //  .ToList();

                //var groupedByWeekAndDayAndHour = doctorWorkScheduleGroupModel
                //    .GroupBy(x => new
                //    {
                //        WeekNumber = System.Globalization.CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(x.StartDate, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday), // Haftanın numarasını alıyoruz
                //        WeekDay = x.StartDate.DayOfWeek // Haftanın gününe göre grupla
                //    })
                //    .Select(group => new
                //    {
                //        WeekNumber = group.Key.WeekNumber,
                //        WeekDay = group.Key.WeekDay,
                //        // Her gün için saat dilimlerine göre grupla
                //        SchedulesByHour = group
                //            .GroupBy(x => x.StartDate.Hour)  // Saat bazında gruplama
                //            .Select(hourGroup => new
                //            {
                //                Hour = hourGroup.Key,
                //                Schedules = hourGroup.Select(x => new
                //                {
                //                    x.WorkScheduleId,
                //                    x.Name,
                //                    x.StartDate,
                //                    x.EndDate,
                //                    x.RowVersion,
                //                    x.IsActive
                //                })
                //                .OrderBy(x => x.StartDate)
                //                .ToList()
                //            })
                //            .ToList()
                //    })
                //    .OrderBy(x => x.WeekNumber) // Haftaya göre sıralama
                //    .ThenBy(x => x.WeekDay)     // Haftanın gününe göre sıralama
                //    .ToList();
                #endregion

                var groupedByWeekAndDayAndHour = doctorWorkScheduleModel.DoctorWorkScheduleAppointmentsModels
                    .GroupBy(x => new
                    {
                        WeekNumber = System.Globalization.CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(x.StartDate, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday), // Haftanın numarasını alıyoruz
                        WeekDay = x.StartDate.DayOfWeek // Haftanın gününe göre grupla
                    })
                    .Select(group => new DoctorWorkScheduleGroupedModel
                    {
                        WeekNumber = group.Key.WeekNumber,
                        WeekDay = group.Key.WeekDay,
                        SchedulesByHour = group
                            .GroupBy(x => x.StartDate.Hour)  // Saat bazında gruplama
                            .OrderBy(hourGroup => hourGroup.Key)
                            .Select(hourGroup => new DoctorWorkScheduleByHour
                            {
                                Hour = hourGroup.Key,
                                Schedules = hourGroup.Select(x => new DoctorWorkScheduleAppointmentsModel
                                {
                                    WorkScheduleId = x.WorkScheduleId,
                                    Name = x.Name,
                                    StartDate = x.StartDate,
                                    EndDate = x.EndDate,
                                    RowVersion = x.RowVersion,
                                    IsActive = x.IsActive
                                })
                                .OrderBy(x => x.StartDate)
                                .ToList()
                            })
                            .ToList()
                    })
                    .OrderBy(x => x.WeekNumber) // Haftaya göre sıralama
                    .ThenBy(x => x.WeekDay)     // Haftanın gününe göre sıralama
                    .ToList();

                return new(ApiResponseModel<List<DoctorWorkScheduleGroupedModel>>.CreateSuccess(groupedByWeekAndDayAndHour));
            }

            return new(ApiResponseModel<List<DoctorWorkScheduleGroupedModel>>.CreateNotFound());
        }
    }
}
