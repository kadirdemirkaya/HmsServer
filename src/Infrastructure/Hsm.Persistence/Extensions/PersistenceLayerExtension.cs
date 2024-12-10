using Hsm.Application.Extensions;
using Hsm.Domain.Entities.Identity;
using Hsm.Domain.Models.Options;
using Hsm.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hsm.Persistence.Extensions
{
    public static class PersistenceLayerExtension
    {
        public static IServiceCollection LoadPersistenceLayerExtension(this IServiceCollection services)
        {
            SqlServerOptions sqlOptions = services.GetOptions<SqlServerOptions>("SqlServerOptions");

            services.AddDbContext<HsmDbContext>(opt => opt.UseSqlServer(sqlOptions.SqlConnection));

            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequiredLength = 10;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequiredUniqueChars = 2;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(60);
                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.User.RequireUniqueEmail = true;

            })
                .AddRoleManager<RoleManager<AppRole>>()
                .AddEntityFrameworkStores<HsmDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
