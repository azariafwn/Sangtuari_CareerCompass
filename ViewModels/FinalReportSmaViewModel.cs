using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SangtuariCareerCompass.Data;
using System.Threading.Tasks;

namespace SangtuariCareerCompass.ViewModels
{
    public class FinalReportSmaViewModel
    {
        public Guid UserAssessmentId { get; set; }
        public string AssessmentNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string SchoolName { get; set; } = string.Empty;
        public DateTime TestedAt { get; set; }
        public DateTime? BirthDate { get; set; }

        public int IstIqScore { get; set; }
        public string IstIqClassification { get; set; } = string.Empty;
        public Dictionary<string, string> IstAspectCategories { get; set; } = new();
        public Dictionary<string, string> PapiAspectJudgments { get; set; } = new();
        public string VarkDominant { get; set; } = string.Empty;
        public Dictionary<string, double> VarkPercentages { get; set; } = new();
        public string HollandSummaryCode { get; set; } = string.Empty;
        public string HollandConsistency { get; set; } = string.Empty;
        public List<string> HollandCareers { get; set; } = new();
        public List<string> HollandEducation { get; set; } = new();
        public Dictionary<string, double> HollandScores { get; set; } = new();

        public static async Task<FinalReportSmaViewModel?> BuildFromDatabaseAsync(ApplicationDbContext dbContext, Guid assessmentId)
        {
            var user = await dbContext.UserAssessments.AsNoTracking().FirstOrDefaultAsync(u => u.Id == assessmentId);
            if (user == null) return null;

            var results = await dbContext.UserTestResults.AsNoTracking()
                .Where(r => r.UserAssessmentId == assessmentId)
                .ToListAsync();

            var vm = new FinalReportSmaViewModel
            {
                UserAssessmentId = user.Id,
                // Mengambil 8 karakter awal GUID sebagai No. Pemeriksaan
                AssessmentNumber = user.Id.ToString().Substring(0, 8).ToUpper(),
                FullName = user.FullName,
                Gender = user.Gender,
                SchoolName = user.SchoolName,
                TestedAt = user.CreatedAt,
                BirthDate = user.BirthDate
            };

            foreach (var res in results)
            {
                try
                {
                    var root = res.ResultDetails.RootElement;
                    if (res.TestCategory == "IST")
                    {
                        vm.IstIqScore = res.OverallScore;
                        vm.IstIqClassification = res.Classification;
                        if (root.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var item in root.EnumerateArray())
                            {
                                vm.IstAspectCategories[item.GetProperty("AspectName").GetString() ?? ""] = item.GetProperty("Category").GetString() ?? "";
                            }
                        }
                    }
                    else if (res.TestCategory == "PAPI_Kostick")
                    {
                        if (root.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var aspect in root.EnumerateArray())
                            {
                                vm.PapiAspectJudgments[aspect.GetProperty("AspectName").GetString() ?? ""] = aspect.GetProperty("FinalJudgment").GetString() ?? "";
                            }
                        }
                    }
                    else if (res.TestCategory == "VARK")
                    {
                        vm.VarkDominant = res.Classification;
                        if (root.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var item in root.EnumerateArray())
                            {
                                vm.VarkPercentages[item.GetProperty("CategoryName").GetString() ?? ""] = item.GetProperty("Percentage").GetDouble();
                            }
                        }
                    }
                    else if (res.TestCategory == "SDS_Holland")
                    {
                        if (root.ValueKind == JsonValueKind.Object)
                        {
                            vm.HollandSummaryCode = root.GetProperty("SummaryCode").GetString() ?? "";
                            vm.HollandConsistency = root.GetProperty("Consistency").GetString() ?? "";
                            if (root.TryGetProperty("Scores", out var scoresArray) && scoresArray.ValueKind == JsonValueKind.Array)
                            {
                                foreach (var item in scoresArray.EnumerateArray())
                                {
                                    vm.HollandScores[item.GetProperty("Name").GetString() ?? ""] = item.GetProperty("Percentage").GetDouble();
                                }
                            }
                        }
                    }
                }
                catch { }
            }

            // Membaca Karier dari typeMasterData berdasarkan Kode Dominan (Huruf Pertama)
            if (!string.IsNullOrEmpty(vm.HollandSummaryCode))
            {
                char dom = vm.HollandSummaryCode.FirstOrDefault();
                string domCode = dom.ToString();

                var hollandPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Scoring", "Holland", "riasecMap.json");
                if (File.Exists(hollandPath))
                {
                    var jsonDoc = JsonDocument.Parse(File.ReadAllText(hollandPath)).RootElement;
                    if (jsonDoc.TryGetProperty("typeMasterData", out var masterData) && masterData.TryGetProperty(domCode, out var typeData))
                    {
                        if (typeData.TryGetProperty("Careers", out var careerArr))
                        {
                            vm.HollandCareers = careerArr.EnumerateArray().Select(c => c.GetString() ?? "").ToList();
                        }
                    }
                }

                // Set Rekomendasi Pendidikan berdasarkan Tipe Dominan
                vm.HollandEducation = dom switch
                {
                    'R' => new List<string> { "S1 Teknik Mesin", "S1 Teknik Sipil", "S1 Teknik Penerbangan/Aeronautika" },
                    'I' => new List<string> { "S1 Kedokteran", "S1 Ilmu Komputer", "S1 Matematika/Statistika" },
                    'A' => new List<string> { "S1 Desain Komunikasi Visual (DKV)", "S1 Ilmu Komunikasi", "S1 Arsitektur" },
                    'S' => new List<string> { "S1 Psikologi", "S1 Bimbingan Konseling (BK)", "S1 Keperawatan" },
                    'E' => new List<string> { "S1 Manajemen Bisnis", "S1 Hukum", "S1 Hubungan Internasional" },
                    'C' => new List<string> { "S1 Akuntansi", "S1 Sistem Informasi", "S1 Administrasi Publik" },
                    _ => new List<string>()
                };
            }

            return vm;
        }
    }
}