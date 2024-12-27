using EventFlux;
using Hsm.Domain.Models.Dtos.Clinic;
using Hsm.Domain.Models.Response;

namespace Hsm.Application.Cqrs.Queries.Responses
{
    public class GetClinicsByProvinceOrDistrictQueryResponse : IEventResponse
    {
        public ApiResponseModel<List<ClinicalModel>> ApiResponseModel { get; set; }

        public GetClinicsByProvinceOrDistrictQueryResponse(ApiResponseModel<List<ClinicalModel>> apiResponseModel)
        {
            ApiResponseModel = apiResponseModel;
        }
    }
}
