using System;
using System.Collections.Generic;
using System.Text.Json;

namespace SangtuariCareerCompass.Models.DTOs
{
    public class SubTestSubmissionDto
    {
        public Guid UserAssessmentId { get; set; }
        public string SubTestName { get; set; } = string.Empty;
        public JsonElement Answers { get; set; } = new();
    }
}