using EventFlux;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Entities.Identity;
using Hsm.Domain.Models.Response;
using Microsoft.AspNetCore.Identity;

namespace Hsm.Application.Cqrs.Commands.Handlers
{
    public class EmailConfirmationCommandHandler(UserManager<AppUser> _userManager) : IEventHandler<EmailConfirmationCommandRequest, EmailConfirmationCommandResponse>
    {
        public async Task<EmailConfirmationCommandResponse> Handle(EmailConfirmationCommandRequest @event)
        {
            var user = await _userManager.FindByEmailAsync(@event.Email);
            if (user == null)
                return new EmailConfirmationCommandResponse(
                    ApiResponseModel<bool>.CreateFailure("User not found!")
                );

            var claims = await _userManager.GetClaimsAsync(user);
            var verificationCodeClaim = claims.FirstOrDefault(c => c.Type == "VerificationCode");

            if (verificationCodeClaim == null)
                return new EmailConfirmationCommandResponse(ApiResponseModel<bool>.CreateFailure("Verification code not found!"));

            if (verificationCodeClaim.Value != @event.Code.ToString())
                return new EmailConfirmationCommandResponse(ApiResponseModel<bool>.CreateFailure("Verification code is invalid!"));

            user.EmailConfirmed = true;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
                return new EmailConfirmationCommandResponse(ApiResponseModel<bool>.CreateFailure("Failed to update user!"));

            var removeClaimResult = await _userManager.RemoveClaimAsync(user, verificationCodeClaim);
            if (!removeClaimResult.Succeeded)
                return new EmailConfirmationCommandResponse(ApiResponseModel<bool>.CreateFailure("User confirmed, but failed to remove verification code claim."));

            return new EmailConfirmationCommandResponse(ApiResponseModel<bool>.CreateSuccess(true));
        }
    }
}
