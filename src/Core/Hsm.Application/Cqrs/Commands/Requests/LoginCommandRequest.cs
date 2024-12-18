using EventFlux;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Models.Dtos.User;

namespace Hsm.Application.Cqrs.Commands.Requests
{
    public class LoginCommandRequest : IEventRequest<LoginCommandResponse>
    {
        public UserLoginDto userLoginDto { get; set; }

        public LoginCommandRequest(UserLoginDto userLoginDto)
        {
            this.userLoginDto = userLoginDto;
        }
    }
}
