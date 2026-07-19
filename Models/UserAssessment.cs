using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace SangtuariCareerCompass.Models
{
    public class UserAssessment
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Page 1: Tipe Asesmen
        [Required]
        public string AssessmentType { get; set; } // Exploration, Discovery, Advanced

        // Page 2: Data Diri Dasar
        [Required]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string SchoolName { get; set; }
        [Required]
        public string ClassName { get; set; }
        [Required]
        public string Major { get; set; }

        // Page 3: Data Tambahan Dinamis (Disimpan sebagai JSONB di Postgres)
        // Format: { "FatherJob": "...", "MotherJob": "...", "Hobby": "...", "Goals": "...", "LikedSubjects": ["A","B","C"], "DislikedSubjects": ["X","Y","Z"] }
        public JsonDocument AdditionalData { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}