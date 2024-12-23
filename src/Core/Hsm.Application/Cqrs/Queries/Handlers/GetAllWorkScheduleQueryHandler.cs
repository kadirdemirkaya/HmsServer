using EfCore.Repository;
using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Queries.Requests;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Application.Extensions;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Appointment;
using Hsm.Domain.Models.Dtos.Doctor;
using Hsm.Domain.Models.Dtos.WorkSchedule;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using ModelMapper;
using System.Linq.Expressions;

namespace Hsm.Application.Cqrs.Queries.Handlers
{
    public class GetAllWorkScheduleQueryHandler(IReadRepository<WorkSchedule> _readRepo) : IEventHandler<GetAllWorkScheduleQueryRequest, GetAllWorkScheduleQueryResponse>
    {
        public async Task<GetAllWorkScheduleQueryResponse> Handle(GetAllWorkScheduleQueryRequest @event)
        {
            Specification<WorkSchedule> specification = new();
            specification.AsNoTracking = false;
            specification.Skip = @event.PageSize;
            specification.Take = @event.PageNumber;
            specification.Includes = (query => query.Include(w => w.Doctor).Include(w => w.Appointments));
            Expression<Func<WorkSchedule, WorkScheduleModel>> selectExpression = workSchedule => new WorkScheduleModel
            {
                Id = workSchedule.Id,
                Name = workSchedule.Name,
                StartDate = workSchedule.StartDate,
                EndDate = workSchedule.EndDate,
                IsActive = workSchedule.IsActive,
                RowVersion = workSchedule.RowVersion,
                DoctorModel = new()
                {
                    Id = workSchedule.Doctor.Id,
                    AppUserId = workSchedule.Doctor.AppUserId,
                    FirstName = workSchedule.Doctor.FirstName,
                    IsActive = workSchedule.Doctor.IsActive,
                    RowVersion = workSchedule.Doctor.RowVersion,
                    LastName = workSchedule.Doctor.LastName,
                    Schedule = workSchedule.Doctor.Schedule,
                    Specialty = workSchedule.Doctor.Specialty,
                },
                AppointmentModels = workSchedule.Appointments.Select(appointment => new AppointmentModel
                {
                    Id = appointment.Id,
                    AppointmentTime = appointment.AppointmentTime,
                    IsActive = appointment.IsActive,
                    UserId = appointment.UserId,
                    RowVersion = appointment.RowVersion
                }).ToList()
            };

            //PageResponse<WorkSchedule> pageWorkSchedule = await _unitOfWork.GetTable().Table.Set<WorkSchedule>().Include(w => w.Doctor).Include(w => w.Appointments).GetPage(@event.PageNumber, @event.PageSize);

            List<WorkScheduleModel> workScheduleModels = await _readRepo.GetListAsync(specification, selectExpression);

            PageResponse<WorkScheduleModel> pageResponse = new(workScheduleModels, new()
            {
                PageNumber = @event.PageNumber,
                PageSize = @event.PageSize,
                TotalRowCount = workScheduleModels.Count()
            });

            return new(ApiResponseModel<PageResponse<WorkScheduleModel>>.CreateSuccess(pageResponse));
        }
    }
}
