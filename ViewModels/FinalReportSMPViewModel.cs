using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SangtuariCareerCompass.Data;

namespace SangtuariCareerCompass.ViewModels
{
    public class FinalReportSMPViewModel
    {
        public Guid UserAssessmentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string SchoolName { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;

        // CFIT & EAS
        public int CFIT_IQ { get; set; }
        public string CFIT_Kategori { get; set; } = string.Empty;
        public string EAS_Verbal { get; set; } = string.Empty;
        public string EAS_Numerik { get; set; } = string.Empty;
        public string EAS_Spasial { get; set; } = string.Empty;

        // VARK & Holland
        public string VarkJson { get; set; } = "[]";
        public string DominantVark { get; set; } = string.Empty;
        public string HollandJson { get; set; } = "{}";
        public string HollandSummaryCode { get; set; } = string.Empty;
        public string HollandConsistency { get; set; } = string.Empty;

        // Properti Baru untuk Rekomendasi Dinamis
        public List<string> HollandCareers { get; set; } = new();
        public List<string> HollandEducation { get; set; } = new();

        public static async Task<FinalReportSMPViewModel?> BuildFromDatabaseAsync(ApplicationDbContext dbContext, Guid assessmentId)
        {
            var user = await dbContext.UserAssessments.AsNoTracking().FirstOrDefaultAsync(u => u.Id == assessmentId);
            if (user == null) return null;

            var results = await dbContext.UserTestResults
                .AsNoTracking()
                .Where(r => r.UserAssessmentId == assessmentId)
                .ToListAsync();

            var vm = new FinalReportSMPViewModel
            {
                UserAssessmentId = user.Id,
                FullName = user.FullName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                SchoolName = user.SchoolName,
                Grade = user.ClassName
            };

            foreach (var res in results)
            {
                if (res.TestCategory == "CFIT")
                {
                    vm.CFIT_IQ = res.OverallScore;
                    vm.CFIT_Kategori = res.Classification;
                }
                else if (res.TestCategory == "EAS")
                {
                    var easList = JsonSerializer.Deserialize<List<EasSubTestScore>>(res.ResultDetails.RootElement.GetRawText());
                    if (easList != null)
                    {
                        vm.EAS_Verbal = easList.FirstOrDefault(x => x.Aspek.Contains("Verbal"))?.Category ?? "-";
                        vm.EAS_Numerik = easList.FirstOrDefault(x => x.Aspek.Contains("Numerik"))?.Category ?? "-";
                        vm.EAS_Spasial = easList.FirstOrDefault(x => x.Aspek.Contains("Spasial"))?.Category ?? "-";
                    }
                }
                else if (res.TestCategory == "VARK")
                {
                    vm.DominantVark = res.Classification;
                    vm.VarkJson = res.ResultDetails.RootElement.GetRawText();
                }
                else if (res.TestCategory == "SDS_Holland")
                {
                    vm.HollandSummaryCode = res.Classification;

                    var hollandData = JsonSerializer.Deserialize<JsonElement>(res.ResultDetails.RootElement.GetRawText());
                    if (hollandData.TryGetProperty("Consistency", out var cons)) vm.HollandConsistency = cons.GetString() ?? "-";

                    vm.HollandJson = res.ResultDetails.RootElement.GetRawText();
                }
            }

            // Ekstraksi Rekomendasi Karier & Pendidikan berdasarkan Kode Holland
            if (!string.IsNullOrEmpty(vm.HollandSummaryCode))
            {
                var careers = new HashSet<string>();
                var educations = new HashSet<string>();

                // Peta Statis Pendidikan (karena di JSON belum ada)
                var eduMap = new Dictionary<char, List<string>>
                {
                    {'R', new List<string>{"Teknik Mesin", "Teknik Sipil", "Ilmu Komputer"}},
                    {'I', new List<string>{"Biologi", "Kedokteran", "Sistem Informasi"}},
                    {'A', new List<string>{"Desain Komunikasi Visual", "Ilmu Komunikasi", "Sastra"}},
                    {'S', new List<string>{"Psikologi", "Bimbingan Konseling", "Ilmu Pendidikan"}},
                    {'E', new List<string>{"Manajemen Bisnis", "Hukum", "Hubungan Internasional"}},
                    {'C', new List<string>{"Akuntansi", "Administrasi Bisnis", "Statistika"}}
                };

                var riasecPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Scoring", "Holland", "riasecmap.json");
                if (File.Exists(riasecPath))
                {
                    var doc = JsonDocument.Parse(File.ReadAllText(riasecPath));
                    var typeMaster = doc.RootElement.GetProperty("typeMasterData");

                    // Ambil 2 huruf pertama dari Holland Summary Code (misal: "SI" dari "SIA")
                    int letterCount = Math.Min(2, vm.HollandSummaryCode.Length);
                    for (int i = 0; i < letterCount; i++)
                    {
                        char letter = vm.HollandSummaryCode[i];

                        // Ekstrak Karier dari JSON
                        if (typeMaster.TryGetProperty(letter.ToString(), out var typeObj) && typeObj.TryGetProperty("Careers", out var careerArr))
                        {
                            foreach (var c in careerArr.EnumerateArray()) careers.Add(c.GetString() ?? "");
                        }

                        // Ekstrak Pendidikan dari eduMap
                        if (eduMap.ContainsKey(letter))
                        {
                            foreach (var e in eduMap[letter]) educations.Add(e);
                        }
                    }
                }

                // Ambil 4 rekomendasi teratas agar UI tidak terlalu penuh
                vm.HollandCareers = careers.Take(4).ToList();
                vm.HollandEducation = educations.Take(4).ToList();
            }

            return vm;
        }
    }
}