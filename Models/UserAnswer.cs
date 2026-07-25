using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace SangtuariCareerCompass.Models
{
    public class UserAnswer
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserAssessmentId { get; set; }
        [ForeignKey("UserAssessmentId")]
        public UserAssessment UserAssessment { get; set; }

        [Required]
        public string SubTestName { get; set; } // Contoh: "CFIT", "EAS-1", "VARK", "SDS-Holland", "PAPI-Kostick"

        // Menyimpan daftar jawaban user: { "Q1": "A", "Q2": "C", "Q3": [1, 4] }
        [Required]
        public JsonDocument Answers { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}