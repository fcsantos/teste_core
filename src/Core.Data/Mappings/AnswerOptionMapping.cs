using Core.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Mappings
{
    public class AnswerOptionMapping : IEntityTypeConfiguration<AnswerOption>
    {
        public void Configure(EntityTypeBuilder<AnswerOption> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Option)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(e => e.SortOrder)
                .HasDefaultValueSql("((99))");

            builder.Property(e => e.AnswerValue)
                .HasColumnType("decimal(5, 2)")
                .HasDefaultValueSql("((0))");

            builder.Property(d => d.CreatedBy)
                .IsRequired(false)
                .HasColumnType("varchar(max)");

            builder.Property(d => d.CreatedDate)
                .IsRequired(false)
                .HasColumnType("DateTime")
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(d => d.UpdatedBy)
                .IsRequired(false)
                .HasColumnType("varchar(max)");

            builder.Property(d => d.UpdatedDate)
                .IsRequired(false)
                .HasColumnType("DateTime");

            // 1 : N => Question : AnswerOptions
            builder.HasOne(d => d.Question)
                .WithMany(p => p.AnswerOptions)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("AnswerOptions");
        }
    }
}




