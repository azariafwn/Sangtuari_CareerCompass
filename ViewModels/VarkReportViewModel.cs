using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SangtuariCareerCompass.Data;
using System.Threading.Tasks;

namespace SangtuariCareerCompass.ViewModels
{
    public class VarkScoreItem
    {
        public string CategoryCode { get; set; } = string.Empty; // V, A, R, K
        public string CategoryName { get; set; } = string.Empty; // Visual, Aural/auditori, Read/Write, Kinesthetic
        public int Score { get; set; }
        public double Percentage { get; set; }
    }

    public class VarkReportViewModel
    {
        public Guid UserAssessmentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string SchoolName { get; set; } = string.Empty;

        // Menyimpan pre-calculated scores jika ada di JSON DB
        public Dictionary<string, int> ExistingScores { get; set; } = new();

        // Jawaban mentah: { "Q1": ["c", "d"], "Q2": ["b", "c"] }
        public Dictionary<string, List<string>> UserAnswers { get; set; } = new();

        public List<VarkScoreItem> CategoryScores { get; set; } = new();
        public List<string> DominantCategories { get; set; } = new();
        public string DominantCategoryText { get; set; } = string.Empty;
        public string InterpretationText { get; set; } = string.Empty;
        public string RecommendedStudyMethods { get; set; } = string.Empty;

        public static async Task<VarkReportViewModel?> BuildFromDatabaseAsync(ApplicationDbContext dbContext, Guid assessmentId)
        {
            var user = await dbContext.UserAssessments
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == assessmentId);

            if (user == null) return null;

            var answerRecord = await dbContext.UserAnswers
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.UserAssessmentId == assessmentId && a.SubTestName == "VARK");

            var cleanedAnswers = new Dictionary<string, List<string>>();
            var existingScores = new Dictionary<string, int>();

            if (answerRecord?.Answers != null)
            {
                try
                {
                    using var doc = JsonDocument.Parse(answerRecord.Answers.RootElement.GetRawText());
                    var root = doc.RootElement;

                    // 1. Ekstrak Scores jika ada
                    if (root.TryGetProperty("Scores", out var scoresObj) && scoresObj.ValueKind == JsonValueKind.Object)
                    {
                        foreach (var prop in scoresObj.EnumerateObject())
                        {
                            existingScores[prop.Name.ToUpper()] = prop.Value.GetInt32();
                        }
                    }

                    // 2. Ekstrak UserResponses
                    JsonElement responsesElement = root;
                    if (root.TryGetProperty("UserResponses", out var userRespObj))
                    {
                        responsesElement = userRespObj;
                    }

                    if (responsesElement.ValueKind == JsonValueKind.Object)
                    {
                        foreach (var prop in responsesElement.EnumerateObject())
                        {
                            var list = new List<string>();
                            if (prop.Value.ValueKind == JsonValueKind.Array)
                            {
                                foreach (var item in prop.Value.EnumerateArray())
                                {
                                    if (item.GetString() is string val) list.Add(val.ToLower());
                                }
                            }
                            else if (prop.Value.ValueKind == JsonValueKind.String)
                            {
                                if (prop.Value.GetString() is string val) list.Add(val.ToLower());
                            }

                            cleanedAnswers[prop.Name] = list;
                        }
                    }
                }
                catch { }
            }

            return new VarkReportViewModel
            {
                UserAssessmentId = user.Id,
                FullName = user.FullName,
                SchoolName = user.SchoolName,
                UserAnswers = cleanedAnswers,
                ExistingScores = existingScores
            };
        }
    }
}