using EventFlux;
using Hsm.Domain.Models.Dtos.Doctor;
using Hsm.Domain.Models.Response;

namespace Hsm.Application.Cqrs.Queries.Responses
{
    public class GetDoctorQueryResponse : IEventResponse
    {
        public ApiResponseModel<List<DoctorModel>> ApiResponseModel { get; set; }

        public GetDoctorQueryResponse(ApiResponseModel<List<DoctorModel>> apiResponseModel)
        {
            ApiResponseModel = apiResponseModel;
        }
    }
}
