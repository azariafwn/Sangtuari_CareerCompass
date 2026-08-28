using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using SangtuariCareerCompass.ViewModels;

namespace SangtuariCareerCompass.Services.Scoring
{
    public class CfitScoringEngine
    {
        private JsonElement _keyRoot;
        private JsonElement _normRoot;

        public CfitScoringEngine()
        {
            LoadFiles();
        }

        private void LoadFiles()
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Scoring", "CFIT");

            var keyPath = Path.Combine(basePath, "cfitKey.json");
            if (File.Exists(keyPath)) _keyRoot = JsonDocument.Parse(File.ReadAllText(keyPath)).RootElement;

            var normPath = Path.Combine(basePath, "cfitNorms.json");
            if (File.Exists(normPath)) _normRoot = JsonDocument.Parse(File.ReadAllText(normPath)).RootElement;
        }

        public void ProcessScoring(CfitReportViewModel vm)
        {
            int totalCorrect = 0;
            vm.SubTestScores = new List<CfitSubTestScore>();

            for (int i = 1; i <= 4; i++)
            {
                // Antisipasi format JSON key "CFIT_SubTest_1" atau "CFIT_SubTest_01"
                string keySubName = $"CFIT_SubTest_{i}";

                // Antisipasi format database "CFIT_SubTest_01" atau "CFIT_SubTest_1"
                string dbKeyWithZero = $"CFIT_SubTest_{i:D2}";
                string dbKeyNoZero = $"CFIT_SubTest_{i}";

                int correct = 0;
                int totalSoal = 0;

                // Cari jawaban user menggunakan format 0X atau X
                Dictionary<string, string> userAnswers = null;
                if (vm.UserAnswers.ContainsKey(dbKeyWithZero)) userAnswers = vm.UserAnswers[dbKeyWithZero];
                else if (vm.UserAnswers.ContainsKey(dbKeyNoZero)) userAnswers = vm.UserAnswers[dbKeyNoZero];

                if (userAnswers != null && _keyRoot.TryGetProperty(keySubName, out var keyObj))
                {
                    foreach (var q in keyObj.EnumerateObject())
                    {
                        totalSoal++;
                        string expected = q.Value.GetString()?.ToLower() ?? "";

                        if (userAnswers.TryGetValue(q.Name, out string actual) && actual == expected)
                        {
                            correct++;
                        }
                    }
                }

                vm.SubTestScores.Add(new CfitSubTestScore
                {
                    SubTestName = $"Subtes {i}",
                    CorrectCount = correct,
                    TotalSoal = totalSoal
                });

                totalCorrect += correct;
            }

            vm.TotalRawScore = totalCorrect;
            vm.CalculatedIQ = MapRawScoreToIQ(vm.TotalRawScore, vm.AgeInMonths);
            vm.IQClassification = DetermineClassification(vm.CalculatedIQ);
        }

        private int MapRawScoreToIQ(int rawScore, int ageMonths)
        {
            // Tentukan indeks kolom norma berdasarkan usia bulan[cite: 14]
            int ageIndex = 5; // Default >= 17 tahun (204 bulan)
            if (ageMonths <= 160) ageIndex = 0;      // 13.0 - 13.4
            else if (ageMonths <= 167) ageIndex = 1; // 13.5 - 13.11
            else if (ageMonths <= 179) ageIndex = 2; // 14.0 - 14.11
            else if (ageMonths <= 191) ageIndex = 3; // 15.0 - 15.11
            else if (ageMonths <= 203) ageIndex = 4; // 16.0 - 16.11

            // Limit Raw Score agar tidak meledak di luar index (Max 46 untuk skala ini)
            string rsKey = Math.Clamp(rawScore, 0, 46).ToString();

            if (_normRoot.TryGetProperty("NormTable", out var normTable) &&
                normTable.TryGetProperty(rsKey, out var rowArr))
            {
                var arr = rowArr.EnumerateArray().ToList();
                if (arr.Count > ageIndex) return arr[ageIndex].GetInt32();
            }

            return 100; // Fallback jika raw score tidak ditemukan
        }

        private string DetermineClassification(int iq)
        {
            // Standar Deviasi Klasifikasi IQ CFIT[cite: 14]
            if (iq >= 170) return "Genius";
            if (iq >= 140) return "Very Superior";
            if (iq >= 120) return "Superior";
            if (iq >= 110) return "High Average";
            if (iq >= 90) return "Average";
            if (iq >= 80) return "Low Average";
            if (iq >= 70) return "Borderline Mental Retardation";
            if (iq >= 52) return "Mild Mental Retardation";
            if (iq >= 36) return "Moderate Mental Retardation";
            if (iq >= 25) return "Severe Mental Retardation";
            return "Profound Mental Retardation";
        }
    }
}