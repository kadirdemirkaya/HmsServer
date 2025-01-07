using EventFlux;
using Hsm.Domain.Models.Response;

namespace Hsm.Application.Cqrs.Commands.Responses
{
    public class EmailConfirmationCommandResponse : IEventResponse
    {
        public ApiResponseModel<bool> ApiResponseModel { get; set; }

        public EmailConfirmationCommandResponse(ApiResponseModel<bool> apiResponseModel)
        {
            ApiResponseModel = apiResponseModel;
        }
    }
}
