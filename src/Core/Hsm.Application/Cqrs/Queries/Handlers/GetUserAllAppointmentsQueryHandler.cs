using EfCore.Repository;
using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Queries.Requests;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Appointment;
using Hsm.Domain.Models.Dtos.Doctor;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hsm.Application.Cqrs.Queries.Handlers
{
    public class GetUserAllAppointmentsQueryHandler(IReadRepository<Appointment> _readRepo) : IEventHandler<GetUserAllAppointmentsQueryRequest, GetUserAllAppointmentsQueryResponse>
    {
        public async Task<GetUserAllAppointmentsQueryResponse> Handle(GetUserAllAppointmentsQueryRequest @event)
        {
            Specification<Appointment> specification = new();
            specification.AsNoTracking = false;
            specification.Skip = @event.PageSize;
            specification.Take = @event.PageNumber;
            specification.Conditions.Add(a => a.UserId == @event.UserId);
            specification.Includes = query => query.Include(d => d.User).Include(a => a.WorkSchedule).Include(w => w.WorkSchedule.Doctor);
            Expression<Func<Appointment, UserAppointmentsModel>> selectExpression = appointment => new UserAppointmentsModel
            {
                Id = appointment.Id,
                AppointmentTime = appointment.AppointmentTime,
                IsActive = appointment.IsActive,
                UserModel = new()
                {
                    Id = appointment.User.Id,
                    FirstName = appointment.User.UserName,
                    LastName = appointment.User.UserName,
                    TcNumber = appointment.User.TcNumber,
                },
                WorkScheduleModel = new()
                {
                    Id = appointment.WorkScheduleId,
                    EndDate = appointment.WorkSchedule.EndDate,
                    IsActive = appointment.WorkSchedule.IsActive,
                    Name = appointment.WorkSchedule.Name,
                    RowVersion = appointment.WorkSchedule.RowVersion,
                    StartDate = appointment.WorkSchedule.StartDate,
                    DoctorModel = new()
                    {
                        Id = appointment.WorkSchedule.Doctor.Id,
                        AppUserId = appointment.WorkSchedule.Doctor.AppUserId,
                        FirstName = appointment.WorkSchedule.Doctor.FirstName,
                        LastName = appointment.WorkSchedule.Doctor.LastName,
                        RowVersion = appointment.WorkSchedule.Doctor.RowVersion,
                        IsActive = appointment.WorkSchedule.Doctor.IsActive,
                        Schedule = appointment.WorkSchedule.Doctor.Schedule,
                        Specialty = appointment.WorkSchedule.Doctor.Specialty
                    },
                }
            };

            List<UserAppointmentsModel>? userAppointmentsModels = await _readRepo.GetListAsync(specification, selectExpression);

            if (userAppointmentsModels is null || userAppointmentsModels.Count == 0)
                return new(ApiResponseModel<PageResponse<UserAppointmentsModel>>.CreateNotFound("Doctors not found"));

            PageResponse<UserAppointmentsModel> pageResponse = new(userAppointmentsModels, new()
            {
                PageNumber = @event.PageNumber,
                PageSize = @event.PageSize,
                TotalRowCount = userAppointmentsModels.Count()
            });

            return new(ApiResponseModel<PageResponse<UserAppointmentsModel>>.CreateSuccess(pageResponse));
        }
    }
}
