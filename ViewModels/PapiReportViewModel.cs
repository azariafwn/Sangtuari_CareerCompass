using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SangtuariCareerCompass.Data;
using System.Threading.Tasks;

namespace SangtuariCareerCompass.ViewModels
{
    public class PapiFactorScore
    {
        public string Code { get; set; } = string.Empty;
        public int RawScore { get; set; }
        public string Leveling { get; set; } = string.Empty; // Kurang, Rata-rata, Baik, dll
    }

    public class PapiAspectGroup
    {
        public string AspectName { get; set; } = string.Empty;
        public List<PapiFactorScore> Factors { get; set; } = new();
        public string FinalJudgment { get; set; } = string.Empty; // Hasil manual psikolog
    }

    // ViewModel Induk
    public class PapiReportViewModel
    {
        public Guid UserAssessmentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public Dictionary<string, string> RawAnswers { get; set; } = new();

        // Output Engine
        public List<PapiAspectGroup> AspectGroups { get; set; } = new();
        public bool IsJudged { get; set; }

        public static async Task<PapiReportViewModel?> BuildFromDatabaseAsync(ApplicationDbContext dbContext, Guid assessmentId)
        {
            var user = await dbContext.UserAssessments.AsNoTracking().FirstOrDefaultAsync(u => u.Id == assessmentId);
            if (user == null) return null;

            var answerRecord = await dbContext.UserAnswers.AsNoTracking()
                .FirstOrDefaultAsync(a => a.UserAssessmentId == assessmentId && a.SubTestName.StartsWith("PAPI"));

            var rawAnswers = new Dictionary<string, string>();
            if (answerRecord?.Answers != null)
            {
                try
                {
                    using var doc = JsonDocument.Parse(answerRecord.Answers.RootElement.GetRawText());
                    foreach (var prop in doc.RootElement.EnumerateObject())
                    {
                        rawAnswers[prop.Name] = prop.Value.GetString()?.ToUpper() ?? "";
                    }
                }
                catch { }
            }

            // Cek apakah psikolog sudah memberikan judgment
            var testResult = await dbContext.UserTestResults.AsNoTracking()
                .FirstOrDefaultAsync(r => r.UserAssessmentId == assessmentId && r.TestCategory == "PAPI_Kostick");

            var vm = new PapiReportViewModel
            {
                UserAssessmentId = user.Id,
                FullName = user.FullName,
                RawAnswers = rawAnswers,
                IsJudged = testResult != null
            };

            // Jika sudah ada result, inject judgment-nya
            if (testResult != null && testResult.ResultDetails.RootElement.ValueKind == JsonValueKind.Array)
            {
                vm.AspectGroups = JsonSerializer.Deserialize<List<PapiAspectGroup>>(testResult.ResultDetails.RootElement.GetRawText()) ?? new();
            }

            return vm;
        }
    }

    // DTO untuk POST form Judgment Psikolog
    public class SubmitPapiJudgmentDto
    {
        [Required]
        public Guid UserAssessmentId { get; set; }
        [Required]
        public string JudgmentsJson { get; set; } = string.Empty; // Serialized AspectGroups
    }
}