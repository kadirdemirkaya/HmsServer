using EventFlux;
using Hsm.Domain.Models.Response;

namespace Hsm.Application.Cqrs.Commands.Responses
{
    public class DeleteDoctorCommandResponse : IEventResponse
    {
        public ApiResponseModel<bool> ApiResponseModel { get; set; }

        public DeleteDoctorCommandResponse(ApiResponseModel<bool> apiResponseModel)
        {
            ApiResponseModel = apiResponseModel;
        }
    }
}
