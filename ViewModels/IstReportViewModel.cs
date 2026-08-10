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
        public int RawScore { get; set; } // RW
        public int StandardScore { get; set; } // SW
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
        public DateTime TestedAt { get; set; }

        public Dictionary<string, Dictionary<string, string>> CleanedAnswers { get; set; } = new();

        public List<IstSubTestReportItem> SubTestResults { get; set; } = new();
        public int GesamtStandardScore { get; set; }
        public int CalculatedIQ { get; set; }
        public string IQClassification { get; set; } = string.Empty;
        public string IQIntelligenceLevel { get; set; } = string.Empty;

        // Isolated LINQ Query Builder
        public static async Task<IstReportViewModel?> BuildFromDatabaseAsync(ApplicationDbContext dbContext, Guid assessmentId)
        {
            var user = await dbContext.UserAssessments
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == assessmentId);

            if (user == null) return null;

            var rawAnswersList = await dbContext.UserAnswers
                .AsNoTracking()
                .Where(a => a.UserAssessmentId == assessmentId && a.SubTestName.StartsWith("IST_"))
                .ToListAsync();

            var cleanedAnswers = new Dictionary<string, Dictionary<string, string>>();

            foreach (var ans in rawAnswersList)
            {
                try
                {
                    if (ans.Answers != null)
                    {
                        var parsed = JsonSerializer.Deserialize<Dictionary<string, string>>(ans.Answers.RootElement.GetRawText());
                        if (parsed != null)
                        {
                            cleanedAnswers[ans.SubTestName] = parsed;
                        }
                    }
                }
                catch { }
            }

            return new IstReportViewModel
            {
                UserAssessmentId = user.Id,
                FullName = user.FullName,
                SchoolName = user.SchoolName,
                BirthDate = user.BirthDate,
                CleanedAnswers = cleanedAnswers
            };
        }
    }
}