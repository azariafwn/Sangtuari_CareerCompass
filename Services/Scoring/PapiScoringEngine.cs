using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SangtuariCareerCompass.ViewModels;

namespace SangtuariCareerCompass.Services.Scoring
{
    public class PapiScoringEngine
    {
        private JsonElement _scoringKey;
        private JsonElement _aspectMapping;

        public PapiScoringEngine()
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Scoring", "PAPI", "papiMap.json");
            if (File.Exists(jsonPath))
            {
                var doc = JsonSerializer.Deserialize<JsonDocument>(File.ReadAllText(jsonPath))!.RootElement;
                _scoringKey = doc.GetProperty("scoringKey");
                _aspectMapping = doc.GetProperty("aspectMapping");
            }
        }

        public void ProcessRawScoring(PapiReportViewModel vm)
        {
            if (vm.IsJudged && vm.AspectGroups.Count > 0) return; // Lewati jika sudah di-judge

            var factorScores = new Dictionary<string, int>();
            string[] allFactors = { "G", "L", "I", "T", "V", "S", "R", "D", "C", "E", "W", "F", "K", "Z", "O", "B", "X", "P", "A", "N" };
            foreach (var f in allFactors) factorScores[f] = 0;

            // 1. Kalkulasi Raw Score berdasarkan 90 Item
            foreach (var ans in vm.RawAnswers)
            {
                string qNum = ans.Key.Replace("Q", ""); // "Q1" -> "1"
                string choice = ans.Value; // "A" atau "B"

                if (_scoringKey.TryGetProperty(qNum, out var qNode) && qNode.TryGetProperty(choice, out var factorNode))
                {
                    string factor = factorNode.GetString() ?? "";
                    if (factorScores.ContainsKey(factor)) factorScores[factor]++;
                }
            }

            // 2. Kelompokkan ke Aspek & Leveling
            vm.AspectGroups.Clear();
            foreach (var aspect in _aspectMapping.EnumerateObject())
            {
                var group = new PapiAspectGroup { AspectName = aspect.Name };
                foreach (var fNode in aspect.Value.EnumerateArray())
                {
                    string fCode = fNode.GetString() ?? "";
                    int rawScore = factorScores[fCode];
                    group.Factors.Add(new PapiFactorScore
                    {
                        Code = fCode,
                        RawScore = rawScore,
                        Leveling = GetFactorLeveling(fCode, rawScore)
                    });
                }
                vm.AspectGroups.Add(group);
            }
        }

        private string GetFactorLeveling(string code, int score)
        {
            // Leveling disederhanakan berdasarkan referensi Norma PAPI
            // Kategori: Kurang Sekali, Kurang, Rata-rata, Baik, Baik Sekali
            return code switch
            {
                "N" => score >= 6 ? "Baik" : (score >= 4 ? "Rata-rata" : "Kurang"),
                "G" => score >= 4 ? "Baik" : "Kurang",
                "A" => score >= 6 ? "Baik" : "Kurang",
                "L" => score >= 5 ? "Baik" : "Kurang",
                "P" => score >= 5 ? "Baik" : "Kurang",
                "I" => score >= 8 ? "Baik Sekali" : (score >= 5 ? "Baik" : (score >= 3 ? "Rata-rata" : "Kurang")),
                "C" => score >= 6 ? "Baik Sekali" : (score >= 3 ? "Rata-rata" : "Kurang"),
                "D" => score >= 4 ? "Baik" : "Kurang",
                "R" => score >= 5 ? "Baik" : "Kurang",
                "O" => score >= 5 ? "Baik Sekali" : (score >= 3 ? "Rata-rata" : "Kurang Sekali"),
                "B" => score >= 6 ? "Baik" : (score >= 4 ? "Rata-rata" : "Kurang Sekali"),
                "S" => score >= 6 ? "Baik" : "Kurang",
                "X" => score >= 6 ? "Baik Sekali" : (score >= 4 ? "Baik" : (score >= 2 ? "Rata-rata" : "Kurang")),
                "Z" => score >= 8 ? "Kurang" : (score >= 5 ? "Baik" : "Rata-rata"),
                "E" => score > 6 ? "Kurang Sekali" : (score >= 4 ? "Baik" : "Rata-rata"),
                "K" => score >= 8 ? "Kurang" : (score >= 6 ? "Baik" : (score == 5 ? "Rata-rata" : "Kurang Sekali")),
                "T" => score >= 4 ? "Baik" : "Rata-rata",
                "V" => score >= 5 ? "Baik" : "Rata-rata",
                "F" => score >= 6 ? "Baik Sekali" : (score >= 4 ? "Baik" : "Kurang"),
                "W" => score >= 6 ? "Baik" : (score >= 4 ? "Rata-rata" : "Kurang"),
                _ => "Rata-rata"
            };
        }
    }
}