using Microsoft.EntityFrameworkCore;
using SangtuariCareerCompass.Models;

namespace SangtuariCareerCompass.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<PsychologistUser> PsychologistUsers { get; set; } = default!;
        public DbSet<UserAssessment> UserAssessments { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<UserTestResult> UserTestResults { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Pemetaan khusus kolom JSONB PostgreSQL
            modelBuilder.Entity<UserAssessment>()
                .Property(u => u.AdditionalData)
                .HasColumnType("jsonb");

            modelBuilder.Entity<UserAnswer>()
                .Property(a => a.Answers)
                .HasColumnType("jsonb");

            modelBuilder.Entity<PsychologistUser>().HasData(new PsychologistUser
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                FullName = "Kepala Psikolog Sangtuari",
                Email = "admin@sangtuari.com",
                // Hash BCrypt untuk password "admin123"
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = "Head"
            });
        }
    }
}