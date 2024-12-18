using Hsm.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Hsm.Persistence.Context
{
    public static class HsmSeedDb
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            var roles = new[] { "Admin", "Moderator", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new AppRole { Name = role, NormalizedName = role.ToUpper() });
                }
            }

            var defaultUsers = new List<AppUser>
            {
                new()
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    EmailConfirmed = true,
                    TcNumber = "2223335566"
                },
                new()
                {
                    UserName = "user@example.com",
                    Email = "user@example.com",
                    EmailConfirmed = true,
                    TcNumber = "1123435577"
                }
            };

            foreach (var defaultUser in defaultUsers)
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    var result = await userManager.CreateAsync(defaultUser, "Admin123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(defaultUser, "Admin");
                    }
                }
            }
        }
    }

}
