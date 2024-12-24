using Hsm.Domain.Entities.Identity;
using Hsm.Domain.Models.Dtos.Hospital;
using Hsm.Domain.Models.Dtos.User;
using Hsm.Domain.Models.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace HsmServer.IntegrationTest
{
    public class HospitalControllerTest : IClassFixture<InMemoryWebApplicationFactory<Program>>
    {
        private InMemoryWebApplicationFactory<Program> _factory;
        private readonly IServiceProvider _serviceProvider;
        public HospitalControllerTest(InMemoryWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _serviceProvider = _factory.Services;
        }

        private async Task<string> GetJwtTokenAsync()
        {
            var loginDto = new UserLoginDto
            {
                TcNumber = "12345678910",
                Password = "User_123"
            };

            var userManager = _serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = _serviceProvider.GetRequiredService<RoleManager<AppRole>>();

            var roles = new[] { "Admin", "Moderator", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new AppRole { Name = role, NormalizedName = role.ToUpper() });
                }
            }

            AppUser appUser = new()
            {
                TcNumber = "12345678910",
                Email = "user@gmail.com",
                IsActive = true,
                Id = Guid.NewGuid(),
                UserName = "User",
                EmailConfirmed = true,
                PhoneNumber = "5556667788",
                CreatedDateUTC = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(appUser, "User_123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(appUser, "Admin");

                var client = _factory.CreateClient();
                var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("/api/Authentication/SignIn", content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to get JWT token");
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                ApiResponseModel<UserDto> apiResponseModel = JsonConvert.DeserializeObject<ApiResponseModel<UserDto>>(responseContent);

                return apiResponseModel.Data.Token;
            }
            return string.Empty;
        }

        [Fact]
        public async Task CreateHospital_ReturnOkStatus_WhenDataIsValid()
        {
            CreateHospitalDto createHospitalDto = new()
            {
                AddressDto = new()
                {
                    City = "Istanbul",
                    Country = "Turkiye",
                    PostalCode = "12345",
                    State = "true",
                    Street = "Dere"
                },
                ContactNumber = "06665554433",
                CreateCityDto = new()
                {
                    Name = "Istanbul",
                },
                Name = "Umraniye devlet 2",
            };

            string token = await GetJwtTokenAsync();

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var httpContent = new StringContent(JsonConvert.SerializeObject(createHospitalDto), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/hospital/create-hospital", httpContent);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
