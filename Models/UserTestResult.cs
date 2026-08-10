using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace SangtuariCareerCompass.Models
{
    [Table("UserTestResults")]
    public class UserTestResult
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserAssessmentId { get; set; }
        [ForeignKey("UserAssessmentId")]
        public UserAssessment UserAssessment { get; set; } = null!;

        [Required]
        public string TestCategory { get; set; } = string.Empty; // "IST", "CFIT", "EAS", "PAPI", "VARK", "SDS_Holland"

        public int OverallScore { get; set; } // IQ untuk IST/CFIT, atau skor dominan
        public string Classification { get; set; } = string.Empty; // "Cerdas", "Sangat Cerdas", dll.

        [Required, Column(TypeName = "jsonb")]
        public JsonDocument ResultDetails { get; set; } = null!; // Menampung detail breakdown per aspek/subtes

        public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;
    }
}