using Core.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Mappings
{
    public class PathologyMapping : IEntityTypeConfiguration<Pathology>
    {
        public void Configure(EntityTypeBuilder<Pathology> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.CreatedBy)
                .IsRequired(false)
                .HasColumnType("varchar(max)");
            builder.Property(p => p.CreatedDate)
                .IsRequired(false)
                .HasColumnType("DateTime")
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.UpdatedBy)
                .IsRequired(false)
                .HasColumnType("varchar(max)");
            builder.Property(p => p.UpdatedDate)
                .IsRequired(false)
                .HasColumnType("DateTime");

            // 1 : N => Pathology : SubPathologies
            builder.HasMany(p => p.SubPathologies)
                .WithOne(s => s.ParentPathology)
                .HasForeignKey(s => s.ParentPathologyId)
                .IsRequired(false);

            // 1 : N => Pathology : ClinicalSummaryFacilitators
            builder.HasMany(p => p.ClinicalSummaryFacilitators)
                .WithOne(c => c.Pathology)
                .HasForeignKey(c => c.PathologyId);

            builder.ToTable("Pathologies");
        }
    }
}
