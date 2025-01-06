using Hsm.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;

namespace Hsm.Application.Abstractions
{
    public interface IJwtTokenService
    {
        Task<string> GenerateToken(AppUser appUser);
        bool ValidateCurrentToken(string token);
        string GetClaim(string token, string claimType);
        string GetClaimFromRequest(HttpContext httpContext, string claimType);
        string ExtractTokenFromHeader(HttpRequest request);

    }
}
