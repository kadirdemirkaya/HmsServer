using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hsm.Persistence.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable(Constant.Table.Appointments);

            builder.HasKey(a => a.Id);

            builder.Property(a => a.AppointmentTime).IsRequired();

            builder.Property(c => c.CreatedDateUTC).IsRequired();
            builder.Property(c => c.UpdatedDateUTC).IsRequired(false);
            builder.Property(c => c.RowVersion).IsRowVersion();
            builder.Property(c => c.IsActive).IsRequired();

            builder.HasOne(a => a.User)
                   .WithMany(u => u.Appointments)
                   .HasForeignKey(u => u.UserId);

            builder.HasOne(a => a.WorkSchedule)
                   .WithMany(u => u.Appointments)
                   .HasForeignKey(u => u.WorkScheduleId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
