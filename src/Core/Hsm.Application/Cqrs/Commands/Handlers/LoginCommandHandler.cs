using EventFlux;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Hsm.Application.Cqrs.Commands.Handlers
{
    public class LoginCommandHandler(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager) : IEventHandler<LoginCommandRequest, LoginCommandResponse>
    {
        public async Task<LoginCommandResponse> Handle(LoginCommandRequest @event)
        {
            return default;                                
        }
    }
}
