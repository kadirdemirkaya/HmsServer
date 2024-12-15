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

            builder.Property(a => a.CreatedDateUTC).IsRequired();
            builder.Property(a => a.UpdatedDateUTC).IsRequired();
            builder.Property(x => x.RowVersion).IsRowVersion();
            builder.Property(x => x.IsActive).IsRowVersion();

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
