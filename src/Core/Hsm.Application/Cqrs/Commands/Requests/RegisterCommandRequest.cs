using EventFlux;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Models.Dtos.User;

namespace Hsm.Application.Cqrs.Commands.Requests
{
    public class RegisterCommandRequest : IEventRequest<RegisterCommandResponse>
    {
        public UserRegisterDto UserRegisterDto { get; set; }

        public RegisterCommandRequest(UserRegisterDto userRegisterDto)
        {
            UserRegisterDto = userRegisterDto;
        }
    }
}
