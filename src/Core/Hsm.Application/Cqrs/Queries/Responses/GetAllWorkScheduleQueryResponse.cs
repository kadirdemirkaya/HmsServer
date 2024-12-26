using EventFlux;
using Hsm.Domain.Models.Dtos.Hospital;
using Hsm.Domain.Models.Dtos.WorkSchedule;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;

namespace Hsm.Application.Cqrs.Queries.Responses
{
    public class GetAllWorkScheduleQueryResponse : IEventResponse
    {
        public ApiResponseModel<PageResponse<WorkScheduleModel>> ApiResponseModel { get; set; }

        public GetAllWorkScheduleQueryResponse(ApiResponseModel<PageResponse<WorkScheduleModel>> apiResponseModel)
        {
            ApiResponseModel = apiResponseModel;
        }
    }
}
