using Hsm.Domain.Entities.Base;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Hsm.Persistence.Context
{
    public class HsmDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public HsmDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<ElectronicHealthRecord> ElectronicHealthRecords { get; set; }
        public DbSet<Staff> StaffMembers { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<BedManagement> Beds { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Patient>()
                .HasOne(p => p.AppUser) 
                .WithMany() 
                .HasForeignKey(p => p.AppUserId) 
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
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
                            entity.IsActive = true;
                            break;
                        case EntityState.Deleted:
                            entity.SetUpdatedDateUTC(DateTime.UtcNow);
                            entity.IsActive = false;
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
