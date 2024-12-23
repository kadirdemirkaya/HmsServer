using EventFlux;
using Hsm.Domain.Models.Dtos.Clinic;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;

namespace Hsm.Application.Cqrs.Queries.Responses
{
    public class GetAllHospitalsByClinicQueryResponse : IEventResponse
    {
        public ApiResponseModel<PageResponse<ClinicalHospitalsModel>> ApiResponseModel { get; set; }

        public GetAllHospitalsByClinicQueryResponse(ApiResponseModel<PageResponse<ClinicalHospitalsModel>> apiResponseModel)
        {
            ApiResponseModel = apiResponseModel;
        }
    }
}
