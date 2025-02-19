﻿using EfCore.Repository;
using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Queries.Requests;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Appointment;
using Hsm.Domain.Models.Dtos.Doctor;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hsm.Application.Cqrs.Queries.Handlers
{
    public class GetUserActiveAppointmentsQueryHandler(IReadRepository<Appointment> _readRepo) : IEventHandler<GetUserActiveAppointmentsQueryRequest, GetUserActiveAppointmentsQueryResponse>
    {
        public async Task<GetUserActiveAppointmentsQueryResponse> Handle(GetUserActiveAppointmentsQueryRequest @event)
        {
            Specification<Appointment> specification = new();
            specification.AsNoTracking = false;
            specification.Skip = @event.PageSize;
            specification.Take = @event.PageNumber;
            specification.Conditions.Add(a => a.UserId == @event.UserId);
            if (@event.IsActive is true)
            {
                specification.Conditions.Add(a => a.IsActive == true);
                specification.Conditions.Add(a => a.WorkSchedule.IsActive == false);
            }
            else
                specification.Conditions.Add(a => a.IsActive == false);
            specification.Includes = query => query.Include(d => d.User).Include(a => a.WorkSchedule).ThenInclude(w => w.Doctor);

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
                        Specialty = appointment.WorkSchedule.Doctor.Specialty,
                        HospitalModel = new()
                        {
                            Id = appointment.WorkSchedule.Doctor.Hospital.Id,
                            Name = appointment.WorkSchedule.Doctor.Hospital.Name,
                            ContactNumber = appointment.WorkSchedule.Doctor.Hospital.ContactNumber,
                            RowVersion = appointment.WorkSchedule.Doctor.Hospital.RowVersion,
                            IsActive = appointment.WorkSchedule.Doctor.Hospital.IsActive,
                        }
                    },
                }
            };

            List<UserAppointmentsModel> userAppointmentsModels = await _readRepo.GetListAsync(specification, selectExpression);

            if (userAppointmentsModels is null)
                return new(ApiResponseModel<PageResponse<UserAppointmentsModel>>.CreateNotFound("User's active appointment not found !"));

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
