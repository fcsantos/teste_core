using Core.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Core.Data.Mappings
{
    public class NoticeMapping : IEntityTypeConfiguration<Notice>
    {
        public void Configure(EntityTypeBuilder<Notice> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Description)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.StartDate)
                .IsRequired()
                .HasColumnType("DateTime");

            builder.Property(p => p.EndDate)
               .IsRequired()
               .HasColumnType("DateTime");

            builder.Property(n => n.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasColumnType("varchar(100)");

            builder.Property(n => n.SendToAllUsers)
                .IsRequired()
                .HasColumnType("bit");

            builder.Property(p => p.IsActive)
               .IsRequired()
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

            builder.HasMany(x => x.NoticeUsers)
                    .WithOne(x => x.Notice);
                

            builder.ToTable("Notices");
        }
    }
}