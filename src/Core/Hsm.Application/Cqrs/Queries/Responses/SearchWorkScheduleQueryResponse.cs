using EventFlux;
using Hsm.Domain.Models.Dtos.WorkSchedule;
using Hsm.Domain.Models.Response;

namespace Hsm.Application.Cqrs.Queries.Responses
{
    public class SearchWorkScheduleQueryResponse : IEventResponse
    {
        public ApiResponseModel<List<SearchWorkScheduleModel>> ApiResponseModel { get; set; }

        public SearchWorkScheduleQueryResponse(ApiResponseModel<List<SearchWorkScheduleModel>> apiResponseModel)
        {
            ApiResponseModel = apiResponseModel;
        }
    }
}
