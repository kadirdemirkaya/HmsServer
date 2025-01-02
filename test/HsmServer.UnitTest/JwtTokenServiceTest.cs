using Base.Repository.Helpers;
using Hsm.Domain.Entities.Identity;
using Hsm.Domain.Models.Options;
using Hsm.Persistence.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HsmServer.UnitTest
{
    public class JwtTokenServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IConfigurationSection> _mockConfigurationSection;
        private readonly Mock<UserManager<AppUser>> _mockUserManager;
        private readonly JwtTokenService _jwtTokenService;
        private readonly JwtOptions _jwtOptions;

        public JwtTokenServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfigurationSection = new Mock<IConfigurationSection>();
            _mockUserManager = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);

            _jwtOptions = new JwtOptions
            {
                Secret = "supersecretkey12345",
                ExpiryMinutes = "60",
                Issuer = "TestIssuer",
                Audience = "TestAudience"
            };

            _mockConfigurationSection.Setup(x => x["Secret"]).Returns(_jwtOptions.Secret);
            _mockConfigurationSection.Setup(x => x["ExpiryMinutes"]).Returns(_jwtOptions.ExpiryMinutes);
            _mockConfigurationSection.Setup(x => x["Issuer"]).Returns(_jwtOptions.Issuer);
            _mockConfigurationSection.Setup(x => x["Audience"]).Returns(_jwtOptions.Audience);

            _mockConfiguration.Setup(x => x.GetSection("JwtOptions")).Returns(_mockConfigurationSection.Object);

            _jwtTokenService = new JwtTokenService(_mockConfiguration.Object, _mockUserManager.Object);
        }

        [Fact]
        public async Task GenerateToken_ShouldReturnToken()
        {
            // Arrange
            var appUser = new AppUser
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                UserName = "testuser"
            };

            _mockUserManager.Setup(x => x.GetRolesAsync(appUser)).ReturnsAsync(new List<string> { "Admin" });

            // Act
            var token = await _jwtTokenService.GenerateToken(appUser);

            // Assert
            Assert.NotNull(token);
            Assert.IsType<string>(token);
        }

        [Fact]
        public void GetClaim_ShouldReturnClaimValue()
        {
            // Arrange
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Secret));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim("TestClaim", "TestValue")
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Act
            var claimValue = _jwtTokenService.GetClaim(tokenString, "TestClaim");

            // Assert
            Assert.Equal("TestValue", claimValue);
        }

        [Fact]
        public void GetClaimFromRequest_ShouldReturnClaimValue()
        {
            // Arrange
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Secret));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim("TestClaim", "TestValue")
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer " + tokenString;

            // Act
            var claimValue = _jwtTokenService.GetClaimFromRequest(httpContext, "TestClaim");

            // Assert
            Assert.Equal("TestValue", claimValue);
        }

        [Fact]
        public void ValidateCurrentToken_ShouldReturnTrueForValidToken()
        {
            // Arrange
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Secret));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Act
            var isValid = _jwtTokenService.ValidateCurrentToken(tokenString);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void ValidateCurrentToken_ShouldReturnFalseForInvalidToken()
        {
            // Arrange
            var invalidToken = "invalid.token.string";

            // Act
            var isValid = _jwtTokenService.ValidateCurrentToken(invalidToken);

            // Assert
            Assert.False(isValid);
        }
    }
}
