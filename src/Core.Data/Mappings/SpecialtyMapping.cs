using Core.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Mappings
{
    public class SpecialtyMapping : IEntityTypeConfiguration<Specialty>
    {
        public void Configure(EntityTypeBuilder<Specialty> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.CreatedBy)
                .IsRequired(false)
                .HasColumnType("varchar(max)");
            builder.Property(s => s.CreatedDate)
                .IsRequired(false)
                .HasColumnType("DateTime")
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(s => s.UpdatedBy)
                .IsRequired(false)
                .HasColumnType("varchar(max)");
            builder.Property(s => s.UpdatedDate)
                .IsRequired(false)
                .HasColumnType("DateTime");

            // 1 : N => Specialty : SubSpecialties
            builder.HasMany(s => s.SubSpecialties)
                .WithOne(s => s.ParentSpecialty)
                .HasForeignKey(s => s.ParentSpecialtyId)
                .IsRequired(false);

            builder.ToTable("Specialties");
        }
    }
}
