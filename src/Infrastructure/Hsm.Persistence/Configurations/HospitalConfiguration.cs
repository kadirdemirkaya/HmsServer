using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hsm.Persistence.Configurations
{
    public class HospitalConfiguration : IEntityTypeConfiguration<Hospital>
    {
        public void Configure(EntityTypeBuilder<Hospital> builder)
        {
            builder.ToTable(Constant.Table.Hospitals);

            builder.HasKey(c => c.Id);

            builder.Property(c => c.ContactNumber).IsRequired();
            builder.Property(c => c.Name).IsRequired();

            builder.Property(c => c.CreatedDateUTC).IsRequired();
            builder.Property(c => c.UpdatedDateUTC).IsRequired(false);
            builder.Property(c => c.RowVersion)
                .IsConcurrencyToken()
                .HasColumnType("uuid")
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("gen_random_uuid()");
            builder.Property(c => c.IsActive).IsRequired();

            builder.HasOne(c => c.City)
                   .WithMany(h => h.Hospitals)
                   .HasForeignKey(c => c.CityId);

            builder.HasMany(d => d.Doctors)
                   .WithOne(w => w.Hospital)
                   .HasForeignKey(w => w.HospitalId);

            builder.HasMany(c => c.Clinicals)
                   .WithOne(h => h.Hospital)
                   .HasForeignKey(c => c.HospitalId);
        }
    }
}
