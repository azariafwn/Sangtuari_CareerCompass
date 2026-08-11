using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using SangtuariCareerCompass.ViewModels;

namespace SangtuariCareerCompass.Services.Scoring
{
    public class VarkScoringEngine
    {
        private JsonElement _scoringMap;

        public VarkScoringEngine()
        {
            LoadScoringMap();
        }

        private void LoadScoringMap()
        {
            var mapPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Scoring", "VARK", "scoringMap.json");
            if (File.Exists(mapPath))
            {
                _scoringMap = JsonSerializer.Deserialize<JsonDocument>(File.ReadAllText(mapPath))!.RootElement;
            }
        }

        public void ProcessScoring(VarkReportViewModel vm)
        {
            int scoreV = 0, scoreA = 0, scoreR = 0, scoreK = 0;

            // Jika nilai Scores sudah dihitung saat simpan jawaban di DB
            if (vm.ExistingScores.Any())
            {
                scoreV = vm.ExistingScores.GetValueOrDefault("V", 0);
                scoreA = vm.ExistingScores.GetValueOrDefault("A", 0);
                scoreR = vm.ExistingScores.GetValueOrDefault("R", 0);
                scoreK = vm.ExistingScores.GetValueOrDefault("K", 0);
            }
            else
            {
                // Kalkulasi manual berdasarkan UserAnswers dan scoringMap.json
                foreach (var (qKey, selectedOptions) in vm.UserAnswers)
                {
                    string qNum = qKey.Replace("Q", "").Replace("q", "");
                    if (_scoringMap.ValueKind == JsonValueKind.Object && _scoringMap.TryGetProperty(qNum, out var qObj))
                    {
                        foreach (var opt in selectedOptions)
                        {
                            if (qObj.TryGetProperty(opt.ToLower(), out var catProp))
                            {
                                string category = catProp.GetString() ?? "";
                                switch (category.ToUpper())
                                {
                                    case "V": scoreV++; break;
                                    case "A": scoreA++; break;
                                    case "R": scoreR++; break;
                                    case "K": scoreK++; break;
                                }
                            }
                        }
                    }
                }
            }

            int totalScore = scoreV + scoreA + scoreR + scoreK;
            double total = totalScore > 0 ? (double)totalScore : 1.0;

            vm.CategoryScores = new List<VarkScoreItem>
            {
                new VarkScoreItem { CategoryCode = "V", CategoryName = "Visual", Score = scoreV, Percentage = Math.Round((scoreV / total) * 100, 1) },
                new VarkScoreItem { CategoryCode = "A", CategoryName = "Aural/auditori", Score = scoreA, Percentage = Math.Round((scoreA / total) * 100, 1) },
                new VarkScoreItem { CategoryCode = "R", CategoryName = "Read/Write", Score = scoreR, Percentage = Math.Round((scoreR / total) * 100, 1) },
                new VarkScoreItem { CategoryCode = "K", CategoryName = "Kinesthetic", Score = scoreK, Percentage = Math.Round((scoreK / total) * 100, 1) }
            };

            // Menentukan Preferensi Dominan
            int maxScore = vm.CategoryScores.Max(c => c.Score);
            var dominantItems = vm.CategoryScores.Where(c => c.Score == maxScore && maxScore > 0).ToList();

            vm.DominantCategories = dominantItems.Select(d => d.CategoryName).ToList();
            vm.DominantCategoryText = dominantItems.Any() ? string.Join(" dan ", vm.DominantCategories) : "Belum Terdeteksi";

            // Penyusunan Interpretasi & Rekomendasi
            var interpList = new List<string>();
            var methodList = new List<string>();

            foreach (var item in dominantItems)
            {
                switch (item.CategoryCode)
                {
                    case "V":
                        interpList.Add("representasi visual");
                        methodList.Add("pembelajaran yang terdapat diagram, grafik, skema, peta konsep, dan pemanfaatan warna untuk menandai kalimat atau kata penting");
                        break;
                    case "A":
                        interpList.Add("kegiatan diskusi dan mendengar");
                        methodList.Add("kegiatan diskusi, tanya jawab, penjelasan verbal, presentasi, menjelaskan kepada orang lain, atau mendengarkan rekaman");
                        break;
                    case "R":
                        interpList.Add("kegiatan baca tulis");
                        methodList.Add("membaca buku atau artikel, membuat ringkasan, dan mencatat");
                        break;
                    case "K":
                        interpList.Add("pengalaman langsung dan praktik");
                        methodList.Add("kegiatan praktik, eksperimen, demonstrasi, studi kasus, dan simulasi");
                        break;
                }
            }

            string interpJoined = string.Join(" serta ", interpList);
            string methodJoined = string.Join(", serta ", methodList);

            vm.InterpretationText = $"Preferensi belajar Ananda {vm.FullName} adalah {vm.DominantCategoryText.ToLower()}. Artinya, Ananda {vm.FullName} lebih mudah belajar dan menerima informasi jika melibatkan {interpJoined}.";
            vm.RecommendedStudyMethods = $"Metode belajar yang disarankan untuk Ananda {vm.FullName} adalah {methodJoined}.";
        }
    }
}