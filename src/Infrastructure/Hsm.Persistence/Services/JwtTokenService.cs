using Base.Repository.Helpers;
using Hsm.Application.Abstractions;
using Hsm.Domain.Entities.Identity;
using Hsm.Domain.Models.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hsm.Persistence.Services
{
    public class JwtTokenService(IConfiguration _configuration, UserManager<AppUser> _userManager) : IJwtTokenService
    {
        private readonly JwtOptions _jwtTokenConfig = _configuration.GetOptions<JwtOptions>("JwtOptions");

        public async Task<string> GenerateToken(AppUser appUser)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtTokenConfig.Secret));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Tek bir Jti claim ekliyoruz
                    new Claim("Id", appUser.Id.ToString()),
                    new Claim("Email", appUser.Email),
                    new Claim("UserName", appUser.UserName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_jwtTokenConfig.ExpiryMinutes)),
                Issuer = _jwtTokenConfig.Issuer,
                Audience = _jwtTokenConfig.Audience,
                NotBefore = DateTime.UtcNow,
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var roles = await _userManager.GetRolesAsync(appUser);

            foreach (var role in roles)
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GetClaim(string token, string claimType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            var stringClaimValue = securityToken.Claims.First(claim => claim.Type == claimType).Value;

            return stringClaimValue;
        }

        public string GetClaimFromRequest(HttpContext httpContext, string claimType)
        {
            var authorizationHeader = httpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                throw new UnauthorizedAccessException("Authorization token is missing or invalid.");
            }

            var token = authorizationHeader.Substring("Bearer ".Length); // "Bearer " kısmını çıkar
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            var claimValue = securityToken.Claims.FirstOrDefault(claim => claim.Type == claimType)?.Value;

            if (claimValue == null)
            {
                throw new Exception($"Claim '{claimType}' not found in token.");
            }

            return claimValue;
        }
        public string ExtractTokenFromHeader(HttpRequest request)
        {
            var authorizationHeader = request.Headers["Authorization"].FirstOrDefault();

            if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
            {
                return authorizationHeader.Substring("Bearer ".Length).Trim();
            }

            return null;  
        }

        public bool ValidateCurrentToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtTokenConfig.Issuer,
                    ValidAudience = _jwtTokenConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenConfig.Secret)),
                    ClockSkew = TimeSpan.Zero
                };

                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "sub").Value;

            }
            catch (SecurityTokenExpiredException)
            {
                return false;
            }
            catch (SecurityTokenInvalidIssuerException)
            {
                return false;
            }
            catch (SecurityTokenInvalidAudienceException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
