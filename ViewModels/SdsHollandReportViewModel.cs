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
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Score { get; set; }
        public double Percentage { get; set; }
    }

    public class SdsHollandReportViewModel
    {
        public Guid UserAssessmentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string SchoolName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;

        public Dictionary<string, string> UserResponses { get; set; } = new();
        public Dictionary<string, int> CalculatedScores { get; set; } = new();

        public List<RiasecScoreItem> RiasecScores { get; set; } = new();
        public string PrimaryCode { get; set; } = string.Empty;
        public string PrimaryCodeName { get; set; } = string.Empty;
        public string PrimaryDescription { get; set; } = string.Empty;
        public List<string> PrimaryCareers { get; set; } = new();

        public string SummaryCode { get; set; } = string.Empty;
        public string ConsistencyDegree { get; set; } = string.Empty;
        public string ConsistencyDetails { get; set; } = string.Empty;

        // Properti yang diperlukan oleh Engine & Controller
        public int TotalElevationScore { get; set; }
        public string ProfileElevationCategory { get; set; } = string.Empty;
        public string ElevationInterpretation { get; set; } = string.Empty;

        public static async Task<SdsHollandReportViewModel?> BuildFromDatabaseAsync(ApplicationDbContext dbContext, Guid assessmentId)
        {
            var user = await dbContext.UserAssessments
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == assessmentId);

            if (user == null) return null;

            var answerRecord = await dbContext.UserAnswers
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.UserAssessmentId == assessmentId && (a.SubTestName == "SDS_Holland" || a.SubTestName == "SDS-Holland"));

            var userResponses = new Dictionary<string, string>();
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

                    if (root.TryGetProperty("UserResponses", out var respObj) && respObj.ValueKind == JsonValueKind.Object)
                    {
                        foreach (var prop in respObj.EnumerateObject())
                        {
                            userResponses[prop.Name.ToUpper()] = prop.Value.GetString()?.ToUpper() ?? "";
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
                UserResponses = userResponses,
                CalculatedScores = existingScores
            };
        }
    }
}