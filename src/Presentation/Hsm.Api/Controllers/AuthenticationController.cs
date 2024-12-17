using EventFlux;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Models.Dtos.User;
using Microsoft.AspNetCore.Mvc;

namespace Hsm.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly EventBus _eventBus;

        public AuthenticationController(EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        [HttpPost]
        [Route("SignUp")]
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
