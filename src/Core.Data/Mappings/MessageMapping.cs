using Core.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Mappings
{
    public class MessageMapping : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Text)
                .IsRequired()
                .HasColumnType("varchar(5000)");

            builder.Property(m => m.IsActive)
                .IsRequired(false)
                .HasDefaultValueSql("1")
                .HasColumnType("bit");

            builder.Property(m => m.IsReply)
                .IsRequired()
                .HasColumnType("bit");

            builder.Property(p => p.IsMailSender)
                .IsRequired(true)
                .HasDefaultValueSql("0")
                .HasColumnType("bit");

            builder.Property(m => m.StatusMessage)
                .HasColumnType("varchar(20)")
                .HasConversion<string>();

            builder.Property(m => m.CreatedBy)
                .IsRequired(false)
                .HasColumnType("varchar(max)");
            builder.Property(m => m.CreatedDate)
                .IsRequired(false)
                .HasColumnType("DateTime")
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(m => m.UpdatedBy)
                .IsRequired(false)
                .HasColumnType("varchar(max)");
            builder.Property(m => m.UpdatedDate)
                .IsRequired(false)
                .HasColumnType("DateTime");

            builder.ToTable("Messages");
        }
    }
}
