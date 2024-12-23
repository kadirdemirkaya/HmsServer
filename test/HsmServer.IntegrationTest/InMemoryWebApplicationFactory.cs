using Hsm.Domain.Entities.Identity;
using Hsm.Persistence.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HsmServer.IntegrationTest
{
    public class InMemoryWebApplicationFactory<T> : WebApplicationFactory<T> where T : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing")
                   .ConfigureTestServices(services =>
                   {
                       var options = new DbContextOptionsBuilder<HsmDbContext>()
                                                              .UseInMemoryDatabase("testMemory").Options;

                       services.AddScoped<HsmDbContext>(provider => new HospitalTestDbContext(options));

                       var serviceProvider = services.BuildServiceProvider();
                       using var scope = serviceProvider.CreateScope();
                       var scopedService = scope.ServiceProvider;
                       var db = scopedService.GetRequiredService<HsmDbContext>();
                       db.Database.EnsureCreated();

                       #region
                       //var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAuthenticationSchemeProvider));
                       //if (descriptor != null)
                       //{
                       //    services.Remove(descriptor); 
                       //}

                       //services.AddIdentity<AppUser, AppRole>()
                       //     .AddEntityFrameworkStores<HospitalTestDbContext>()
                       //     .AddDefaultTokenProviders();
                       #endregion


                       //services.AddAuthentication("TestScheme").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", opt => { });

                   });
        }
    }
}
