using Microsoft.EntityFrameworkCore;
using SangtuariCareerCompass.Models;

namespace SangtuariCareerCompass.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<UserAssessment> UserAssessments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Memetakan properti JsonDocument menjadi kolom JSONB di PostgreSQL
            modelBuilder.Entity<UserAssessment>()
                .Property(b => b.AdditionalData)
                .HasColumnType("jsonb");
        }
    }
}