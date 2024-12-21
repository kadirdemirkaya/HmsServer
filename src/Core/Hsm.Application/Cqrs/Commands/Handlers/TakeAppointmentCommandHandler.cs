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
    public class TakeAppointmentCommandHandler(IWriteRepository<Appointment> _writeRepo, IReadRepository<Appointment> _readRepo, IJwtTokenService _jwtTokenService, IHttpContextAccessor _httpContextAccessor) : IEventHandler<TakeAppointmentCommandRequest, TakeAppointmentCommandResponse>
    {
        public async Task<TakeAppointmentCommandResponse> Handle(TakeAppointmentCommandRequest @event)
        {
            Guid id = Guid.Parse(_jwtTokenService.GetClaimFromRequest(_httpContextAccessor.HttpContext, "Id"));

            Appointment oldappointment = await _readRepo.GetAsync(a => a.UserId == id);

            if (oldappointment is null) // new appointment 
            {
                Appointment newAppoinment = ModelMap.Map<TakeAppointmentDto, Appointment>(@event.TakeAppointmentDto);
                newAppoinment.SetUserId(id);

                await _writeRepo.AddAsync(newAppoinment);

                bool createResponse = await _writeRepo.SaveChangesAsync();

                return createResponse is true ? new(ApiResponseModel<bool>.CreateSuccess(createResponse)) : new(ApiResponseModel<bool>.CreateFailure());
            }
            else // old appointment is cancel
            {
                Appointment newAppoinment = ModelMap.Map<TakeAppointmentDto, Appointment>(@event.TakeAppointmentDto);
                newAppoinment.SetUserId(id);

                oldappointment.SetIsActive(false);
                bool updateResponse = await _writeRepo.UpdateAsync(oldappointment);

                await _writeRepo.AddAsync(newAppoinment);

                bool createResponse = await _writeRepo.SaveChangesAsync();

                return createResponse is true ? new(ApiResponseModel<bool>.CreateSuccess(createResponse)) : new(ApiResponseModel<bool>.CreateFailure());
            }
        }
    }
}
