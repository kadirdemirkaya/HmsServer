using Hsm.Domain.Entities.Base;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;

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
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Entity<Doctor>()
               .HasOne(d => d.Hospital)
               .WithMany(h => h.Doctors)
               .HasForeignKey(d => d.HospitalId);

            builder.Entity<Doctor>()
                .HasOne(d => d.AppUser)
                .WithMany(u => u.Doctors)
                .HasForeignKey(d => d.AppUserId);

            builder.Entity<City>()
                .HasMany(c => c.Hospitals)
                .WithOne(h => h.City)
                .HasForeignKey(c => c.CityId);

            builder.Entity<Hospital>()
                .OwnsOne(h => h.Address, address =>
                {
                    address.Property(a => a.Street).HasMaxLength(100);
                    address.Property(a => a.City).HasMaxLength(50);
                    address.Property(a => a.State).HasMaxLength(50);
                    address.Property(a => a.PostalCode).HasMaxLength(20);
                });

            builder.Entity<Hospital>()
                .HasOne(h => h.City)
                .WithMany(c => c.Hospitals)
                .HasForeignKey(h => h.CityId);

            builder.Entity<WorkSchedule>()
                .HasOne(ws => ws.Doctor)
                .WithMany(d => d.WorkSchedules)
                .HasForeignKey(ws => ws.DoctorId);

            builder.Entity<Appointment>()
                .HasOne(a => a.User)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.UserId);

            builder.Entity<Appointment>()
                .HasOne(a => a.WorkSchedule)
                .WithMany(w => w.Appointments)
                .HasForeignKey(a => a.WorkScheduleId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId);

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
