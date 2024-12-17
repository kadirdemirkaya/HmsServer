using Hsm.Domain.Entities.Identity;

namespace Hsm.Application.Abstractions
{
    public interface IJwtTokenService
    {
        string GenerateToken(AppUser appUser);
        bool ValidateCurrentToken(string token);
        string GetClaim(string token, string claimType);
    }
}
