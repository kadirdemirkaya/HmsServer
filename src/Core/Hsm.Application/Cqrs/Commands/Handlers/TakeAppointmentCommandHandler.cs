using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Abstractions;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Appointment;
using Hsm.Domain.Models.Response;
using Microsoft.AspNetCore.Http;
using ModelMapper;

namespace Hsm.Application.Cqrs.Commands.Handlers
{
    public class TakeAppointmentCommandHandler(IWriteRepository<Appointment> _writeRepo, IReadRepository<Appointment> _readRepo, IReadRepository<WorkSchedule> _workScheduleReadRepo, IWriteRepository<WorkSchedule> _workScheduleWriteRepo, IJwtTokenService _jwtTokenService, IHttpContextAccessor _httpContextAccessor) : IEventHandler<TakeAppointmentCommandRequest, TakeAppointmentCommandResponse>
    {
        // ---------------------------------------------------
        // user'ın apointment'i yoksa yeni bir tane oluştur
        // sonra appointment için workscheduleid ver appointment oluştur
        // ardından workschedule'ü false et
        // ---------------------------------------------------
        // user'ın appointment'i varsa mevcut olanı iptal et
        // eski workschedule'ü true yap
        // eski appointment'i false yap
        // yeni workschedule'ü appointment için ekle
        // ve eklenen workschedule'ü false yap
        // ---------------------------------------------------
        public async Task<TakeAppointmentCommandResponse> Handle(TakeAppointmentCommandRequest @event)
        {
            Guid id = Guid.Parse(_jwtTokenService.GetClaimFromRequest(_httpContextAccessor.HttpContext, "Id"));

            Appointment oldappointment = await _readRepo.GetAsync(a => a.UserId == id && a.IsActive == true);

            if (oldappointment is null) // new appointment 
            {
                Appointment newAppoinment = ModelMap.Map<TakeAppointmentDto, Appointment>(@event.TakeAppointmentDto);
                newAppoinment.SetUserId(id);

                await _writeRepo.AddAsync(newAppoinment);

                WorkSchedule workSchedule = await _workScheduleReadRepo.GetAsync(w => w.Id == @event.TakeAppointmentDto.WorkScheduleId);
                workSchedule.SetIsActive(false);

                bool updateResponse = await _workScheduleWriteRepo.UpdateAsync(workSchedule);
                //bool createResponse = await _writeRepo.SaveChangesAsync();

                return updateResponse is true ? new(ApiResponseModel<bool>.CreateSuccess(updateResponse)) : new(ApiResponseModel<bool>.CreateFailure());
            }
            else // old appointment is cancel
            {
                Appointment newAppoinment = ModelMap.Map<TakeAppointmentDto, Appointment>(@event.TakeAppointmentDto);
                newAppoinment.SetUserId(id);

                await _writeRepo.AddAsync(newAppoinment);

                oldappointment.SetIsActive(false);

                bool updateAppointResponse = await _writeRepo.UpdateAsync(oldappointment); // new appointment added
                //bool saveResponse = await _writeRepo.SaveChangesAsync();

                WorkSchedule oldWorkSchedule = await _workScheduleReadRepo.GetAsync(w => w.Id == oldappointment.WorkScheduleId && w.IsActive == false);
                oldWorkSchedule.SetIsActive(true);

                bool updateWCResponse = await _workScheduleWriteRepo.UpdateAsync(oldWorkSchedule); // oldwc update

                WorkSchedule workSchedule = await _workScheduleReadRepo.GetAsync(w => w.Id == @event.TakeAppointmentDto.WorkScheduleId);
                workSchedule.SetIsActive(false);

                bool updResponse = await _workScheduleWriteRepo.UpdateAsync(workSchedule); // oldwc update

                return updateAppointResponse && updateWCResponse && updResponse is true ? new(ApiResponseModel<bool>.CreateSuccess(true)) : new(ApiResponseModel<bool>.CreateFailure());
            }
        }
    }
}
