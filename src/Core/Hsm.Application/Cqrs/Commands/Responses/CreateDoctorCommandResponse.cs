using EventFlux;
using Hsm.Domain.Models.Response;

namespace Hsm.Application.Cqrs.Commands.Responses
{
    public class CreateDoctorCommandResponse : IEventResponse
    {
        public ApiResponseModel<bool> ApiResponseModel { get; set; }

        public CreateDoctorCommandResponse(ApiResponseModel<bool> apiResponseModel)
        {
            ApiResponseModel = apiResponseModel;
        }
    }
}
