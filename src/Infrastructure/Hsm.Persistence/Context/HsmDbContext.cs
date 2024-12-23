using Hsm.Domain.Entities.Base;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hsm.Persistence.Context
{
    public class HsmDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public HsmDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<WorkSchedule> WorkSchedules { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);

            base.OnModelCreating(builder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entity)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            entity.SetCreatedDateUTC(DateTime.UtcNow);
                            entity.SetIsActive(true);
                            break;
                        case EntityState.Deleted:
                            entity.SetUpdatedDateUTC(DateTime.UtcNow);
                            entity.SetIsActive(true);
                            break;
                        case EntityState.Modified:
                            entity.SetUpdatedDateUTC(DateTime.UtcNow);
                            break;
                        default:
                            break;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
