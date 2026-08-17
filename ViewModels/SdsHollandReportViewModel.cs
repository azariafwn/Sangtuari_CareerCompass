using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SangtuariCareerCompass.Data;
using System.Threading.Tasks;

namespace SangtuariCareerCompass.ViewModels
{
    public class RiasecScoreItem
    {
        public string Code { get; set; } = string.Empty; // R, I, A, S, E, C
        public string Name { get; set; } = string.Empty;
        public int Score { get; set; }
        public double Percentage { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class SdsHollandReportViewModel
    {
        public Guid UserAssessmentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string SchoolName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;

        public Dictionary<string, int> ExistingScores { get; set; } = new();

        public List<RiasecScoreItem> RiasecScores { get; set; } = new();
        public string PrimaryCode { get; set; } = string.Empty;
        public string SummaryCode { get; set; } = string.Empty; // Top 3 codes (misal: "AIC")
        public int TotalElevationScore { get; set; }
        public string ProfileElevationCategory { get; set; } = string.Empty;
        public string ElevationInterpretation { get; set; } = string.Empty;
        public string ConsistencyDegree { get; set; } = string.Empty; // Adjacent, Alternate, Opposite
        public string ConsistencyInterpretation { get; set; } = string.Empty;
        public List<string> RecommendedCareers { get; set; } = new();

        public static async Task<SdsHollandReportViewModel?> BuildFromDatabaseAsync(ApplicationDbContext dbContext, Guid assessmentId)
        {
            var user = await dbContext.UserAssessments
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == assessmentId);

            if (user == null) return null;

            var answerRecord = await dbContext.UserAnswers
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.UserAssessmentId == assessmentId && (a.SubTestName == "SDS_Holland" || a.SubTestName == "SDS-Holland"));

            var existingScores = new Dictionary<string, int>();

            if (answerRecord?.Answers != null)
            {
                try
                {
                    using var doc = JsonDocument.Parse(answerRecord.Answers.RootElement.GetRawText());
                    var root = doc.RootElement;

                    if (root.TryGetProperty("Scores", out var scoresObj) && scoresObj.ValueKind == JsonValueKind.Object)
                    {
                        foreach (var prop in scoresObj.EnumerateObject())
                        {
                            existingScores[prop.Name.ToUpper()] = prop.Value.GetInt32();
                        }
                    }
                }
                catch { }
            }

            return new SdsHollandReportViewModel
            {
                UserAssessmentId = user.Id,
                FullName = user.FullName,
                SchoolName = user.SchoolName,
                Gender = user.Gender,
                ExistingScores = existingScores
            };
        }
    }
}