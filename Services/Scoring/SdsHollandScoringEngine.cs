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
            var scoresDict = new Dictionary<string, int>();

            if (vm.CalculatedScores.Count == 6)
            {
                scoresDict = vm.CalculatedScores;
            }
            else if (_rulesData.ValueKind == JsonValueKind.Object && _rulesData.TryGetProperty("itemCategories", out var itemCatObj))
            {
                foreach (var code in rawOrder)
                {
                    int sum = 0;
                    if (itemCatObj.TryGetProperty(code, out var itemArr))
                    {
                        foreach (var itemNum in itemArr.EnumerateArray())
                        {
                            string qKey = $"Q{itemNum.GetInt32()}";
                            if (vm.UserResponses.TryGetValue(qKey, out var ans) && ans == "YA")
                            {
                                sum++;
                            }
                        }
                    }
                    scoresDict[code] = sum;
                }
            }

            int totalElevation = scoresDict.Values.Sum();
            double totalDiv = totalElevation > 0 ? (double)totalElevation : 1.0;

            var list = new List<RiasecScoreItem>();
            foreach (var code in rawOrder)
            {
                int val = scoresDict.GetValueOrDefault(code, 0);
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
                    Score = val,
                    Percentage = Math.Round((val / totalDiv) * 100, 1)
                });
            }

            vm.RiasecScores = list;

            // 1. Urutkan untuk membentuk Summary Code 3 Huruf
            var sorted = list.OrderByDescending(x => x.Score).ToList();
            var top3 = sorted.Take(3).ToList();
            vm.PrimaryCode = top3.FirstOrDefault()?.Code ?? "R";
            vm.SummaryCode = string.Join("", top3.Select(x => x.Code));

            // 2. Deskripsi & Karir Berdasarkan Kode Dominan Saja
            if (_rulesData.ValueKind == JsonValueKind.Object &&
                _rulesData.TryGetProperty("typeMasterData", out var masterObj) &&
                masterObj.TryGetProperty(vm.PrimaryCode, out var primaryObj))
            {
                vm.PrimaryCodeName = primaryObj.GetProperty("Name").GetString() ?? "";
                vm.PrimaryDescription = primaryObj.GetProperty("Description").GetString() ?? "";

                var careers = new List<string>();
                if (primaryObj.TryGetProperty("Careers", out var careerArr))
                {
                    foreach (var c in careerArr.EnumerateArray())
                    {
                        if (c.GetString() is string cStr) careers.Add(cStr);
                    }
                }
                vm.PrimaryCareers = careers;
            }

            // 3. Evaluasi Tingkat Konsistensi Ganda (Kombinasi Huruf 1-2 dan 2-3)
            if (vm.SummaryCode.Length >= 3)
            {
                string pair1 = vm.SummaryCode.Substring(0, 2);
                string pair2 = vm.SummaryCode.Substring(1, 2);

                string level1 = GetSinglePairConsistency(pair1);
                string level2 = GetSinglePairConsistency(pair2);

                vm.ConsistencyDegree = DetermineLowestConsistency(level1, level2);
                vm.ConsistencyDetails = $"Kombinasi {pair1} = {level1}, {pair2} = {level2}. Konsistensi keseluruhan: {vm.ConsistencyDegree}.";
            }
            else
            {
                vm.ConsistencyDegree = "Tinggi";
                vm.ConsistencyDetails = "Konsistensi stabil.";
            }
        }

        private string GetSinglePairConsistency(string pair)
        {
            if (_rulesData.ValueKind == JsonValueKind.Object && _rulesData.TryGetProperty("consistencyRules", out var rules))
            {
                if (rules.TryGetProperty("Adjacent", out var adj) && adj.EnumerateArray().Any(x => x.GetString() == pair))
                    return "Tinggi";
                if (rules.TryGetProperty("Alternate", out var alt) && alt.EnumerateArray().Any(x => x.GetString() == pair))
                    return "Sedang";
                if (rules.TryGetProperty("Opposite", out var opp) && opp.EnumerateArray().Any(x => x.GetString() == pair))
                    return "Rendah";
            }
            return "Sedang";
        }

        private string DetermineLowestConsistency(string c1, string c2)
        {
            if (c1 == "Rendah" || c2 == "Rendah") return "Rendah";
            if (c1 == "Sedang" || c2 == "Sedang") return "Sedang";
            return "Tinggi";
        }
    }
}