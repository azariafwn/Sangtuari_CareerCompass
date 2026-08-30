using Microsoft.AspNetCore.Mvc;
using SangtuariCareerCompass.Data;
using SangtuariCareerCompass.Models;
using SangtuariCareerCompass.Models.DTOs;
using SangtuariCareerCompass.Services.Scoring;
using SangtuariCareerCompass.ViewModels;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace SangtuariCareerCompass.Controllers
{
    // Nantinya kita bisa tambahkan [Authorize(Roles = "Psychologist")] di sini
    public class PsychologistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PsychologistController(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- Dasbor Placeholder ---
        [HttpGet]
        public IActionResult Index()
        {
            return View(); // Nanti kita buat dasbor antrean penilaian di sini
        }

        // --- IST JUDGMENT ---
        [HttpGet]
        public async Task<IActionResult> IstJudgment(Guid userAssessmentId)
        {
            var vm = await IstReportViewModel.BuildFromDatabaseAsync(_context, userAssessmentId);
            if (vm == null) return NotFound("Data tidak ditemukan.");

            //if (vm.IsJudged) return RedirectToAction("FinalReportSMA", "Report", new { userAssessmentId });

            // Redirect ke instruksi PAPI Kostick jika sudah dinilai
            if (vm.IsJudged) return RedirectToAction("Instruction", "Quiz", new { testKey = "PapiKostick", userAssessmentId });

            // Sesuaikan path ke folder baru
            return View("~/Views/Psychologist/Report/IstJudgment.cshtml", vm);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitIstJudgment([FromBody] SubmitIstJudgmentDto dto)
        {
            var vm = await IstReportViewModel.BuildFromDatabaseAsync(_context, dto.UserAssessmentId);
            if (vm == null) return BadRequest("Data tidak valid.");

            vm.GeRawScore = dto.GeRawScore;

            var engine = new IstScoringEngine();
            engine.ProcessScoring(vm);

            var resultEntity = new UserTestResult
            {
                UserAssessmentId = dto.UserAssessmentId,
                TestCategory = "IST",
                OverallScore = vm.CalculatedIQ,
                Classification = vm.IQClassification,
                ResultDetails = JsonDocument.Parse(JsonSerializer.Serialize(vm.SubTestResults))
            };

            _context.UserTestResults.Add(resultEntity);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }

        // --- PAPI KOSTICK JUDGMENT ---
        [HttpGet]
        public async Task<IActionResult> PapiJudgment(Guid userAssessmentId)
        {
            var vm = await PapiReportViewModel.BuildFromDatabaseAsync(_context, userAssessmentId);
            if (vm == null) return NotFound("Data tidak ditemukan.");

            //if (vm.IsJudged) return RedirectToAction("FinalReportSMA", "Report", new { userAssessmentId });
            // Redirect ke instruksi VARK jika sudah dinilai
            if (vm.IsJudged) return RedirectToAction("Instruction", "Quiz", new { testKey = "VARK", userAssessmentId });

            var engine = new PapiScoringEngine();
            engine.ProcessRawScoring(vm);

            // Sesuaikan path ke folder baru
            return View("~/Views/Psychologist/Report/PapiJudgment.cshtml", vm);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitPapiJudgment([FromBody] SubmitPapiJudgmentDto dto)
        {
            var aspectGroups = JsonSerializer.Deserialize<List<PapiAspectGroup>>(dto.JudgmentsJson);

            var resultEntity = new UserTestResult
            {
                UserAssessmentId = dto.UserAssessmentId,
                TestCategory = "PAPI_Kostick",
                OverallScore = 0,
                Classification = "Manual Judged",
                ResultDetails = JsonDocument.Parse(JsonSerializer.Serialize(aspectGroups))
            };

            _context.UserTestResults.Add(resultEntity);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }
    }
}