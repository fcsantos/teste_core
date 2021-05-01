using Core.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Mappings
{
    public class ServiceDoctorMapping : IEntityTypeConfiguration<ServiceDoctor>
    {
        public void Configure(EntityTypeBuilder<ServiceDoctor> builder) {

            builder.HasKey(sd => new { sd.DoctorId, sd.ServiceId });

            builder
                .HasOne(sd => sd.Doctor)
                .WithMany(d => d.ServiceDoctors)
                .HasForeignKey(y => y.DoctorId);

            builder
                .HasOne(sd => sd.Service)
                .WithMany(s => s.ServiceDoctors)
                .HasForeignKey(y => y.ServiceId);

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

            builder.ToTable("ServiceDoctors");
        }
    }
}
