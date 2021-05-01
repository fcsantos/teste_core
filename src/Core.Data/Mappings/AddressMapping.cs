using Core.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Mappings
{
    public class AddressMapping : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        { 
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Location)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(a => a.Number)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(a => a.PostalCode)
                .IsRequired()
                .HasColumnType("char(8)");

            builder.Property(a => a.Complement)
                .HasColumnType("varchar(250)");

            builder.Property(a => a.District)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(a => a.City)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(a => a.State)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(a => a.CreatedBy)
                .IsRequired(false)
                .HasColumnType("varchar(max)");
            builder.Property(a => a.CreatedDate)
                .IsRequired(false)
                .HasColumnType("DateTime")
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(a => a.UpdatedBy)
                .IsRequired(false)
                .HasColumnType("varchar(max)");
            builder.Property(a => a.UpdatedDate)
                .IsRequired(false)
                .HasColumnType("DateTime");

            // 1 : 1 => Endereco : Fornecedor 
            builder.HasOne(a => a.Fornecedor)
                .WithOne(f => f.Address);

            // 1 : 1 => Endereco : Patient 
            builder.HasOne(a => a.Patient)
                .WithOne(p => p.Address);

            builder.ToTable("Adresses");
        }
    }
}