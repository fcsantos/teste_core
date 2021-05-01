using Core.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Mappings
{
    public class DoctorSpecialtyMapping : IEntityTypeConfiguration<DoctorSpecialty>
    {
        public void Configure(EntityTypeBuilder<DoctorSpecialty> builder) {

            builder.HasKey(x => new { x.DoctorId, x.SpecialtyId });

            builder
                .HasOne(x => x.Doctor)
                .WithMany(y => y.DoctorSpecialties)
                .HasForeignKey(y => y.DoctorId);

            builder
                .HasOne(x => x.Specialty)
                .WithMany(y => y.DoctorSpecialties)
                .HasForeignKey(y => y.SpecialtyId);
        }
    }
}
