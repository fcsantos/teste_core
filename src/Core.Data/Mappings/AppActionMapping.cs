using Core.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Mappings
{
    public class AppActionMapping : IEntityTypeConfiguration<AppAction>
    {
        public void Configure(EntityTypeBuilder<AppAction> builder)
        {
            builder.HasKey(p => p.Id);

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


            builder.ToTable("Actions");
        }
    }
}