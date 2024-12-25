using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hsm.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable(Constant.Table.Notifications);

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Message).IsRequired();
            builder.Property(c => c.NotificationType).IsRequired();

            builder.Property(c => c.CreatedDateUTC).IsRequired();
            builder.Property(c => c.UpdatedDateUTC).IsRequired(false);
            builder.Property(c => c.RowVersion)
                .IsConcurrencyToken()
                .HasColumnType("uuid")
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("gen_random_uuid()");
            builder.Property(c => c.IsActive).IsRequired();

            builder.HasOne(c => c.User)
                   .WithMany(h => h.Notifications)
                   .HasForeignKey(c => c.UserId);

        }
    }
}
