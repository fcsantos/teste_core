using Core.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Mappings
{
    public class EmergencyChannelMapping : IEntityTypeConfiguration<EmergencyChannel>
    {
        public void Configure(EntityTypeBuilder<EmergencyChannel> builder)
        {
            builder.HasKey(h => h.Id);

            builder.Property(h => h.Name)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(h => h.Description)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(h => h.Cell)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(h => h.sortOrder)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(h => h.IsActive)
                .IsRequired()
                .HasDefaultValueSql("1")
                .HasColumnType("bit");

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

            builder.ToTable("EmergencyChannel");
        }
    }
}
