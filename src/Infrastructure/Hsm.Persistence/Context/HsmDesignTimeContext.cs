using Hsm.Application.Extensions;
using Hsm.Domain.Models.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Hsm.Persistence.Context
{
    public class HsmDesignTimeContext : IDesignTimeDbContextFactory<HsmDbContext>
    {
        public HsmDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath($"{Directory.GetCurrentDirectory()}{"/../../Presentation/Hsm.Api"}")
               .AddJsonFile("appsettings.json")
               .Build();

            DbContextOptionsBuilder<HsmDbContext> dbContextOptionsBuilder = new();

            SqlServerOptions sqlOptions = configuration.GetOptions<SqlServerOptions>("SqlServerOptions");
            dbContextOptionsBuilder.UseNpgsql(sqlOptions.SqlConnection);

            return new(dbContextOptionsBuilder.Options);
        }
    }
}
