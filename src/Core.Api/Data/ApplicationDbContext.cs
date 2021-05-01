using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Core.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Name = "admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "user",
                    NormalizedName = "USER"
                },
                new IdentityRole
                {
                    Name = "geral",
                    NormalizedName = "GERAL"
                },
                new IdentityRole
                {
                    Name = "fornecedor",
                    NormalizedName = "FORNECEDOR"
                },
                new IdentityRole
                {
                    Name = "medico",
                    NormalizedName = "MEDICO"
                },
                new IdentityRole
                {
                    Name = "paciente",
                    NormalizedName = "PACIENTE"
                }
            );

            base.OnModelCreating(builder);
        }
    }
}