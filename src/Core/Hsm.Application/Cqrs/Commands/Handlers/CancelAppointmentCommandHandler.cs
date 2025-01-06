using EfCore.Repository;
using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Abstractions;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Entities.Identity;
using Hsm.Domain.Models.Constants;
using Hsm.Domain.Models.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hsm.Application.Cqrs.Commands.Handlers
{
    public class CancelAppointmentCommandHandler(IReadRepository<Appointment> _readRepo, IWriteRepository<Appointment> _writeRepo, IMailService _mailService, UserManager<AppUser> _userManager) : IEventHandler<CancelAppointmentCommandRequest, CancelAppointmentCommandResponse>
    {
        public async Task<CancelAppointmentCommandResponse> Handle(CancelAppointmentCommandRequest @event)
        {
            Specification<Appointment> specification = new();
            specification.AsNoTracking = false;
            specification.Conditions.Add(a => a.Id == @event.AppointmentId);
            specification.Conditions.Add(a => a.IsActive == true);
            specification.Includes = query => query.Include(h => h.WorkSchedule).ThenInclude(wc => wc.Doctor).ThenInclude(d => d.Hospital).ThenInclude(h => h.Clinicals).Include(a => a.User);

            Appointment? existAppointment = await _readRepo.GetAsync(specification);

            if (existAppointment is not null)
            {
                existAppointment.SetIsActive(false);

                existAppointment.WorkSchedule.SetIsActive(true);

                bool updateResponse = await _writeRepo.UpdateAsync(existAppointment);

                if (updateResponse)
                {
                    AppUser user = await _userManager.FindByIdAsync(existAppointment.UserId.ToString());

                    _mailService.SendMessageAsync(user.Email, "Cancel Appointment", Constant.HtmlBodies.CancelAppointment(existAppointment.WorkSchedule.Doctor.Hospital.Name, existAppointment.WorkSchedule.GetStartDateForMail(), existAppointment.WorkSchedule.Doctor.Hospital.Clinicals.FirstOrDefault().Name, existAppointment.WorkSchedule.Doctor.GetFullName()));
                }

                return updateResponse is true ? new(ApiResponseModel<bool>.CreateSuccess(updateResponse)) : new(ApiResponseModel<bool>.CreateFailure());
            }
            return new(ApiResponseModel<bool>.CreateNotFound());
        }
    }
}
