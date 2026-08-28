using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SangtuariCareerCompass.Data;

namespace SangtuariCareerCompass.ViewModels
{
    public class CfitSubTestScore
    {
        public string SubTestName { get; set; } = string.Empty;
        public int CorrectCount { get; set; }
        public int TotalSoal { get; set; }
    }

    public class CfitReportViewModel
    {
        public Guid UserAssessmentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int AgeInMonths { get; set; }

        // Menyimpan semua jawaban user: { "CFIT_SubTest_1": { "Q1": "a", ... } }
        public Dictionary<string, Dictionary<string, string>> UserAnswers { get; set; } = new();

        public List<CfitSubTestScore> SubTestScores { get; set; } = new();
        public int TotalRawScore { get; set; }
        public int CalculatedIQ { get; set; }
        public string IQClassification { get; set; } = string.Empty;

        public static async Task<CfitReportViewModel?> BuildFromDatabaseAsync(ApplicationDbContext dbContext, Guid assessmentId)
        {
            var user = await dbContext.UserAssessments.AsNoTracking().FirstOrDefaultAsync(u => u.Id == assessmentId);
            if (user == null) return null;

            // Kalkulasi Umur dalam Bulan (Asumsi usia SMP = 13-15 tahun, fallback 15 tahun/180 bulan jika kosong)
            int ageMonths = 180; // Fallback 15 tahun
            if (user.BirthDate != default(DateTime))
            {
                var today = DateTime.Today;
                ageMonths = ((today.Year - user.BirthDate.Year) * 12) + today.Month - user.BirthDate.Month;
            }

            var answers = await dbContext.UserAnswers
                .AsNoTracking()
                .Where(a => a.UserAssessmentId == assessmentId && a.SubTestName.StartsWith("CFIT"))
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

            return new CfitReportViewModel
            {
                UserAssessmentId = user.Id,
                FullName = user.FullName,
                AgeInMonths = ageMonths,
                UserAnswers = userAnsDict
            };
        }
    }
}