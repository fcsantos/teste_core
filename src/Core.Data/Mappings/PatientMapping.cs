using Core.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Mappings
{
    public class PatientMapping : IEntityTypeConfiguration<Patient>
    { 
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(p => p.Document)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(p => p.DocumentCard)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(p => p.Email)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(p => p.Cell)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(p => p.BirthDate)
                .IsRequired()
                .HasColumnType("DateTime");

            builder.Property(p => p.IsActive)
                .IsRequired(false)
                .HasDefaultValueSql("1")
                .HasColumnType("bit");

            builder.Property(p => p.IsMailSender)
                .IsRequired(true)
                .HasDefaultValueSql("0")
                .HasColumnType("bit");

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

            // 1 : N => Patient : Messages
            builder.HasMany(p => p.Messages)
                .WithOne(m => m.Patient)
                .HasForeignKey(m => m.PatientId);

            // 1 : N => Patient : Consultations
            builder.HasMany(p => p.Consultations)
                .WithOne(c => c.Patient)
                .HasForeignKey(c => c.PatientId);

            // 1 : N => Patient : Allergies
            builder.HasMany(p => p.Allergies)
                .WithOne(c => c.Patient)
                .HasForeignKey(c => c.PatientId);

            // 1 : N => Patient : Diagnostics
            builder.HasMany(p => p.Diagnostics)
                .WithOne(c => c.Patient)
                .HasForeignKey(c => c.PatientId);

            // 1 : N => Patient : CarePlans
            builder.HasMany(p => p.CarePlans)
                .WithOne(c => c.Patient)
                .HasForeignKey(c => c.PatientId);



            builder.ToTable("Patients");
        }
    }
}
