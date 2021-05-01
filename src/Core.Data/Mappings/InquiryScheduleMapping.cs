using Core.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Mappings
{
    public class InquiryScheduleMapping : IEntityTypeConfiguration<InquirySchedule>
    {
        public void Configure(EntityTypeBuilder<InquirySchedule> builder)
        {
            builder.HasKey(iq => iq.Id);

            builder.Property(iq => iq.Answered)
                .IsRequired(false)
                .HasDefaultValueSql("0")
                .HasColumnType("bit");

            builder.Property(p => p.IsMailSender)
                .IsRequired(true)
                .HasDefaultValueSql("0")
                .HasColumnType("bit");

            builder.Property(iq => iq.StartDate)
                .IsRequired()
                .HasColumnType("DateTime");
            
            builder.Property(iq => iq.EndDate)
                .IsRequired()
                .HasColumnType("DateTime");

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

            builder.ToTable("InquiriesSchedule");
        }
    }
}