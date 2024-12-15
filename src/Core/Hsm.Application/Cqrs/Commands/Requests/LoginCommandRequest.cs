using EventFlux;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Models.Dtos.User;

namespace Hsm.Application.Cqrs.Commands.Requests
{
    public record LoginCommandRequest(UserLoginDto userLoginDto) : IEventRequest<LoginCommandResponse>;
}
