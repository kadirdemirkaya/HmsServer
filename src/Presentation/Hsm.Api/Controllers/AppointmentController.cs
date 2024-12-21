using EventFlux;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Application.Filters;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Appointment;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hsm.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Admin,User")]
    public class AppointmentController(EventBus _eventBus) : BaseController
    {
        [HttpPost]
        [Route("take-appointment")]
        public async Task<IActionResult> TakeAppointment([FromBody] TakeAppointmentDto takeAppointmentDto)
        {
            TakeAppointmentCommandRequest takeAppointmentCommandRequest = new(takeAppointmentDto);
            TakeAppointmentCommandResponse takeAppointmentCommandResponse = await _eventBus.SendAsync(takeAppointmentCommandRequest);

            if (!takeAppointmentCommandResponse.ApiResponseModel.Success)
                return BadRequest(takeAppointmentCommandResponse.ApiResponseModel);

            return Ok(takeAppointmentCommandResponse.ApiResponseModel);
        }

        [HttpDelete]
        [Route("cancel-appointment/{id}")]
        [ServiceFilter(typeof(GenericNotFoundFilter<Appointment>))]
        public async Task<IActionResult> CancelAppointment(Guid id)
        {
            CancelAppointmentCommandRequest cancelAppointmentCommandRequest = new(id);
            CancelAppointmentCommandResponse cancelAppointmentCommandResponse = await _eventBus.SendAsync(cancelAppointmentCommandRequest);

            if (!cancelAppointmentCommandResponse.ApiResponseModel.Success)
                return BadRequest(cancelAppointmentCommandResponse.ApiResponseModel);

            return Ok(cancelAppointmentCommandResponse.ApiResponseModel);
        }

    }
}
