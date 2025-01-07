using EventFlux;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Models.Dtos.User;
using Microsoft.AspNetCore.Mvc;

namespace Hsm.Api.Controllers
{
    public class AuthenticationController : BaseController
    {
        private readonly EventBus _eventBus;

        public AuthenticationController(EventBus eventBus)
        {
            _eventBus = eventBus;
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

        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            RegisterCommandRequest registerCommandRequest = new(userRegisterDto);
            RegisterCommandResponse registerCommandResponse = await _eventBus.SendAsync(registerCommandRequest);

            if (!registerCommandResponse.ApiResponseModel.Success)
                return BadRequest(registerCommandResponse.ApiResponseModel);

            return Ok(registerCommandResponse.ApiResponseModel);
        }

        [HttpPost]
        [Route("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] int code)
        {
            EmailConfirmationCommandRequest emailConfirmationCommandRequest = new(email, code);
            EmailConfirmationCommandResponse emailConfirmationCommandResponse = await _eventBus.SendAsync(emailConfirmationCommandRequest);

            if (!emailConfirmationCommandResponse.ApiResponseModel.Success)
                return BadRequest(emailConfirmationCommandResponse.ApiResponseModel);

            return Ok(emailConfirmationCommandResponse.ApiResponseModel);
        }
    }
}
