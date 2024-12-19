using EventFlux;
using Hsm.Domain.Models.Dtos.Hospital;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;

namespace Hsm.Application.Cqrs.Queries.Responses
{
    public class GetAllHospitalQueryResponse : IEventResponse
    {
        public ApiResponseModel<PageResponse<HospitalModel>> ApiResponseModel { get; set; }

        public GetAllHospitalQueryResponse(ApiResponseModel<PageResponse<HospitalModel>> apiResponseModel)
        {
            ApiResponseModel = apiResponseModel;
        }
    }
}
