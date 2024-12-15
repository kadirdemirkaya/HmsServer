using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hsm.Persistence.Configurations
{
    public class WorkScheduleConfiguration : IEntityTypeConfiguration<WorkSchedule>
    {
        public void Configure(EntityTypeBuilder<WorkSchedule> builder)
        {
            builder.ToTable(Constant.Table.WorkSchedules);

            builder.HasKey(c => c.Id);

            builder.Property(c => c.StartDate).IsRequired();
            builder.Property(c => c.EndDate).IsRequired();
            builder.Property(c => c.Name).IsRequired();

            builder.Property(c => c.CreatedDateUTC).IsRequired();
            builder.Property(c => c.UpdatedDateUTC).IsRequired();
            builder.Property(c => c.RowVersion).IsRowVersion();
            builder.Property(c => c.IsActive).IsRowVersion();

            builder.HasOne(c => c.Doctor)
                   .WithMany(h => h.WorkSchedules)
                   .HasForeignKey(c => c.DoctorId);

            builder.HasMany(d => d.Appointments)
                   .WithOne(w => w.WorkSchedule)
                   .HasForeignKey(w => w.WorkScheduleId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
