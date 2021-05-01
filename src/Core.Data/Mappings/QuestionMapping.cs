using Core.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Mappings
{
    public class QuestionMapping : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(e => e.Title)
                .IsRequired(true)
                .HasMaxLength(250);

            builder.Property(e => e.Placeholder)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(m => m.SingleLine)
                .IsRequired(false)
                .HasDefaultValueSql("1")
                .HasColumnType("bit");

            builder.Property(e => e.SortOrder)
                .HasDefaultValueSql("((99))");

            builder.Property(m => m.TypeOfAnswer)
                .HasColumnType("varchar(20)")
                .HasConversion<string>();

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

            // 1 : N => Inquiry : Question
            builder.HasOne(d => d.Inquiry)
                .WithMany(p => p.Questions)
                .HasForeignKey(d => d.InquiryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Questions");
        }
    }
}
