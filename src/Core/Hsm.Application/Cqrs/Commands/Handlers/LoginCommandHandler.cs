using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Abstractions;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Entities.Identity;
using Hsm.Domain.Models.Dtos.User;
using Hsm.Domain.Models.Response;
using Microsoft.AspNetCore.Identity;

namespace Hsm.Application.Cqrs.Commands.Handlers
{
    public class LoginCommandHandler(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, IUnitOfWork<AppUser> _unitOfWork, IJwtTokenService _jwtTokenService) : IEventHandler<LoginCommandRequest, LoginCommandResponse>
    {
        public async Task<LoginCommandResponse> Handle(LoginCommandRequest @event)
        {
            IReadRepository<AppUser> _readRepository = _unitOfWork.GetReadRepository();
            AppUser? appUser = await _readRepository.GetAsync(u => u.TcNumber == @event.userLoginDto.TcNumber, false);

            if (appUser is null)
                return new LoginCommandResponse(ApiResponseModel<UserDto>.CreateNotFound("User not found"));

            SignInResult? identityResult = await _signInManager.PasswordSignInAsync(appUser, @event.userLoginDto.Password, false, false);

            if (!identityResult.Succeeded)
                return new LoginCommandResponse(ApiResponseModel<UserDto>.CreateNotFound("User not found"));

            if (identityResult.IsLockedOut)
                return new LoginCommandResponse(ApiResponseModel<UserDto>.CreateNotFound("User too many wrong entries"));

            string token = _jwtTokenService.GenerateToken(appUser);

            return new LoginCommandResponse(ApiResponseModel<UserDto>.CreateSuccess(new UserDto()
            {
                Email = appUser.Email,
                FirstName = appUser.UserName,
                LastName = appUser.UserName,
                Token = token
            }));
        }
    }
}
