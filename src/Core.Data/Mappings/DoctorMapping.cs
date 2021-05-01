using Core.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Mappings
{
    public class DoctorMapping : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(d => d.BirthDate)
                .IsRequired()
                .HasColumnType("DateTime");

            builder.Property(d => d.Email)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(d => d.Cell)
               .IsRequired()
               .HasColumnType("varchar(20)");

            builder.Property(d => d.DocumentCard)
               .IsRequired()
               .HasColumnType("varchar(20)");

            builder.Property(d => d.IsActive)
                .IsRequired(false)
                .HasDefaultValueSql("1")
                .HasColumnType("bit");

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

            builder.HasMany(d => d.DoctorSpecialties)
                .WithOne(de => de.Doctor);

            builder.HasMany(s => s.Services)
                .WithOne(d => d.Doctor);

            // 1 : N => Doctor : Messages
            builder.HasMany(d => d.Messages)
                .WithOne(m => m.Doctor)
                .HasForeignKey(m => m.DoctorId);

            // 1 : N => Doctor : Consultations
            builder.HasMany(d => d.Consultations)
                .WithOne(c => c.Doctor)
                .HasForeignKey(c => c.DoctorId);

            // 1 : N => Doctor : Allergies
            builder.HasMany(d => d.Allergies)
                .WithOne(c => c.Doctor)
                .HasForeignKey(c => c.DoctorId);

            // 1 : N => Doctor : Diagnostics
            builder.HasMany(d => d.Diagnostics)
                .WithOne(c => c.Doctor)
                .HasForeignKey(c => c.DoctorId);

            // 1 : N => Doctor : CarePlans
            builder.HasMany(d => d.CarePlans)
                .WithOne(c => c.Doctor)
                .HasForeignKey(c => c.DoctorId);

            // 1 : N => Doctor : ClinicalSummaryFacilitators
            builder.HasMany(d => d.ClinicalSummaryFacilitators)
                .WithOne(c => c.Doctor)
                .HasForeignKey(c => c.DoctorId);

            builder.ToTable("Doctors");
        }
    }
}
