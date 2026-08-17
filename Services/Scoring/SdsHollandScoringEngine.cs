using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using SangtuariCareerCompass.ViewModels;

namespace SangtuariCareerCompass.Services.Scoring
{
    public class SdsHollandScoringEngine
    {
        private JsonElement _rulesData;

        public SdsHollandScoringEngine()
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Scoring", "Holland", "riasecMap.json");
            if (File.Exists(jsonPath))
            {
                _rulesData = JsonSerializer.Deserialize<JsonDocument>(File.ReadAllText(jsonPath))!.RootElement;
            }
        }

        public void ProcessScoring(SdsHollandReportViewModel vm)
        {
            var rawOrder = new[] { "R", "I", "A", "S", "E", "C" };
            var list = new List<RiasecScoreItem>();

            int totalScore = 0;
            foreach (var code in rawOrder)
            {
                int val = vm.ExistingScores.GetValueOrDefault(code, 0);
                totalScore += val;

                string name = code switch
                {
                    "R" => "Realistic",
                    "I" => "Investigative",
                    "A" => "Artistic",
                    "S" => "Social",
                    "E" => "Enterprising",
                    "C" => "Conventional",
                    _ => code
                };

                list.Add(new RiasecScoreItem
                {
                    Code = code,
                    Name = name,
                    Score = val
                });
            }

            double totalDiv = totalScore > 0 ? (double)totalScore : 1.0;
            list.ForEach(x => x.Percentage = Math.Round((x.Score / totalDiv) * 100, 1));
            vm.RiasecScores = list;
            vm.TotalElevationScore = totalScore;

            // 1. Tentukan Summary Code (Top 3 Scores)
            var sorted = list.OrderByDescending(x => x.Score).ToList();
            var top3 = sorted.Take(3).ToList();
            vm.PrimaryCode = top3.FirstOrDefault()?.Code ?? "R";
            vm.SummaryCode = string.Join("", top3.Select(x => x.Code));

            // 2. Evaluasi Profile Elevation berdasarkan Gender
            bool isMale = vm.Gender.Contains("Laki", StringComparison.OrdinalIgnoreCase);
            if (isMale)
            {
                if (totalScore > 150) { vm.ProfileElevationCategory = "Tinggi"; vm.ElevationInterpretation = "Gaya ekspresif, antusias, memiliki efektivitas keberfungsian edukasional, dan minat terarah jelas."; }
                else if (totalScore >= 129) { vm.ProfileElevationCategory = "Rata-rata"; vm.ElevationInterpretation = "Memiliki stabilitas minat yang seimbang dalam eksplorasi karier umum."; }
                else { vm.ProfileElevationCategory = "Rendah"; vm.ElevationInterpretation = "Cenderung kurang antusias terhadap bidang konvensional atau memerlukan eksplorasi alternatif minat lebih lanjut."; }
            }
            else
            {
                if (totalScore > 147) { vm.ProfileElevationCategory = "Tinggi"; vm.ElevationInterpretation = "Gaya ekspresif, antusias, memiliki efektivitas keberfungsian edukasional, dan minat terarah jelas."; }
                else if (totalScore >= 128) { vm.ProfileElevationCategory = "Rata-rata"; vm.ElevationInterpretation = "Memiliki stabilitas minat yang seimbang dalam eksplorasi karier umum."; }
                else { vm.ProfileElevationCategory = "Rendah"; vm.ElevationInterpretation = "Cenderung kurang antusias terhadap bidang konvensional atau memerlukan eksplorasi alternatif minat lebih lanjut."; }
            }

            // 3. Evaluasi Konsistensi 2 Huruf Pertama pada Model Heksagon
            if (vm.SummaryCode.Length >= 2)
            {
                string pair = vm.SummaryCode.Substring(0, 2);
                vm.ConsistencyDegree = GetConsistencyDegree(pair);
                vm.ConsistencyInterpretation = vm.ConsistencyDegree switch
                {
                    "Adjacent (Tinggi)" => "Minat Anda memiliki konsistensi tinggi karena kedua tipe utama saling berdampingan dan saling menguatkan dalam kepribadian kerja.",
                    "Alternate (Sedang)" => "Minat Anda memiliki konsistensi sedang dengan kombinasi variasi kompetensi yang fleksibel.",
                    "Opposite (Rendah)" => "Minat Anda memiliki polarisasi berseberangan, menunjukkan keberagaman minat yang unik namun memerlukan integrasi fokus karier.",
                    _ => "Konsistensi minat dalam kategori seimbang."
                };
            }

            // 4. Rekomendasi Karier dari Top Codes
            var careers = new HashSet<string>();
            if (_rulesData.ValueKind == JsonValueKind.Object && _rulesData.TryGetProperty("careerRecommendations", out var careerObj))
            {
                foreach (var topItem in top3)
                {
                    if (careerObj.TryGetProperty(topItem.Code, out var rec))
                    {
                        if (rec.TryGetProperty("Careers", out var careerArray))
                        {
                            foreach (var c in careerArray.EnumerateArray())
                            {
                                if (c.GetString() is string cStr) careers.Add(cStr);
                            }
                        }
                    }
                }
            }
            vm.RecommendedCareers = careers.Take(12).ToList();
        }

        private string GetConsistencyDegree(string pair)
        {
            if (_rulesData.ValueKind == JsonValueKind.Object && _rulesData.TryGetProperty("consistencyRules", out var rules))
            {
                if (rules.TryGetProperty("Adjacent", out var adj) && adj.EnumerateArray().Any(x => x.GetString() == pair))
                    return "Adjacent (Tinggi)";
                if (rules.TryGetProperty("Alternate", out var alt) && alt.EnumerateArray().Any(x => x.GetString() == pair))
                    return "Alternate (Sedang)";
                if (rules.TryGetProperty("Opposite", out var opp) && opp.EnumerateArray().Any(x => x.GetString() == pair))
                    return "Opposite (Rendah)";
            }
            return "Adjacent (Tinggi)";
        }
    }
}