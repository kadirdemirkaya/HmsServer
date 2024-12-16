using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hsm.Persistence.Configurations
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable(Constant.Table.Doctors);

            builder.HasKey(c => c.Id);

            builder.Property(c => c.FirstName).IsRequired();
            builder.Property(c => c.LastName).IsRequired();
            builder.Property(c => c.Schedule).IsRequired();
            builder.Property(c => c.Specialty).IsRequired();

            builder.Property(c => c.CreatedDateUTC).IsRequired();
            builder.Property(c => c.UpdatedDateUTC).IsRequired(false);
            builder.Property(c => c.RowVersion).IsRowVersion();
            builder.Property(c => c.IsActive).IsRequired();

            builder.HasOne(d => d.AppUser)
                   .WithMany(u => u.Doctors)
                   .HasForeignKey(d => d.AppUserId);

            builder.HasOne(d => d.Hospital)
                 .WithMany(u => u.Doctors)
                 .HasForeignKey(d => d.HospitalId);

            builder.HasMany(d => d.WorkSchedules)
                   .WithOne(w => w.Doctor)
                   .HasForeignKey(w => w.DoctorId);
        }
    }
}
