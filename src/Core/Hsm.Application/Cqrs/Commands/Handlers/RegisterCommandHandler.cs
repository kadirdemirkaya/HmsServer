using EventFlux;
using Hsm.Application.Abstractions;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Entities.Identity;
using Hsm.Domain.Models.Constants;
using Hsm.Domain.Models.Response;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hsm.Application.Cqrs.Commands.Handlers
{
    public class RegisterCommandHandler(UserManager<AppUser> _userManager, IMailService _mailService) : IEventHandler<RegisterCommandRequest, RegisterCommandResponse>
    {
        public async Task<RegisterCommandResponse> Handle(RegisterCommandRequest @event)
        {
            if (!string.Equals(@event.UserRegisterDto.Password, @event.UserRegisterDto.ConfirmPassword))
                return new(ApiResponseModel<bool>.CreateFailure("User password and confirm password do not match!"));

            string verificationCode = new Random().Next(100000, 999999).ToString();

            AppUser appUser = new()
            {
                UserName = $"{@event.UserRegisterDto.FirstName}{@event.UserRegisterDto.LastName}",
                Email = @event.UserRegisterDto.Email,
                EmailConfirmed = false,
                TcNumber = @event.UserRegisterDto.TcNumber
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, @event.UserRegisterDto.Password);

            if (result.Succeeded)
            {
                var existingClaims = await _userManager.GetClaimsAsync(appUser);

                var existingVerificationCodeClaim = existingClaims.FirstOrDefault(c => c.Type == "VerificationCode");
                if (existingVerificationCodeClaim != null)
                {
                    var removeResult = await _userManager.RemoveClaimAsync(appUser, existingVerificationCodeClaim);

                    if (!removeResult.Succeeded)
                        return new(ApiResponseModel<bool>.CreateFailure("Failed to remove existing verification code claim!"));
                }

                IdentityResult addClaimResult = await _userManager.AddClaimAsync(appUser, new Claim("VerificationCode", verificationCode));

                if (!addClaimResult.Succeeded)
                    return new(ApiResponseModel<bool>.CreateFailure("Failed to add verification code claim!"));

                IdentityResult roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                if (roleResult.Succeeded)
                {
                    await _mailService.SendMessageAsync(appUser.Email, "HRYS Web Site Verification Code", Constant.HtmlBodies.EmailCode(verificationCode));

                    return new(ApiResponseModel<bool>.CreateSuccess(true));
                }
            }

            return new(ApiResponseModel<bool>.CreateFailure());
        }
    }
}
