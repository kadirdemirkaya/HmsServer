using Base.Repository.Helpers;
using Hsm.Application.Abstractions;
using Hsm.Domain.Entities.Identity;
using Hsm.Domain.Models.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hsm.Persistence.Services
{
    public class JwtTokenService(IConfiguration _configuration) : IJwtTokenService
    {
        private readonly JwtOptions _jwtTokenConfig = _configuration.GetOptions<JwtOptions>("JwtOptions");

        public string GenerateToken(AppUser appUser)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtTokenConfig.Secret));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_jwtTokenConfig.ExpiryMinutes)),
                Issuer = _jwtTokenConfig.Issuer,
                Audience = _jwtTokenConfig.Audience,
                NotBefore = DateTime.UtcNow,
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

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
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
