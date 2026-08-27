using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SangtuariCareerCompass.Data;
using System.Threading.Tasks;

namespace SangtuariCareerCompass.ViewModels
{
    public class IstSubTestReportItem
    {
        public string SubTestCode { get; set; } = string.Empty;
        public string SubTestName { get; set; } = string.Empty;
        public string AspectName { get; set; } = string.Empty;
        public int RawScore { get; set; }
        public int StandardScore { get; set; }
        public string Category { get; set; } = string.Empty;
        public string InterpretationText { get; set; } = string.Empty;
    }

    public class IstReportViewModel
    {
        public Guid UserAssessmentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string SchoolName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        
        public Dictionary<string, Dictionary<string, string>> CleanedAnswers { get; set; } = new();
        
        // Properti Khusus Judgment
        public Dictionary<string, string> GeAnswers { get; set; } = new();
        public int GeRawScore { get; set; }
        public bool IsJudged { get; set; }

        public List<IstSubTestReportItem> SubTestResults { get; set; } = new();
        public int GesamtStandardScore { get; set; }
        public int CalculatedIQ { get; set; }
        public string IQClassification { get; set; } = string.Empty;
        public string IQIntelligenceLevel { get; set; } = string.Empty;

        public static async Task<IstReportViewModel?> BuildFromDatabaseAsync(ApplicationDbContext dbContext, Guid assessmentId)
        {
            var user = await dbContext.UserAssessments.AsNoTracking().FirstOrDefaultAsync(u => u.Id == assessmentId);
            if (user == null) return null;

            var rawAnswersList = await dbContext.UserAnswers.AsNoTracking()
                .Where(a => a.UserAssessmentId == assessmentId && a.SubTestName.StartsWith("IST_"))
                .ToListAsync();

            var cleanedAnswers = new Dictionary<string, Dictionary<string, string>>();
            foreach (var ans in rawAnswersList)
            {
                if (ans.Answers != null)
                {
                    try
                    {
                        var parsed = JsonSerializer.Deserialize<Dictionary<string, string>>(ans.Answers.RootElement.GetRawText());
                        if (parsed != null) cleanedAnswers[ans.SubTestName] = parsed;
                    }
                    catch { }
                }
            }

            // Cek apakah IST sudah dinilai (ada di UserTestResults)
            var testResult = await dbContext.UserTestResults.AsNoTracking()
                .FirstOrDefaultAsync(r => r.UserAssessmentId == assessmentId && r.TestCategory == "IST");

            var vm = new IstReportViewModel
            {
                UserAssessmentId = user.Id,
                FullName = user.FullName,
                SchoolName = user.SchoolName,
                BirthDate = user.BirthDate,
                CleanedAnswers = cleanedAnswers,
                IsJudged = testResult != null,
                GeAnswers = cleanedAnswers.GetValueOrDefault("IST_SubTest_04", new Dictionary<string, string>())
            };

            // Jika sudah dinilai, muat data dari JSONB
            if (testResult != null && testResult.ResultDetails.RootElement.ValueKind == JsonValueKind.Array)
            {
                vm.CalculatedIQ = testResult.OverallScore;
                vm.IQClassification = testResult.Classification;
                vm.SubTestResults = JsonSerializer.Deserialize<List<IstSubTestReportItem>>(testResult.ResultDetails.RootElement.GetRawText()) ?? new();
            }

            return vm;
        }
    }

    public class SubmitIstJudgmentDto
    {
        public Guid UserAssessmentId { get; set; }
        public int GeRawScore { get; set; }
    }
}