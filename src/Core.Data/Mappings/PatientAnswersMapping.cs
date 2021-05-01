using Core.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Mappings
{
    public class PatientAnswersMapping : IEntityTypeConfiguration<PatientAnswers>
    {
        public void Configure(EntityTypeBuilder<PatientAnswers> builder)
        {
            builder.HasKey(iq => iq.Id);

            builder.Property(e => e.AnswerValue)
                .HasColumnType("decimal(5, 2)")
                .HasDefaultValueSql("((0))");

            builder.Property(iq => iq.CreatedBy)
                .IsRequired(false)
                .HasColumnType("varchar(max)");

            builder.Property(iq => iq.CreatedDate)
                .IsRequired(false)
                .HasColumnType("DateTime")
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(iq => iq.UpdatedBy)
                .IsRequired(false)
                .HasColumnType("varchar(max)");

            builder.Property(iq => iq.UpdatedDate)
                .IsRequired(false)
                .HasColumnType("DateTime");

            builder.ToTable("PatientAnswers");
        }
    }
}