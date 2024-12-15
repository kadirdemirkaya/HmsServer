using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hsm.Persistence.Configurations
{
    public class CitiesConfiguraion : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable(Constant.Table.Cities);

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name).IsRequired();

            builder.Property(c => c.CreatedDateUTC).IsRequired();
            builder.Property(c => c.UpdatedDateUTC).IsRequired();
            builder.Property(c => c.RowVersion).IsRowVersion();
            builder.Property(c => c.IsActive).IsRowVersion();

            builder.HasMany(c => c.Hospitals)
                   .WithOne(h => h.City)
                   .HasForeignKey(c => c.CityId);

        }
    }
}
