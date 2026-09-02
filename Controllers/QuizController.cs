using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SangtuariCareerCompass.Data;
using SangtuariCareerCompass.Models;
using SangtuariCareerCompass.Models.DTOs;
using SangtuariCareerCompass.Services;
using SangtuariCareerCompass.Services.Scoring;
using SangtuariCareerCompass.ViewModels;
using System;
using System.IO;
using System.Text.Json;

namespace SangtuariCareerCompass.Controllers
{
    public class QuizController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public QuizController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // EAS 1 - Sudah Aktif
        public IActionResult Eas1(Guid userAssessmentId)
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Quiz", "EAS", "eas1Question.json");
            var questionsJson = System.IO.File.Exists(jsonPath)
                ? System.IO.File.ReadAllText(jsonPath)
                : "[]";

            ViewBag.UserAssessmentId = userAssessmentId;
            ViewBag.QuestionsJson = questionsJson;

            return View("~/Views/Quiz/SMP/EAS/Eas1.cshtml");
        }

        // EAS 2 - Placeholder
        public IActionResult Eas2(Guid userAssessmentId)
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Quiz", "EAS", "eas2Question.json");
            var questionsJson = System.IO.File.Exists(jsonPath)
                ? System.IO.File.ReadAllText(jsonPath)
                : "[]";

            ViewBag.UserAssessmentId = userAssessmentId;
            ViewBag.QuestionsJson = questionsJson;

            return View("~/Views/Quiz/SMP/EAS/Eas2.cshtml");
        }

        // EAS 5 - Placeholder
        public IActionResult Eas5(Guid userAssessmentId)
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Quiz", "EAS", "eas5Question.json");
            var questionsJson = System.IO.File.Exists(jsonPath)
                ? System.IO.File.ReadAllText(jsonPath)
                : "[]";

            ViewBag.UserAssessmentId = userAssessmentId;
            ViewBag.QuestionsJson = questionsJson;

            return View("~/Views/Quiz/SMP/EAS/Eas5.cshtml");
        }

        // CFIT - Placeholder
        // CFIT Sub-Test 1 (Series - 3 Menit)
        public IActionResult CfitSubTest1(Guid userAssessmentId)
        {
            if (userAssessmentId == Guid.Empty) return RedirectToAction("Index", "Assessment");

            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Quiz", "CFIT", "cfitSubTest1.json");
            ViewBag.UserAssessmentId = userAssessmentId;
            ViewBag.QuestionsJson = System.IO.File.Exists(jsonPath) ? System.IO.File.ReadAllText(jsonPath) : "[]";

            return View("~/Views/Quiz/SMP/CFIT/SubTest1.cshtml");
        }

        // CFIT Sub-Test 2 (Classification - 4 Menit)
        public IActionResult CfitSubTest2(Guid userAssessmentId)
        {
            if (userAssessmentId == Guid.Empty) return RedirectToAction("Index", "Assessment");

            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Quiz", "CFIT", "cfitSubTest2.json");
            ViewBag.UserAssessmentId = userAssessmentId;
            ViewBag.QuestionsJson = System.IO.File.Exists(jsonPath) ? System.IO.File.ReadAllText(jsonPath) : "[]";

            return View("~/Views/Quiz/SMP/CFIT/SubTest2.cshtml");
        }

        // CFIT Sub-Test 3 (Matrices - 3 Menit)
        public IActionResult CfitSubTest3(Guid userAssessmentId)
        {
            if (userAssessmentId == Guid.Empty) return RedirectToAction("Index", "Assessment");

            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Quiz", "CFIT", "cfitSubTest3.json");
            ViewBag.UserAssessmentId = userAssessmentId;
            ViewBag.QuestionsJson = System.IO.File.Exists(jsonPath) ? System.IO.File.ReadAllText(jsonPath) : "[]";

            return View("~/Views/Quiz/SMP/CFIT/SubTest3.cshtml");
        }

        // CFIT Sub-Test 4 (Conditions/Topology - 2.5 Menit)
        public IActionResult CfitSubTest4(Guid userAssessmentId)
        {
            if (userAssessmentId == Guid.Empty) return RedirectToAction("Index", "Assessment");

            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Quiz", "CFIT", "cfitSubTest4.json");
            ViewBag.UserAssessmentId = userAssessmentId;
            ViewBag.QuestionsJson = System.IO.File.Exists(jsonPath) ? System.IO.File.ReadAllText(jsonPath) : "[]";

            return View("~/Views/Quiz/SMP/CFIT/SubTest4.cshtml");
        }

        // VARK - Placeholder
        public IActionResult Vark(Guid userAssessmentId)
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Quiz", "VARK", "varkQuestion.json");
            var questionsJson = System.IO.File.Exists(jsonPath)
                ? System.IO.File.ReadAllText(jsonPath)
                : "[]";

            ViewBag.UserAssessmentId = userAssessmentId;
            ViewBag.QuestionsJson = questionsJson;

            return View("~/Views/Quiz/SMP/VARK/Index.cshtml");
        }

        // SDS Holland - Placeholder
        public IActionResult SdsHolland(Guid userAssessmentId)
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Quiz", "Holland", "hollandQuestion.json");
            var questionsJson = System.IO.File.Exists(jsonPath)
                ? System.IO.File.ReadAllText(jsonPath)
                : "[]";

            ViewBag.UserAssessmentId = userAssessmentId;
            ViewBag.QuestionsJson = questionsJson;

            return View("~/Views/Quiz/SMP/Holland/Index.cshtml");
        }

        // IST Sub-Test 01 (SE) - Aktif
        public IActionResult IstSubTest1(Guid userAssessmentId)
        {

            if (userAssessmentId == Guid.Empty)
            {
                // Fail-safe jika ID tidak terbawa di URL
                return RedirectToAction("Index", "Assessment");
            }

            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Quiz", "IST", "istSubTest1.json");
            var questionsJson = System.IO.File.Exists(jsonPath)
                ? System.IO.File.ReadAllText(jsonPath)
                : "[]";

            ViewBag.UserAssessmentId = userAssessmentId;
            ViewBag.QuestionsJson = questionsJson;

            return View("~/Views/Quiz/SMA/IST/SubTest1.cshtml");
        }

        // IST Sub-Test 02 s.d. 09 - Placeholder Coming Soon
        public IActionResult IstSubTest2(Guid userAssessmentId)
        {

            if (userAssessmentId == Guid.Empty)
            {
                // Fail-safe jika ID tidak terbawa di URL
                return RedirectToAction("Index", "Assessment");
            }

            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Quiz", "IST", "istSubTest2.json");
            var questionsJson = System.IO.File.Exists(jsonPath)
                ? System.IO.File.ReadAllText(jsonPath)
                : "[]";

            ViewBag.UserAssessmentId = userAssessmentId;
            ViewBag.QuestionsJson = questionsJson;

            return View("~/Views/Quiz/SMA/IST/SubTest2.cshtml");
        }
        public IActionResult IstSubTest3(Guid userAssessmentId)
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Quiz", "IST", "istSubTest3.json");
            var questionsJson = System.IO.File.Exists(jsonPath)
                ? System.IO.File.ReadAllText(jsonPath)
                : "[]";

            ViewBag.UserAssessmentId = userAssessmentId;
            ViewBag.QuestionsJson = questionsJson;

            return View("~/Views/Quiz/SMA/IST/SubTest3.cshtml");
        }

        public IActionResult IstSubTest4(Guid userAssessmentId)
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Quiz", "IST", "istSubTest4.json");
            var questionsJson = System.IO.File.Exists(jsonPath)
                ? System.IO.File.ReadAllText(jsonPath)
                : "[]";

            ViewBag.UserAssessmentId = userAssessmentId;
            ViewBag.QuestionsJson = questionsJson;

            return View("~/Views/Quiz/SMA/IST/SubTest4.cshtml");
        }

        public IActionResult IstSubTest5(Guid userAssessmentId)
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Quiz", "IST", "istSubTest5.json");
            var questionsJson = System.IO.File.Exists(jsonPath)
                ? System.IO.File.ReadAllText(jsonPath)
                : "[]";

            ViewBag.UserAssessmentId = userAssessmentId;
            ViewBag.QuestionsJson = questionsJson;

            return View("~/Views/Quiz/SMA/IST/SubTest5.cshtml");
        }

        public IActionResult IstSubTest6(Guid userAssessmentId)
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Quiz", "IST", "istSubTest6.json");
            var questionsJson = System.IO.File.Exists(jsonPath)
                ? System.IO.File.ReadAllText(jsonPath)
                : "[]";

            ViewBag.UserAssessmentId = userAssessmentId;
            ViewBag.QuestionsJson = questionsJson;

            return View("~/Views/Quiz/SMA/IST/SubTest6.cshtml");
        }

        public IActionResult IstSubTest7(Guid userAssessmentId)
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Quiz", "IST", "istSubTest7.json");
            var questionsJson = System.IO.File.Exists(jsonPath)
                ? System.IO.File.ReadAllText(jsonPath)
                : "[]";

            ViewBag.UserAssessmentId = userAssessmentId;
            ViewBag.QuestionsJson = questionsJson;

            return View("~/Views/Quiz/SMA/IST/SubTest7.cshtml");
        }

        public IActionResult IstSubTest8(Guid userAssessmentId)
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Quiz", "IST", "istSubTest8.json");
            var questionsJson = System.IO.File.Exists(jsonPath)
                ? System.IO.File.ReadAllText(jsonPath)
                : "[]";

            ViewBag.UserAssessmentId = userAssessmentId;
            ViewBag.QuestionsJson = questionsJson;

            return View("~/Views/Quiz/SMA/IST/SubTest8.cshtml");
        }

        public IActionResult IstSubTest9(Guid userAssessmentId)
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Quiz", "IST", "istSubTest9.json");
            var questionsJson = System.IO.File.Exists(jsonPath)
                ? System.IO.File.ReadAllText(jsonPath)
                : "{}";

            ViewBag.UserAssessmentId = userAssessmentId;
            ViewBag.QuestionsJson = questionsJson;

            return View("~/Views/Quiz/SMA/IST/SubTest9.cshtml");
        }

        public IActionResult PapiKostick(Guid userAssessmentId)
        {
            if (userAssessmentId == Guid.Empty)
            {
                return RedirectToAction("Index", "Assessment");
            }

            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Quiz", "PapiKostick", "papiKostick.json");
            var questionsJson = System.IO.File.Exists(jsonPath)
                ? System.IO.File.ReadAllText(jsonPath)
                : "[]";

            ViewBag.UserAssessmentId = userAssessmentId;
            ViewBag.QuestionsJson = questionsJson;

            return View("~/Views/Quiz/SMA/PapiKostick/Index.cshtml");
        }

        public IActionResult Instruction(string testKey, Guid userAssessmentId)
        {
            if (userAssessmentId == Guid.Empty || string.IsNullOrEmpty(testKey))
            {
                return RedirectToAction("Index", "Assessment");
            }

            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Quiz", "instructions.json");
            var instructionsJson = System.IO.File.Exists(jsonPath)
                ? System.IO.File.ReadAllText(jsonPath)
                : "{}";

            ViewBag.TestKey = testKey;
            ViewBag.UserAssessmentId = userAssessmentId;
            ViewBag.InstructionsJson = instructionsJson;

            return View("~/Views/Quiz/Instruction.cshtml");
        }

        // =======================================================
        // POST METHOD: IN-MEMORY SCORING 
        // =======================================================
        [HttpPost]
        [Route("/Assessment/SubmitSubTest")]
        public async Task<IActionResult> SubmitSubTest([FromBody] SubTestSubmissionDto dto)
        {
            if (dto == null || dto.UserAssessmentId == Guid.Empty)
                return BadRequest(new { success = false, message = "Data tidak valid." });

            try
            {
                // 1. Simpan Jawaban Mentah (Selalu dilakukan untuk semua tes)
                var userAnswer = new UserAnswer
                {
                    UserAssessmentId = dto.UserAssessmentId,
                    SubTestName = dto.SubTestName,
                    Answers = JsonDocument.Parse(dto.Answers.GetRawText()) // Convert dari JsonElement
                };
                _context.UserAnswers.Add(userAnswer);
                await _context.SaveChangesAsync();

                // 2. Kalkulasi Auto-Scoring Khusus VARK & Holland (Mengambil dari Payload)
                if (dto.SubTestName.Equals("VARK", StringComparison.OrdinalIgnoreCase))
                {
                    var varkScores = new List<VarkScoreItem>();

                    if (dto.Answers.TryGetProperty("Scores", out var scoresObj))
                    {
                        int total = 0;
                        foreach (var prop in scoresObj.EnumerateObject()) total += prop.Value.GetInt32();

                        foreach (var prop in scoresObj.EnumerateObject())
                        {
                            string code = prop.Name.ToUpper();
                            string name = code switch { "V" => "Visual", "A" => "Aural/auditori", "R" => "Read/Write", "K" => "Kinesthetic", _ => code };
                            int score = prop.Value.GetInt32();

                            varkScores.Add(new VarkScoreItem
                            {
                                CategoryCode = code,
                                CategoryName = name,
                                Score = score,
                                Percentage = total > 0 ? Math.Round((double)score / total * 100, 1) : 0
                            });
                        }
                    }

                    int maxScore = varkScores.Any() ? varkScores.Max(x => x.Score) : 0;
                    var dominantText = string.Join(" and ", varkScores.Where(x => x.Score == maxScore).Select(x => x.CategoryName));

                    var resultEntity = new UserTestResult
                    {
                        UserAssessmentId = dto.UserAssessmentId,
                        TestCategory = "VARK",
                        OverallScore = maxScore,
                        Classification = string.IsNullOrEmpty(dominantText) ? "Belum Terkalkulasi" : dominantText,
                        ResultDetails = JsonDocument.Parse(JsonSerializer.Serialize(varkScores))
                    };

                    _context.UserTestResults.Add(resultEntity);
                    await _context.SaveChangesAsync();
                }
                
                else if (dto.SubTestName.Equals("SDS_Holland", StringComparison.OrdinalIgnoreCase))
                {
                    var sdsVm = new SdsHollandReportViewModel { UserAssessmentId = dto.UserAssessmentId };

                    if (dto.Answers.TryGetProperty("Scores", out var scoresObj))
                    {
                        foreach (var prop in scoresObj.EnumerateObject())
                        {
                            sdsVm.CalculatedScores[prop.Name.ToUpper()] = prop.Value.GetInt32();
                        }
                    }

                    // Engine murni dipanggil HANYA untuk menghitung Elevasi dan Konsistensi
                    var engine = new SdsHollandScoringEngine();
                    engine.ProcessScoring(sdsVm);

                    var resultEntity = new UserTestResult
                    {
                        UserAssessmentId = dto.UserAssessmentId,
                        TestCategory = "SDS_Holland",
                        OverallScore = sdsVm.TotalElevationScore,
                        Classification = sdsVm.SummaryCode ?? "XXX",
                        ResultDetails = JsonDocument.Parse(JsonSerializer.Serialize(new
                        {
                            SummaryCode = sdsVm.SummaryCode,
                            ProfileElevation = sdsVm.ProfileElevationCategory,
                            Consistency = sdsVm.ConsistencyDegree,
                            Scores = sdsVm.RiasecScores
                        }))
                    };

                    _context.UserTestResults.Add(resultEntity);
                    await _context.SaveChangesAsync();                    

                    return Ok(new
                    {
                        success = true,
                        nextUrl = $"/Quiz/Completed?userAssessmentId={dto.UserAssessmentId}"
                    });
                }

                // Ubah kondisi if untuk menangkap "CFIT_SubTest_04"
                else if (dto.SubTestName.Equals("CFIT_SubTest_04", StringComparison.OrdinalIgnoreCase) ||
                         dto.SubTestName.Equals("CFIT_SubTest_4", StringComparison.OrdinalIgnoreCase))
                {
                    var cfitVm = await CfitReportViewModel.BuildFromDatabaseAsync(_context, dto.UserAssessmentId)
                                 ?? new CfitReportViewModel { UserAssessmentId = dto.UserAssessmentId };

                    var sub4Dict = new Dictionary<string, string>();
                    if (dto.Answers.ValueKind == JsonValueKind.Object)
                    {
                        foreach (var prop in dto.Answers.EnumerateObject())
                        {
                            sub4Dict[prop.Name] = prop.Value.GetString()?.ToLower() ?? "";
                        }
                    }

                    // Pastikan kita menyimpan di ViewModel dengan key format 04 agar seragam dengan DB
                    cfitVm.UserAnswers[dto.SubTestName] = sub4Dict;

                    var engine = new CfitScoringEngine();
                    engine.ProcessScoring(cfitVm);

                    var resultEntity = new UserTestResult
                    {
                        UserAssessmentId = dto.UserAssessmentId,
                        TestCategory = "CFIT",
                        OverallScore = cfitVm.CalculatedIQ,
                        Classification = cfitVm.IQClassification,
                        ResultDetails = JsonDocument.Parse(JsonSerializer.Serialize(new
                        {
                            TotalRawScore = cfitVm.TotalRawScore,
                            SubTestScores = cfitVm.SubTestScores,
                            AgeInMonths = cfitVm.AgeInMonths
                        }))
                    };

                    _context.UserTestResults.Add(resultEntity);
                    await _context.SaveChangesAsync();
                }

                else if (dto.SubTestName.Equals("EAS-5", StringComparison.OrdinalIgnoreCase))
                {
                    var easVm = await EasReportViewModel.BuildFromDatabaseAsync(_context, dto.UserAssessmentId)
                                 ?? new EasReportViewModel { UserAssessmentId = dto.UserAssessmentId };

                    var sub5Dict = new Dictionary<string, string>();
                    if (dto.Answers.ValueKind == JsonValueKind.Object)
                    {
                        foreach (var prop in dto.Answers.EnumerateObject())
                        {
                            sub5Dict[prop.Name] = prop.Value.GetString()?.ToLower() ?? "";
                        }
                    }

                    // Gunakan key format baru agar seragam
                    easVm.UserAnswers["EAS-5"] = sub5Dict;

                    var engine = new EasScoringEngine();
                    engine.ProcessScoring(easVm);

                    var resultEntity = new UserTestResult
                    {
                        UserAssessmentId = dto.UserAssessmentId,
                        TestCategory = "EAS",
                        OverallScore = 0,
                        Classification = "Komposit EAS",
                        ResultDetails = JsonDocument.Parse(JsonSerializer.Serialize(easVm.SubTestScores))
                    };

                    _context.UserTestResults.Add(resultEntity);
                    await _context.SaveChangesAsync();
                }

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Scoring {dto.SubTestName}: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Terjadi kesalahan internal saat skoring." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Completed(Guid userAssessmentId)
        {
            if (userAssessmentId == Guid.Empty)
                return RedirectToAction("Index", "Assessment");

            var assessment = await _context.UserAssessments
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userAssessmentId);

            if (assessment == null)
                return NotFound();

            // LOGIKA AUTO-EMAIL KHUSUS SMP (Exploration)
            if (assessment.AssessmentType == "Exploration")
            {
                // Generate URL lengkap berdasarkan host server saat ini
                var request = HttpContext.Request;
                var baseUrl = $"{request.Scheme}://{request.Host}";
                var reportUrl = $"{baseUrl}/Report/FinalReportSMP?userAssessmentId={assessment.Id}";

                // Eksekusi Fire-and-Forget di background thread agar loading UI tidak terblokir
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _emailService.SendResultEmailAsync(
                            assessment.Email,
                            assessment.FullName,
                            reportUrl,
                            assessment.AssessmentType
                        );
                    }
                    catch (Exception ex)
                    {
                        // Logging jika email gagal (bisa diganti dengan ILogger ke depannya)
                        Console.WriteLine($"[Error] Gagal mengirim auto-email ke {assessment.Email}: {ex.Message}");
                    }
                });
            }

            return View(assessment);
        }
    }
}