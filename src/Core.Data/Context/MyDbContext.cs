using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.Context
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Address> Adresses { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Notice> Notices { get; set; }
        public DbSet<NoticeUser> NoticeUsers { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<AppController> Controllers { get; set; }
        public DbSet<AppAction> Actions { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Pathology> Pathologies { get; set; }
        public DbSet<EmergencyChannel> EmergencyChannel { get; set; }
        public DbSet<ClinicalSummaryFacilitator> ClinicalSummaryFacilitators { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceDoctor> ServiceDoctors { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<DoctorSpecialty> DoctorSpecialties { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<Allergy> Allergies { get; set; }
        public DbSet<Diagnostic> Diagnostics { get; set; }
        public DbSet<CarePlan> CarePlans { get; set; }
        public DbSet<Observation> Observations { get; set; }
        public DbSet<Inquiry> Inquiries { get; set; }
        public DbSet<AnswerOption> AnswerOptions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<InquirySchedule> InquiriesSchedule { get; set; }
        public DbSet<PatientAnswers> PatientAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties()
                    .Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyDbContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("CreatedDate") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedDate").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("CreatedDate").IsModified = false;
                    entry.Property("CreatedBy").IsModified = false;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}