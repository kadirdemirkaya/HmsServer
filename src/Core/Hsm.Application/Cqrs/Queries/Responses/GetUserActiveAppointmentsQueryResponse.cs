using EventFlux;
using Hsm.Domain.Models.Dtos.Appointment;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;

namespace Hsm.Application.Cqrs.Queries.Responses
{
    public class GetUserActiveAppointmentsQueryResponse : IEventResponse
    {
        public ApiResponseModel<PageResponse<UserAppointmentsModel>> ApiResponseModel { get; set; }

        public GetUserActiveAppointmentsQueryResponse(ApiResponseModel<PageResponse<UserAppointmentsModel>> apiResponseModel)
        {
            ApiResponseModel = apiResponseModel;
        }
    }
}
