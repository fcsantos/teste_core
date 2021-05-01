using Core.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Mappings
{
    public class ClinicalSummaryFacilitatorMapping : IEntityTypeConfiguration<ClinicalSummaryFacilitator>
    {
        public void Configure(EntityTypeBuilder<ClinicalSummaryFacilitator> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Description)
                .IsRequired()
                .HasColumnType("varchar(5000)");

            builder.Property(m => m.TypeClinicalSummary)
                .HasColumnType("varchar(20)")
                .HasConversion<string>();

            builder.Property(c => c.CreatedBy)
                .IsRequired(false)
                .HasColumnType("varchar(max)");
            builder.Property(c => c.CreatedDate)
                .IsRequired(false)
                .HasColumnType("DateTime")
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.UpdatedBy)
                .IsRequired(false)
                .HasColumnType("varchar(max)");
            builder.Property(c => c.UpdatedDate)
                .IsRequired(false)
                .HasColumnType("DateTime");

            builder.ToTable("ClinicalSummaryFacilitators");
        }
    }
}
