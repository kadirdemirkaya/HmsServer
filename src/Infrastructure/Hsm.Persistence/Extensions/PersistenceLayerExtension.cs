using EfCore.Repository.Extensions;
using Hsm.Application.Extensions;
using Hsm.Domain.Entities.Base;
using Hsm.Domain.Entities.Identity;
using Hsm.Domain.Models.Options;
using Hsm.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Hsm.Persistence.Extensions
{
    public static class PersistenceLayerExtension
    {
        public static IServiceCollection LoadPersistenceLayerExtension(this IServiceCollection services, Assembly assembly)
        {
            SqlServerOptions sqlOptions = services.GetOptions<SqlServerOptions>("SqlServerOptions");

            services.AddDbContext<HsmDbContext>(opt => opt.UseSqlServer(sqlOptions.SqlConnection));
            
            services.EfCoreRepositoryServiceRegistration<IBaseEntity, HsmDbContext>(ServiceLifetime.Scoped, assembly);

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
