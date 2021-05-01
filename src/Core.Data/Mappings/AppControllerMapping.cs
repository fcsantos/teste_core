using Core.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Mappings
{
    public class AppControllerMapping : IEntityTypeConfiguration<AppController>
    {
        public void Configure(EntityTypeBuilder<AppController> builder)
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

            // 1 : N => Controller : Actions
            builder.HasMany(c => c.Actions)
                .WithOne(a => a.Controller)
                .HasForeignKey(a => a.ControllerId);

            builder.ToTable("Controllers");
        }
    }
}