using System;

namespace SangtuariCareerCompass.Models.DTOs
{
    public class SubmitSmaJudgmentDto
    {
        public Guid UserAssessmentId { get; set; }
        public string Kekuatan { get; set; } = string.Empty;
        public string AreaPengembangan { get; set; } = string.Empty;
        public string DinamikaKognitif { get; set; } = string.Empty;
        public string DinamikaSikapKerja { get; set; } = string.Empty;
        public string DinamikaGayaBelajar { get; set; } = string.Empty;
        public string DinamikaMinatKarier { get; set; } = string.Empty;
        public string DinamikaRekomendasi { get; set; } = string.Empty;
        public string SaranPengembangan { get; set; } = string.Empty;
    }
}