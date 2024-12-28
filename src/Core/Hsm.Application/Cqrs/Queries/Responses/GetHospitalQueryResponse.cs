using EventFlux;
using Hsm.Domain.Models.Dtos.Hospital;
using Hsm.Domain.Models.Response;

namespace Hsm.Application.Cqrs.Queries.Responses
{
    public class GetHospitalQueryResponse : IEventResponse
    {
        public  ApiResponseModel<List<HospitalModel>> ApiResponseModel { get; set; }

        public GetHospitalQueryResponse(ApiResponseModel<List<HospitalModel>> apiResponseModel)
        {
            ApiResponseModel = apiResponseModel;
        }
    }
}
