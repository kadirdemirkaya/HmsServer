using EventFlux;
using Hsm.Domain.Models.Dtos.Clinic;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;

namespace Hsm.Application.Cqrs.Queries.Responses
{
    public class GetAllClinicsQueryResponse : IEventResponse
    {
        public ApiResponseModel<PageResponse<ClinicalModel>> ApiResponseModel { get; set; }

        public GetAllClinicsQueryResponse(ApiResponseModel<PageResponse<ClinicalModel>> apiResponseModel)
        {
            ApiResponseModel = apiResponseModel;
        }
    }
}
