using EventFlux;
using Hsm.Domain.Models.Dtos.User;
using Hsm.Domain.Models.Response;
using System.Reflection.Metadata.Ecma335;

namespace Hsm.Application.Cqrs.Commands.Responses
{
    public class LoginCommandResponse : IEventResponse
    {
        public ApiResponseModel<UserDto> ApiResponseModel { get; set; }

        public LoginCommandResponse(ApiResponseModel<UserDto> apiResponseModel)
        {
            ApiResponseModel = apiResponseModel;
        }
    }
}
