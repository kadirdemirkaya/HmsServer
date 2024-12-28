using EventFlux;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Application.Cqrs.Queries.Requests;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Application.Filters;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Doctor;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hsm.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DoctorController(EventBus _eventBus) : BaseAuthController
    {
        /// <summary>
        /// This endpoint for create doctor
        /// </summary>
        /// <param name="createDoctorModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create-doctor")]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorDto createDoctorModel)
        {
            CreateDoctorCommandRequest createDoctorCommandRequest = new(createDoctorModel);
            CreateDoctorCommandResponse createDoctorCommandResponse = await _eventBus.SendAsync(createDoctorCommandRequest);

            if (!createDoctorCommandResponse.ApiResponseModel.Success)
                return BadRequest(createDoctorCommandResponse.ApiResponseModel);

            return Ok(createDoctorCommandResponse.ApiResponseModel);
        }

        /// <summary>
        /// Delete doctor with DoctorId property value
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-doctor/{id}")]
        [ServiceFilter(typeof(GenericNotFoundFilter<Doctor>))]
        public async Task<IActionResult> DeleteDoctor([FromQuery] Guid id)
        {
            DeleteDoctorCommandRequest deleteDoctorCommandRequest = new(id);
            DeleteDoctorCommandResponse deleteDoctorCommandResponse = await _eventBus.SendAsync(deleteDoctorCommandRequest);

            if (!deleteDoctorCommandResponse.ApiResponseModel.Success)
                return BadRequest(deleteDoctorCommandResponse.ApiResponseModel);

            return Ok(deleteDoctorCommandResponse.ApiResponseModel);
        }

        /// <summary>
        /// All doctors with pagination
        /// </summary>
        /// <param name="getAllDoctorsQueryRequest"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-all-doctors")]
        public async Task<ActionResult<ApiResponseModel<PageResponse<DoctorModel>>>> GetAllDoctors([FromQuery] GetAllDoctorsQueryRequest getAllDoctorsQueryRequest)
        {
            GetAllDoctorsQueryResponse getAllDoctorsQueryResponse = await _eventBus.SendAsync(getAllDoctorsQueryRequest);

            return Ok(getAllDoctorsQueryResponse.ApiResponseModel);
        }

        [HttpPut]
        [Route("update-doctor")]
        [ServiceFilter(typeof(GenericSpecificNotFoundFilter<Doctor>))]
        public async Task<IActionResult> UpdateDoctor([FromBody] UpdateDoctorDto updateDoctorDto)
        {
            UpdateDoctorCommandRequest updateDoctorCommandRequest = new(updateDoctorDto);
            UpdateDoctorCommandResponse updateDoctorCommandResponse = await _eventBus.SendAsync(updateDoctorCommandRequest);

            if (!updateDoctorCommandResponse.ApiResponseModel.Success)
                return BadRequest(updateDoctorCommandResponse.ApiResponseModel);

            return Ok(updateDoctorCommandResponse.ApiResponseModel);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("get-doctor")]
        public async Task<ActionResult<ApiResponseModel<List<DoctorModel>>>> GetDoctor([FromQuery] Guid hospitalId, [FromQuery] Guid clinicalId)
        {
            GetDoctorQueryRequest getDoctorQueryRequest = new(new() { ClinicalId = clinicalId, HospitalId = hospitalId });
            GetDoctorQueryResponse getDoctorQueryResponse = await _eventBus.SendAsync(getDoctorQueryRequest);

            return Ok(getDoctorQueryResponse.ApiResponseModel);
        }
    }
}
