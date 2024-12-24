using EventFlux;
using Hsm.Domain.Models.Dtos.Clinic;
using Hsm.Domain.Models.Dtos.Doctor;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;

namespace Hsm.Application.Cqrs.Queries.Responses
{
    public class GetAllDoctorsByClinicalResponse : IEventResponse
    {
        public ApiResponseModel<PageResponse<DoctorModel>> ApiResponseModel { get; set; }

        public GetAllDoctorsByClinicalResponse(ApiResponseModel<PageResponse<DoctorModel>> apiResponseModel)
        {
            ApiResponseModel = apiResponseModel;
        }
    }
}
