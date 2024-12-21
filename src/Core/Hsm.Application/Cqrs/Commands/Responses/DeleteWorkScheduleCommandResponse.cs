using EventFlux;
using Hsm.Domain.Models.Response;

namespace Hsm.Application.Cqrs.Commands.Responses
{
    public class DeleteWorkScheduleCommandResponse : IEventResponse
    {
        public ApiResponseModel<bool> ApiResponseModel { get; set; }

        public DeleteWorkScheduleCommandResponse(ApiResponseModel<bool> apiResponseModel)
        {
            ApiResponseModel = apiResponseModel;
        }
    }
}
