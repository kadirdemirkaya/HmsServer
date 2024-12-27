using EventFlux;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Application.Cqrs.Queries.Requests;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Application.Filters;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.WorkSchedule;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Hsm.Api.Controllers
{
    public class WorkScheduleController(EventBus _eventBus) : BaseAllowController
    {
        [HttpPost]
        [Route("create-workschedule")]
        public async Task<IActionResult> CreateWorkSchedule([FromBody] CreateWorkScheduleDto workScheduleDto)
        {
            CreateWorkScheduleRequest createWorkScheduleRequest = new(workScheduleDto);
            CreateWorkScheduleResponse createWorkScheduleResponse = await _eventBus.SendAsync(createWorkScheduleRequest);

            return Ok(createWorkScheduleResponse.ApiResponseModel);
        }

        [HttpDelete]
        [Route("delete-workschedule/{id}")]
        [ServiceFilter(typeof(GenericNotFoundFilter<WorkSchedule>))]
        public async Task<IActionResult> DeleteWorkSchedule(Guid id)
        {
            DeleteWorkScheduleCommandRequest deleteWorkScheduleCommandRequest = new(id);
            DeleteWorkScheduleCommandResponse deleteWorkScheduleCommandResponse = await _eventBus.SendAsync(deleteWorkScheduleCommandRequest);

            return Ok(deleteWorkScheduleCommandResponse.ApiResponseModel);
        }

        [HttpGet]
        [Route("get-all-workschedule")]
        public async Task<ActionResult<ApiResponseModel<PageResponse<WorkScheduleModel>>>> GetAllWorkSchedule([FromQuery] GetAllWorkScheduleQueryRequest getAllWorkScheduleQueryRequest)
        {
            GetAllWorkScheduleQueryResponse getAllWorkScheduleQueryResponse = await _eventBus.SendAsync(getAllWorkScheduleQueryRequest);

            return Ok(getAllWorkScheduleQueryResponse.ApiResponseModel);
        }

        /// <summary>
        /// Parameter must be DoctorId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-workschedule-by-doctor/{id}")]
        [ServiceFilter(typeof(GenericNotFoundFilter<Doctor>))] // !
        public async Task<ActionResult<PageResponse<DoctorWorkScheduleModel>>> GetWorkScheduleByDoctor(Guid id)
        {
            GetWorkScheduleByDoctorQueryRequest getWorkScheduleByDoctorQueryRequest = new(id);
            GetWorkScheduleByDoctorQueryResponse getWorkScheduleByDoctorQueryResponse = await _eventBus.SendAsync(getWorkScheduleByDoctorQueryRequest);

            return Ok(getWorkScheduleByDoctorQueryResponse.ApiResponseModel);
        }

        [HttpPut]
        [Route("update-workschedule")]
        [ServiceFilter(typeof(GenericSpecificNotFoundFilter<WorkSchedule>))]
        public async Task<IActionResult> UpdateWorkSchedule([FromBody] UpdateWorkScheduleDto updateWorkScheduleDto)
        {
            UpdateWorkScheduleCommandRequest updateWorkScheduleCommandRequest = new(updateWorkScheduleDto);
            UpdateWorkScheduleCommandResponse updateWorkScheduleCommandResponse = await _eventBus.SendAsync(updateWorkScheduleCommandRequest);

            return Ok(updateWorkScheduleCommandResponse.ApiResponseModel);
        }
    }
}
