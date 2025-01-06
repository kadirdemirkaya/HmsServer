using EfCore.Repository.Extensions;
using Hsm.Application.Abstractions;
using Hsm.Application.Extensions;
using Hsm.Domain.Entities.Base;
using Hsm.Domain.Entities.Identity;
using Hsm.Domain.Models.Options;
using Hsm.Persistence.Context;
using Hsm.Persistence.Repository;
using Hsm.Persistence.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace Hsm.Persistence.Extensions
{
    public static class PersistenceLayerExtension
    {
        public static IServiceCollection LoadPersistenceLayerExtension(this IServiceCollection services, Assembly assembly)
        {
            SqlServerOptions sqlOptions = services.GetOptions<SqlServerOptions>("SqlServerOptions");
            JwtOptions jwtOptions = services.GetOptions<JwtOptions>("JwtOptions");

            services.AddDbContext<HsmDbContext>(opt => opt.UseNpgsql(sqlOptions.SqlConnection));

            services.EfCoreRepositoryServiceRegistration<IBaseEntity, HsmDbContext>(ServiceLifetime.Scoped, assembly);

            services.AddScoped<IJwtTokenService, JwtTokenService>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IMailService, MailService>();

            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequiredUniqueChars = 2;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(60);
                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 7;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequiredUniqueChars = 1;
                opt.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789ğüşıöçĞÜŞİÖÇ-._@";

            })
                .AddUserManager<UserManager<AppUser>>()
                .AddRoleManager<RoleManager<AppRole>>()
                .AddSignInManager<SignInManager<AppUser>>()
                .AddEntityFrameworkStores<HsmDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("Token successfully validated");
                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }

        public static IApplicationBuilder LoadPersistenceLayerApplicationExtension(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    HsmSeedDb.SeedAsync(services).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<HsmDbContext>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            return app;
        }
    }
}
