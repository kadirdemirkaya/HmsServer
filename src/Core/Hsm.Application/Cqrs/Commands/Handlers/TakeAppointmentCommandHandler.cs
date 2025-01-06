using EfCore.Repository;
using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Abstractions;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Entities.Identity;
using Hsm.Domain.Models.Constants;
using Hsm.Domain.Models.Dtos.Appointment;
using Hsm.Domain.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ModelMapper;

namespace Hsm.Application.Cqrs.Commands.Handlers
{
    public class TakeAppointmentCommandHandler(IWriteRepository<Appointment> _writeRepo, IReadRepository<Appointment> _readRepo, IReadRepository<WorkSchedule> _workScheduleReadRepo, IWriteRepository<WorkSchedule> _workScheduleWriteRepo, IJwtTokenService _jwtTokenService, IHttpContextAccessor _httpContextAccessor, IMailService _mailService, UserManager<AppUser> _userManager) : IEventHandler<TakeAppointmentCommandRequest, TakeAppointmentCommandResponse>
    {
        public async Task<TakeAppointmentCommandResponse> Handle(TakeAppointmentCommandRequest @event)
        {
            bool isNewAppointment = true;
            Guid id = Guid.Parse(_jwtTokenService.GetClaimFromRequest(_httpContextAccessor.HttpContext, "Id"));

            Specification<WorkSchedule> workScheduleSpecification = new();
            workScheduleSpecification.AsNoTracking = false;
            workScheduleSpecification.Includes = query => query.Include(h => h.Doctor).ThenInclude(d => d.Hospital).ThenInclude(h => h.Clinicals);
            workScheduleSpecification.Conditions.Add(a => a.Id == @event.TakeAppointmentDto.WorkScheduleId);
            WorkSchedule isExistWorkSchedule = await _workScheduleReadRepo.GetAsync(workScheduleSpecification);

            WorkSchedule existWorkSchedule = await _workScheduleReadRepo.GetAsync(w => w.Id == @event.TakeAppointmentDto.WorkScheduleId);


            Specification<Appointment> appointmentSpecification = new();
            appointmentSpecification.AsNoTracking = false;
            appointmentSpecification.Includes = query => query.Include(h => h.WorkSchedule).ThenInclude(wc => wc.Doctor).ThenInclude(d => d.Hospital).ThenInclude(h => h.Clinicals);
            appointmentSpecification.Conditions.Add(a => a.UserId == id && a.IsActive == true);
            appointmentSpecification.Conditions.Add(a => a.WorkSchedule.Doctor.Hospital.Clinicals.Any(c => c.HospitalId == existWorkSchedule.Doctor.HospitalId));

            Appointment? isExistAppointment = await _readRepo.GetAsync(appointmentSpecification);

            if (isExistAppointment is null) // new appointment 
            {
                Appointment newAppoinment = ModelMap.Map<TakeAppointmentDto, Appointment>(@event.TakeAppointmentDto);
                newAppoinment.SetUserId(id);

                await _writeRepo.AddAsync(newAppoinment);

                WorkSchedule workSchedule = await _workScheduleReadRepo.GetAsync(w => w.Id == @event.TakeAppointmentDto.WorkScheduleId);
                workSchedule.SetIsActive(false);

                bool updateResponse = await _workScheduleWriteRepo.UpdateAsync(workSchedule);

                if (updateResponse is true)
                {
                    AppUser user = await _userManager.FindByIdAsync(id.ToString());

                    string token = _jwtTokenService.ExtractTokenFromHeader(_httpContextAccessor.HttpContext.Request);

                    await SendEmail(user.Email, user.Email, Constant.HtmlBodies.AppointmentAndCancelAppointment(workSchedule.Doctor.Hospital.Name, workSchedule.GetStartDateForMail(), workSchedule.Doctor.Hospital.Clinicals.FirstOrDefault().Name, workSchedule.Doctor.GetFullName(), newAppoinment.Id.ToString(), token));
                }


                return updateResponse is true ? new(ApiResponseModel<bool>.CreateSuccess(updateResponse)) : new(ApiResponseModel<bool>.CreateFailure());
            }
            else // old appointment is cancel
            {
                Appointment newAppoinment = ModelMap.Map<TakeAppointmentDto, Appointment>(@event.TakeAppointmentDto);
                newAppoinment.SetUserId(id);

                await _writeRepo.AddAsync(newAppoinment);

                isExistAppointment.SetIsActive(false);

                bool updateAppointResponse = await _writeRepo.UpdateAsync(isExistAppointment); // new appointment added
                //bool saveResponse = await _writeRepo.SaveChangesAsync();

                WorkSchedule oldWorkSchedule = await _workScheduleReadRepo.GetAsync(w => w.Id == isExistAppointment.WorkScheduleId && w.IsActive == false);
                oldWorkSchedule.SetIsActive(true);

                bool updateWCResponse = await _workScheduleWriteRepo.UpdateAsync(oldWorkSchedule); // oldwc update

                WorkSchedule workSchedule = await _workScheduleReadRepo.GetAsync(w => w.Id == @event.TakeAppointmentDto.WorkScheduleId);
                workSchedule.SetIsActive(false);

                bool updResponse = await _workScheduleWriteRepo.UpdateAsync(workSchedule); // oldwc update

                if (updateAppointResponse && updateWCResponse && updResponse is true)
                {
                    AppUser user = await _userManager.FindByIdAsync(id.ToString());

                    string token = _jwtTokenService.ExtractTokenFromHeader(_httpContextAccessor.HttpContext.Request);

                    await SendEmail(user.Email, user.Email, Constant.HtmlBodies.AppointmentAndCancelAppointment(workSchedule.Doctor.Hospital.Name, workSchedule.GetStartDateForMail(), workSchedule.Doctor.Hospital.Clinicals.FirstOrDefault().Name, workSchedule.Doctor.FirstName, newAppoinment.Id.ToString(), token));
                }

                return updateAppointResponse && updateWCResponse && updResponse is true ? new(ApiResponseModel<bool>.CreateSuccess(true)) : new(ApiResponseModel<bool>.CreateFailure());
            }
        }

        private async Task SendEmail(string email, string subject, string message)
        {
            await _mailService.SendMessageAsync(email, subject, message, true);
        }
    }
}
