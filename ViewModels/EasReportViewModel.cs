using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SangtuariCareerCompass.Data;

namespace SangtuariCareerCompass.ViewModels
{
    public class EasSubTestScore
    {
        public string SubTestName { get; set; } = string.Empty; // EAS 1, EAS 2, EAS 5
        public string Aspek { get; set; } = string.Empty; // Verbal, Numerik, Visual Spasial
        public int RawScore { get; set; }
        public int Percentile { get; set; }
        public string Category { get; set; } = string.Empty;
    }

    public class EasReportViewModel
    {
        public Guid UserAssessmentId { get; set; }
        public string FullName { get; set; } = string.Empty;

        // Menyimpan jawaban user untuk EAS 1, 2, 5
        public Dictionary<string, Dictionary<string, string>> UserAnswers { get; set; } = new();

        public List<EasSubTestScore> SubTestScores { get; set; } = new();

        public static async Task<EasReportViewModel?> BuildFromDatabaseAsync(ApplicationDbContext dbContext, Guid assessmentId)
        {
            var user = await dbContext.UserAssessments.AsNoTracking().FirstOrDefaultAsync(u => u.Id == assessmentId);
            if (user == null) return null;

            var answers = await dbContext.UserAnswers
                .AsNoTracking()
                .Where(a => a.UserAssessmentId == assessmentId && a.SubTestName.StartsWith("EAS"))
                .ToListAsync();

            var userAnsDict = new Dictionary<string, Dictionary<string, string>>();
            foreach (var ans in answers)
            {
                try
                {
                    var dict = new Dictionary<string, string>();
                    foreach (var prop in ans.Answers.RootElement.EnumerateObject())
                    {
                        dict[prop.Name] = prop.Value.GetString()?.ToLower() ?? "";
                    }
                    userAnsDict[ans.SubTestName] = dict;
                }
                catch { }
            }

            return new EasReportViewModel
            {
                UserAssessmentId = user.Id,
                FullName = user.FullName,
                UserAnswers = userAnsDict
            };
        }
    }
}