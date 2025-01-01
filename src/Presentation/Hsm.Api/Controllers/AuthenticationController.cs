using EventFlux;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Entities.Identity;
using Hsm.Domain.Models.Dtos.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Hsm.Api.Controllers
{
    public class AuthenticationController : BaseController
    {
        private readonly EventBus _eventBus;
        private UserManager<AppUser> _userManager;
        private RoleManager<AppRole> _roleManager;

        public AuthenticationController(EventBus eventBus, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _eventBus = eventBus;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("SignIn")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            LoginCommandRequest loginCommandRequest = new(userLoginDto);
            LoginCommandResponse loginCommandResponse = await _eventBus.SendAsync(loginCommandRequest);

            if (!loginCommandResponse.ApiResponseModel.Success)
                return Unauthorized();

            return Ok(loginCommandResponse.ApiResponseModel);
        }
    }
}
