using EventFlux;
using Hsm.Application.Cqrs.Queries.Requests;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Models.Dtos.Clinic;
using Hsm.Domain.Models.Dtos.Doctor;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Hsm.Api.Controllers
{
    public class ClinicController(EventBus _eventBus) : BaseAllowController
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

        [HttpPost]
        [Route("get-all-doctors-by-clinical")]
        public async Task<ActionResult<ApiResponseModel<PageResponse<DoctorModel>>>> GetAllDoctorsByClinical([FromBody] ClinicalDoctorDto clinicalDoctorDto)
        {
            GetAllDoctorsByClinicalRequest getAllDoctorsByClinicalRequest = new(clinicalDoctorDto);
            GetAllDoctorsByClinicalResponse getAllDoctorsByClinicalResponse = await _eventBus.SendAsync(getAllDoctorsByClinicalRequest);

            return Ok(getAllDoctorsByClinicalResponse.ApiResponseModel);
        }

        [HttpPost]
        [Route("get-clinics-by-province-or-district")]
        public async Task<ActionResult<List<ClinicalModel>>> GetClinicsByProvinceOrDistrict([FromBody] GetClinicDto getClinicDto)
        {
            GetClinicsByProvinceOrDistrictQueryRequest getClinicsByProvinceOrDistrictQueryRequest = new(getClinicDto);
            GetClinicsByProvinceOrDistrictQueryResponse getClinicsByProvinceOrDistrictQueryResponse = await _eventBus.SendAsync(getClinicsByProvinceOrDistrictQueryRequest);

            return Ok(getClinicsByProvinceOrDistrictQueryResponse.ApiResponseModel);
        }
    }
}
