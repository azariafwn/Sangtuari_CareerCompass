using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using SangtuariCareerCompass.ViewModels;

namespace SangtuariCareerCompass.Services.Scoring
{
    public class EasScoringEngine
    {
        private JsonElement _keyRoot;
        private JsonElement _normRoot;

        public EasScoringEngine()
        {
            LoadFiles();
        }

        private void LoadFiles()
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Scoring", "EAS");

            var keyPath = Path.Combine(basePath, "easKey.json");
            if (File.Exists(keyPath)) _keyRoot = JsonDocument.Parse(File.ReadAllText(keyPath)).RootElement;
            else _keyRoot = JsonDocument.Parse("{}").RootElement;

            var normPath = Path.Combine(basePath, "easNorms.json");
            if (File.Exists(normPath)) _normRoot = JsonDocument.Parse(File.ReadAllText(normPath)).RootElement;
            else _normRoot = JsonDocument.Parse("{}").RootElement;
        }

        public void ProcessScoring(EasReportViewModel vm)
        {
            vm.SubTestScores = new List<EasSubTestScore>();

            // Konfigurasi Subtes EAS yang digunakan
            var subTests = new[]
            {
                new { Id = 1, Name = "Kemampuan Verbal", NormKey = "EAS1" },
                new { Id = 2, Name = "Kemampuan Numerik", NormKey = "EAS2" },
                new { Id = 5, Name = "Visual Spasial", NormKey = "EAS5" }
            };

            foreach (var test in subTests)
            {
                // Antisipasi format dengan atau tanpa 0 (misal: EAS_SubTest_5 atau EAS_SubTest_05)
                string dbKey = $"EAS-{test.Id}";
                string jsonKey = $"EAS_SubTest_{test.Id}"; // Sesuai file easKey.json

                int rawScore = 0;

                Dictionary<string, string> userAnswers = null;
                if (vm.UserAnswers.ContainsKey(dbKey))
                {
                    userAnswers = vm.UserAnswers[dbKey];
                }

                if (userAnswers != null && _keyRoot.TryGetProperty(jsonKey, out var keyObj))
                {
                    foreach (var q in keyObj.EnumerateObject())
                    {
                        string expected = q.Value.GetString()?.ToLower() ?? "";
                        if (userAnswers.TryGetValue(q.Name, out string actual) && actual == expected)
                        {
                            rawScore++;
                        }
                    }
                }

                // 2. Konversi ke Percentile
                int percentile = 0;
                string rsStr = rawScore.ToString();
                if (_normRoot.TryGetProperty(test.NormKey, out var normObj) && normObj.TryGetProperty(rsStr, out var pctVal))
                {
                    percentile = pctVal.GetInt32();
                }

                // 3. Tentukan Kategori dari Percentile (Berdasarkan Blueprint EAS)
                vm.SubTestScores.Add(new EasSubTestScore
                {
                    SubTestName = $"EAS {test.Id}",
                    Aspek = test.Name,
                    RawScore = rawScore,
                    Percentile = percentile,
                    Category = DetermineCategory(percentile)
                });
            }
        }

        private string DetermineCategory(int percentile)
        {
            // Leveling Blueprint EAS
            if (percentile >= 81) return "Baik Sekali";
            if (percentile >= 61) return "Baik";
            if (percentile >= 41) return "Rata-rata";
            if (percentile >= 21) return "Kurang";
            return "Kurang Sekali"; // 0-20
        }
    }
}