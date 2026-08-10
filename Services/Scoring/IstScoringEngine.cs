using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SangtuariCareerCompass.ViewModels;

namespace SangtuariCareerCompass.Services.Scoring
{
    public class IstScoringEngine
    {
        private JsonElement _answerKeys;
        private JsonElement _ageNorms;
        private JsonElement _iqConversion;

        public IstScoringEngine()
        {
            LoadScoringData();
        }

        private void LoadScoringData()
        {
            var baseFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Scoring", "IST");

            var keysPath = Path.Combine(baseFolder, "answerKeys.json");
            var normsPath = Path.Combine(baseFolder, "ageNorms.json");
            var iqPath = Path.Combine(baseFolder, "iqConversion.json");

            if (File.Exists(keysPath))
                _answerKeys = JsonSerializer.Deserialize<JsonDocument>(File.ReadAllText(keysPath))!.RootElement;

            if (File.Exists(normsPath))
                _ageNorms = JsonSerializer.Deserialize<JsonDocument>(File.ReadAllText(normsPath))!.RootElement;

            if (File.Exists(iqPath))
                _iqConversion = JsonSerializer.Deserialize<JsonDocument>(File.ReadAllText(iqPath))!.RootElement;
        }

        public void ProcessScoring(IstReportViewModel vm)
        {
            int age = CalculateAge(vm.BirthDate);
            vm.Age = age;
            string ageKey = Math.Clamp(age, 14, 18).ToString();

            var subTestMapping = new (string Code, string JsonKey, string Name, string Aspect)[]
            {
                ("SE", "IST_SubTest_01", "Membuat Penilaian", "Kemampuan Membuat Pertimbangan dan Keputusan"),
                ("WA", "IST_SubTest_02", "Pemahaman Berbahasa", "Kemampuan Verbal"),
                ("AN", "IST_SubTest_03", "Berpikir Analogi", "Kemampuan Analogi dan Hubungan Logis"),
                ("GE", "IST_SubTest_04", "Berpikir Abstraksi Verbal", "Kemampuan Abstraksi Verbal"),
                ("RA", "IST_SubTest_05", "Hitung Praktis", "Kemampuan Numerik Praktis"),
                ("ZR", "IST_SubTest_06", "Memprediksi Kejadian", "Kemampuan Penalaran Numerik"),
                ("FA", "IST_SubTest_07", "Menstrukturkan Pemikiran", "Kemampuan Analisis Visual"),
                ("WU", "IST_SubTest_08", "Berpikir Abstrak Spasial", "Kemampuan Spasial"),
                ("ME", "IST_SubTest_09", "Memori", "Kemampuan Memori")
            };

            int totalSwSum = 0;
            int countValidSubtests = 0;

            foreach (var item in subTestMapping)
            {
                int rw = 0;
                var userAns = vm.CleanedAnswers.GetValueOrDefault(item.JsonKey);

                // Subtest 04 (GE) dikosongkan/diberi default karena penilaian kualitatif
                if (item.Code != "GE" && _answerKeys.TryGetProperty(item.JsonKey, out var keyObj) && userAns != null)
                {
                    foreach (var prop in keyObj.EnumerateObject())
                    {
                        if (userAns.TryGetValue(prop.Name, out var userVal) &&
                            string.Equals(userVal.Trim(), prop.Value.GetString()?.Trim(), StringComparison.OrdinalIgnoreCase))
                        {
                            rw++;
                        }
                    }
                }

                int sw = ConvertRwToSw(ageKey, item.Code, rw);
                totalSwSum += sw;
                countValidSubtests++;

                var (category, text) = GetCategoryAndNarrative(item.Code, sw);

                vm.SubTestResults.Add(new IstSubTestReportItem
                {
                    SubTestCode = item.Code,
                    SubTestName = item.Name,
                    AspectName = item.Aspect,
                    RawScore = rw,
                    StandardScore = sw,
                    Category = category,
                    InterpretationText = text
                });
            }

            vm.GesamtStandardScore = countValidSubtests > 0 ? (int)Math.Round((double)totalSwSum / countValidSubtests) : 100;
            vm.CalculatedIQ = ConvertSwToIQ(vm.GesamtStandardScore);

            var (iqClass, iqLevel) = GetIQLeveling(vm.CalculatedIQ);
            vm.IQClassification = iqClass;
            vm.IQIntelligenceLevel = iqLevel;
        }

        private int ConvertRwToSw(string ageKey, string code, int rw)
        {
            try
            {
                var subObj = _ageNorms.GetProperty(ageKey).GetProperty(code);
                string rwKey = Math.Clamp(rw, 0, 20).ToString();
                return subObj.GetProperty(rwKey).GetInt32();
            }
            catch { return 100; }
        }

        private int ConvertSwToIQ(int avgSw)
        {
            try
            {
                string swKey = avgSw.ToString();
                if (_iqConversion.TryGetProperty(swKey, out var valObj))
                {
                    return valObj.GetProperty("iq").GetInt32();
                }
            }
            catch { }
            return avgSw;
        }

        private (string Category, string Narrative) GetCategoryAndNarrative(string code, int sw)
        {
            string cat = sw switch
            {
                < 80 => "Kurang Sekali",
                <= 89 => "Kurang",
                <= 109 => "Rata-rata",
                <= 119 => "Baik",
                _ => "Baik Sekali"
            };

            string text = (code, cat) switch
            {
                ("SE", "Kurang Sekali") => "Mengalami kesulitan memahami situasi dan menentukan keputusan yang tepat dalam berbagai kondisi.",
                ("SE", "Kurang") => "Kemampuan membuat pertimbangan masih di bawah rata-rata sehingga memerlukan bantuan dalam mengevaluasi suatu situasi.",
                ("SE", "Rata-rata") => "Mampu memahami situasi dan membuat pertimbangan yang sesuai dalam kondisi umum.",
                ("SE", "Baik") => "Mampu menilai situasi dengan baik dan membuat keputusan yang cukup tepat berdasarkan informasi yang tersedia.",
                ("SE", "Baik Sekali") => "Mampu memahami situasi secara komprehensif serta membuat keputusan yang matang dan realistis.",

                ("WA", "Kurang Sekali") => "Mengalami kesulitan memahami makna kata dan informasi berbasis bahasa.",
                ("WA", "Kurang") => "Kemampuan memahami serta mengolah informasi verbal masih di bawah rata-rata.",
                ("WA", "Rata-rata") => "Mampu memahami dan mengolah informasi verbal sesuai tuntutan umum.",
                ("WA", "Baik") => "Mampu memahami makna kata dan konsep serta mengolah informasi verbal dengan baik.",
                ("WA", "Baik Sekali") => "Memiliki kemampuan verbal yang sangat baik, mampu memahami dan mengolah informasi bahasa yang kompleks secara efektif.",

                _ => $"Kemampuan {code} Anda berada pada kategori {cat} dengan Nilai Baku {sw}."
            };

            return (cat, text);
        }

        private (string Classification, string Level) GetIQLeveling(int iq)
        {
            return iq switch
            {
                >= 136 => ("Baik Sekali", "Sangat Cerdas"),
                >= 120 => ("Baik", "Cerdas"),
                >= 110 => ("Baik", "Di atas Rata-rata"),
                >= 95 => ("Cukup", "Rata-Rata"),
                >= 85 => ("Kurang", "Di bawah Rata-rata"),
                _ => ("Kurang Sekali", "Perbatasan")
            };
        }

        private int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}