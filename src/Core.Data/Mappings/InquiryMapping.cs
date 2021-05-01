using Core.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Mappings
{
    public class InquiryMapping : IEntityTypeConfiguration<Inquiry>
    { 
        public void Configure(EntityTypeBuilder<Inquiry> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.Property(p => p.Description)
                .IsRequired(false)
                .HasColumnType("varchar(250)");

            builder.Property(p => p.IsActive)
                .IsRequired(false)
                .HasDefaultValueSql("1")
                .HasColumnType("bit");

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

            builder.ToTable("Inquiries");
        }
    }
}
