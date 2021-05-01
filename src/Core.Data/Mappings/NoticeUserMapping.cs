using Core.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Core.Data.Mappings
{
    public class NoticeUserMapping : IEntityTypeConfiguration<NoticeUser>
    {
        public void Configure(EntityTypeBuilder<NoticeUser> builder)
        {
            builder.HasKey(n => new { n.NoticeId, n.PatientId });

            builder.HasOne(n => n.Patient)
                    .WithMany(p => p.NoticeUsers)
                    .HasForeignKey(n => n.PatientId);

            builder.HasOne(n => n.Notice)
                   .WithMany(p => p.NoticeUsers)
                   .HasForeignKey(n => n.NoticeId);

            builder.Property(p => p.IsRead)
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

            builder.ToTable("NoticeUsers");
        }
    }
}