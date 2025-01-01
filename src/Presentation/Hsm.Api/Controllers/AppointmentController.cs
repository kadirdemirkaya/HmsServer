using EventFlux;
using Hsm.Application.Abstractions;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Application.Cqrs.Queries.Requests;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Application.Filters;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Entities.Identity;
using Hsm.Domain.Models.Dtos.Appointment;
using Hsm.Domain.Models.Dtos.Hospital;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hsm.Api.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class AppointmentController(EventBus _eventBus, IJwtTokenService _jwtTokenService, IHttpContextAccessor _httpContextAccessor) : BaseAuthController
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

        [HttpPost]
        [Route("search-appointments")]
        public async Task<ActionResult<PageResponse<HospitalWithDoctorsModel>>> SearchAppointments([FromBody] SearchAppointmentDto searchAppointmentDto)
        {
            SearchAppointmentsQueryRequest searchAppointmentsQueryRequest = new(searchAppointmentDto);
            SearchAppointmentsQueryResponse searchAppointmentsQueryResponse = await _eventBus.SendAsync(searchAppointmentsQueryRequest);

            return Ok(searchAppointmentsQueryResponse.ApiResponseModel);
        }

        /// <summary>
        /// Kullanıcı id'si ile herhangi bir kişinin randevuları yada istek atan kişinin(token ile) ait olan aktif randevuları listelenir.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-user-active-appointments")]
        //[ServiceFilter(typeof(GenericNotFoundFilter<AppUser>))]
        public async Task<ActionResult<ApiResponseModel<UserAppointmentsModel>>> GetUserActiveAppointments([FromQuery] Guid? id)
        {
            if (id is null)
                id = Guid.Parse(_jwtTokenService.GetClaimFromRequest(_httpContextAccessor.HttpContext, "Id"));

            GetUserActiveAppointmentsQueryRequest getUserAppointmentsQueryRequest = new(id);
            GetUserActiveAppointmentsQueryResponse getUserAppointmentsQueryResponse = await _eventBus.SendAsync(getUserAppointmentsQueryRequest);

            if (!getUserAppointmentsQueryResponse.ApiResponseModel.Success)
            {
                return NotFound(new { Message = "Active appointments not found for the user." });
            }

            return Ok(getUserAppointmentsQueryResponse.ApiResponseModel);
        }

        /// <summary>
        /// User's all active or past appointments 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-user-all-appointments")]
        //[ServiceFilter(typeof(GenericNotFoundFilter<AppUser>))]
        public async Task<ActionResult<ApiResponseModel<PageResponse<UserAppointmentsModel>>>> GetUserAllAppointments([FromQuery] Guid? id)
        {
            if (id is null)
                id = Guid.Parse(_jwtTokenService.GetClaimFromRequest(_httpContextAccessor.HttpContext, "Id"));

            GetUserAllAppointmentsQueryRequest getUserAllAppointmentsQueryRequest = new(id);
            GetUserAllAppointmentsQueryResponse getUserAllAppointmentsQueryResponse = await _eventBus.SendAsync(getUserAllAppointmentsQueryRequest);

            if (!getUserAllAppointmentsQueryResponse.ApiResponseModel.Success)
            {
                return NotFound(new { Message = "Active appointments not found for the user." });
            }

            return Ok(getUserAllAppointmentsQueryResponse.ApiResponseModel);
        }
    }
}
