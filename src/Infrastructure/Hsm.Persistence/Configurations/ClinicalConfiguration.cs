using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hsm.Persistence.Configurations
{
    public class ClinicalConfiguration : IEntityTypeConfiguration<Clinical>
    {
        public void Configure(EntityTypeBuilder<Clinical> builder)
        {
            builder.ToTable(Constant.Table.Clinical);

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name).IsRequired();

            builder.Property(c => c.CreatedDateUTC).IsRequired();
            builder.Property(c => c.UpdatedDateUTC).IsRequired(false);
            builder.Property(c => c.RowVersion).IsRowVersion();
            builder.Property(c => c.IsActive).IsRequired();

            builder.HasOne(a => a.Hospital)
                  .WithMany(u => u.Clinicals)
                  .HasForeignKey(u => u.HospitalId);
        }
    }
}
