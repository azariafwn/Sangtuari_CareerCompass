using System;

namespace SangtuariCareerCompass.Models.DTOs
{
    public class SubmitSmpJudgmentDto
    {
        public Guid UserAssessmentId { get; set; }
        public string Kesimpulan { get; set; } = string.Empty;
        public string Saran { get; set; } = string.Empty;
    }
}