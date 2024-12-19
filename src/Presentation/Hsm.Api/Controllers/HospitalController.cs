using EventFlux;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Application.Cqrs.Queries.Requests;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Models.Dtos.Doctor;
using Hsm.Domain.Models.Dtos.Hospital;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Hsm.Api.Controllers
{
    public class HospitalController(EventBus _eventBus) : BaseController
    {
        /// <summary>
        /// At this endpoint the bool response will be returned
        /// </summary>
        /// <param name="createHospitalDto">Necessary info for hospital create process</param>
        /// <returns>Returns bool data type </returns>
        [HttpPost]
        [Route("create-hospital")]
        public async Task<IActionResult> CreateHospital([FromBody] CreateHospitalDto createHospitalDto)
        {
            CreateHospitalCommandRequest createHospitalCommandRequest = new(createHospitalDto);
            CreateHospitalCommandResponse createHospitalCommandResponse = await _eventBus.SendAsync(createHospitalCommandRequest);

            if (!createHospitalCommandResponse.ApiResponseModel.Success)
                return BadRequest();

            return Ok(createHospitalCommandResponse.ApiResponseModel);
        }

        /// <summary>
        /// "Hospitalid" is required for deletion
        /// </summary>
        /// <param name="id">Doctor's id info</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-hospital/{id}")]
        public async Task<IActionResult> DeleteHospital([FromRoute] Guid id)
        {
            DeleteHospitalCommandRequest deleteHospitalCommandRequest = new(id);
            DeleteHospitalCommandResponse deleteHospitalCommandResponse = await _eventBus.SendAsync(deleteHospitalCommandRequest);

            if (!deleteHospitalCommandResponse.ApiResponseModel.Success)
                return BadRequest();

            return Ok(deleteHospitalCommandResponse.ApiResponseModel);
        }

        /// <summary>
        /// All data is retrieved by pagination
        /// </summary>
        /// <param name="getAllHospitalQueryRequest">It is enough if it is empty</param>
        /// <returns>Returns all hospital information listed</returns>

        [HttpGet]
        [Route("get-all-hospital")]
        public async Task<ActionResult<ApiResponseModel<PageResponse<HospitalModel>>>> GetAllHospital([FromQuery] GetAllHospitalQueryRequest getAllHospitalQueryRequest)
        {
            GetAllHospitalQueryResponse getAllHospitalQueryResponse = await _eventBus.SendAsync(getAllHospitalQueryRequest);

            return Ok(getAllHospitalQueryResponse.ApiResponseModel);
        }

        /// <summary>
        /// Bring all doctors with hospitalid
        /// </summary>
        /// <param name="id">Doctor's id info</param>
        /// <returns>Returns doctor model list </returns>
        [HttpGet]
        [Route("get-doctors-in-hospital")]
        public async Task<ActionResult<ApiResponseModel<PageResponse<DoctorModel>>>> GetDoctorsInHospital([FromQuery] Guid id)
        {
            GetDoctorsInHospitalQueryRequest getDoctorsInHospitalQueryRequest = new(id);
            GetDoctorsInHospitalQueryResponse getDoctorsInHospitalQueryResponse = await _eventBus.SendAsync(getDoctorsInHospitalQueryRequest);

            return Ok(getDoctorsInHospitalQueryResponse.ApiResponseModel);
        }

        /// <summary>
        /// UpdateHospitalDto model request is argument and request sended with rowversion property
        /// </summary>
        /// <param name="updateHospitalDto"></param>
        /// <returns>Return type is bool</returns>
        [HttpPut]
        [Route("update-hospital")]
        public async Task<IActionResult> UpdateHospital([FromBody] UpdateHospitalDto updateHospitalDto)
        {
            UpdateHospitalCommandRequest updateHospitalCommandRequest = new(updateHospitalDto);
            UpdateHospitalCommandResponse updateHospitalCommandResponse = await _eventBus.SendAsync(updateHospitalCommandRequest);

            if (!updateHospitalCommandResponse.ApiResponseModel.Success)
                return BadRequest();

            return Ok(updateHospitalCommandResponse.ApiResponseModel);
        }

    }
}
