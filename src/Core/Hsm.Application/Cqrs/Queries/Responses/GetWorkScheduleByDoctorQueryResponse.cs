using EventFlux;
using Hsm.Domain.Models.Dtos.WorkSchedule;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;

namespace Hsm.Application.Cqrs.Queries.Responses
{
    public class GetWorkScheduleByDoctorQueryResponse : IEventResponse
    {
        public ApiResponseModel<DoctorWorkScheduleModel> ApiResponseModel { get; set; }

        public GetWorkScheduleByDoctorQueryResponse(ApiResponseModel<DoctorWorkScheduleModel> apiResponseModel)
        {
            ApiResponseModel = apiResponseModel;
        }
    }
}
