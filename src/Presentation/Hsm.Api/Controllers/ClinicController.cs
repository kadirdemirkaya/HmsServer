using EventFlux;
using Hsm.Application.Cqrs.Queries.Requests;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Models.Dtos.Clinic;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Hsm.Api.Controllers
{
    public class ClinicController(EventBus _eventBus) : BaseAuthController
    {
        [HttpGet]
        [Route("get-all-clinics")]
        public async Task<ActionResult<ApiResponseModel<PageResponse<ClinicalModel>>>> GetAllClinics()
        {
            GetAllClinicsQueryRequest getAllClinicsQuery = new();
            GetAllClinicsQueryResponse getAllClinicsQueryResponse = await _eventBus.SendAsync(getAllClinicsQuery);

            return Ok(getAllClinicsQueryResponse.ApiResponseModel);
        }

        [HttpPost]
        [Route("get-all-hospitals-by-clinic")]
        public async Task<ActionResult<ApiResponseModel<PageResponse<ClinicalHospitalsModel>>>> GetAllHospitalsByClinic([FromBody] ClinicalHospitalsDto clinicalHospitalsDto)
        {
            GetAllHospitalsByClinicQueryRequest getAllHospitalsByClinicQueryRequest = new(clinicalHospitalsDto);
            GetAllHospitalsByClinicQueryResponse getAllHospitalsByClinicQueryResponse = await _eventBus.SendAsync(getAllHospitalsByClinicQueryRequest);

            return Ok(getAllHospitalsByClinicQueryResponse.ApiResponseModel);
        }
    }
}
