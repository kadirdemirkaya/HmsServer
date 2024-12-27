using EventFlux;
using Hsm.Domain.Models.Dtos.Hospital;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;

namespace Hsm.Application.Cqrs.Queries.Responses
{
    public class SearchAppointmentsQueryResponse : IEventResponse
    {
        public ApiResponseModel<PageResponse<HospitalWithDoctorsModel>> ApiResponseModel { get; set; }

        public SearchAppointmentsQueryResponse(ApiResponseModel<PageResponse<HospitalWithDoctorsModel>> apiResponseModel)
        {
            ApiResponseModel = apiResponseModel;
        }
    }
}
