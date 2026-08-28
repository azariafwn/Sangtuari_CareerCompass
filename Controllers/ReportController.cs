using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SangtuariCareerCompass.Data;
using SangtuariCareerCompass.Models;
using SangtuariCareerCompass.Services.Scoring;
using SangtuariCareerCompass.ViewModels;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace SangtuariCareerCompass.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IstScoringEngine _istEngine;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
            _istEngine = new IstScoringEngine();
        }

        [HttpGet]
        public async Task<IActionResult> IstJudgment(Guid userAssessmentId)
        {
            var vm = await IstReportViewModel.BuildFromDatabaseAsync(_context, userAssessmentId);
            if (vm == null) return NotFound("Data tidak ditemukan.");

            if (vm.IsJudged) return RedirectToAction("IstResult", new { userAssessmentId });

            return View("~/Views/Report/IstJudgment.cshtml", vm);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitIstJudgment([FromBody] SubmitIstJudgmentDto dto)
        {
            var vm = await IstReportViewModel.BuildFromDatabaseAsync(_context, dto.UserAssessmentId);
            if (vm == null) return BadRequest("Data tidak valid.");

            vm.GeRawScore = dto.GeRawScore; // Masukkan nilai inputan psikolog

            // Proses seluruh skor IST termasuk GE
            var engine = new Services.Scoring.IstScoringEngine();
            engine.ProcessScoring(vm);

            // Simpan ke database
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

        //[HttpGet]
        //public async Task<IActionResult> IstResult(Guid userAssessmentId)
        //{
        //    var vm = await IstReportViewModel.BuildFromDatabaseAsync(_context, userAssessmentId);
        //    if (vm == null || !vm.IsJudged) return RedirectToAction("IstJudgment", new { userAssessmentId });

        //    // Populate IQ Intelligence Level for display (since it's dynamically generated)
        //    var engine = new Services.Scoring.IstScoringEngine();
        //    engine.ProcessScoring(vm); // Safe rerun just to populate volatile narrative fields if needed

        //    return View("~/Views/Report/IstResult.cshtml", vm);
        //}

        //public async Task<IActionResult> VarkResult(Guid userAssessmentId)
        //{
        //    if (userAssessmentId == Guid.Empty) return RedirectToAction("Index", "Assessment");

        //    // 1. LINQ Fetch via ViewModel Builder
        //    var reportVm = await VarkReportViewModel.BuildFromDatabaseAsync(_context, userAssessmentId);
        //    if (reportVm == null) return NotFound("Data asesmen VARK tidak ditemukan.");

        //    // 2. Pure Scoring Engine Execution
        //    var varkEngine = new VarkScoringEngine();
        //    varkEngine.ProcessScoring(reportVm);

        //    // 3. Simpan Hasil Akhir ke Tabel UserTestResults
        //    var resultEntity = new UserTestResult
        //    {
        //        UserAssessmentId = userAssessmentId,
        //        TestCategory = "VARK",
        //        OverallScore = reportVm.CategoryScores.Max(c => c.Score),
        //        Classification = reportVm.DominantCategoryText,
        //        ResultDetails = JsonDocument.Parse(JsonSerializer.Serialize(reportVm.CategoryScores))
        //    };

        //    _context.UserTestResults.Add(resultEntity);
        //    await _context.SaveChangesAsync();

        //    return View("~/Views/Report/VarkResult.cshtml", reportVm);
        //}
        
        //public async Task<IActionResult> SdsResult(Guid userAssessmentId)
        //{
        //    if (userAssessmentId == Guid.Empty) return RedirectToAction("Index", "Assessment");

        //    // 1. Eksekusi LINQ Fetch di ViewModel Builder
        //    var reportVm = await SdsHollandReportViewModel.BuildFromDatabaseAsync(_context, userAssessmentId);
        //    if (reportVm == null) return NotFound("Data asesmen SDS Holland tidak ditemukan.");

        //    // 2. Pure Scoring Engine Execution
        //    var sdsEngine = new SdsHollandScoringEngine();
        //    sdsEngine.ProcessScoring(reportVm);

        //    // 3. Simpan Ringkasan Hasil ke Tabel Database UserTestResults
        //    var existingResult = await _context.UserTestResults
        //        .FirstOrDefaultAsync(r => r.UserAssessmentId == userAssessmentId && r.TestCategory == "SDS_Holland");

        //    if (existingResult == null)
        //    {
        //        var resultEntity = new UserTestResult
        //        {
        //            UserAssessmentId = userAssessmentId,
        //            TestCategory = "SDS_Holland",
        //            OverallScore = reportVm.TotalElevationScore,
        //            Classification = reportVm.SummaryCode,
        //            ResultDetails = JsonDocument.Parse(JsonSerializer.Serialize(new
        //            {
        //                SummaryCode = reportVm.SummaryCode,
        //                ProfileElevation = reportVm.ProfileElevationCategory,
        //                Consistency = reportVm.ConsistencyDegree,
        //                Scores = reportVm.RiasecScores
        //            }))
        //        };
        //        _context.UserTestResults.Add(resultEntity);
        //        await _context.SaveChangesAsync();
        //    }

        //    return View("~/Views/Report/SdsResult.cshtml", reportVm);
        //}
        [HttpGet]
        public async Task<IActionResult> PapiJudgment(Guid userAssessmentId)
        {
            var vm = await PapiReportViewModel.BuildFromDatabaseAsync(_context, userAssessmentId);
            if (vm == null) return NotFound("Data tidak ditemukan.");

            if (vm.IsJudged) return RedirectToAction("PapiResult", new { userAssessmentId });

            var engine = new PapiScoringEngine();
            engine.ProcessRawScoring(vm);

            return View("~/Views/Report/PapiJudgment.cshtml", vm);
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

        //[HttpGet]
        //public async Task<IActionResult> PapiResult(Guid userAssessmentId)
        //{
        //    var vm = await PapiReportViewModel.BuildFromDatabaseAsync(_context, userAssessmentId);
        //    if (vm == null || !vm.IsJudged) return RedirectToAction("PapiJudgment", new { userAssessmentId });

        //    return View("~/Views/Report/PapiResult.cshtml", vm);
        //}

        [HttpGet]
        public async Task<IActionResult> FinalReport(Guid userAssessmentId)
        {
            if (userAssessmentId == Guid.Empty) return RedirectToAction("Index", "Assessment");

            var reportVm = await FinalReportViewModel.BuildFromDatabaseAsync(_context, userAssessmentId);
            if (reportVm == null) return NotFound("Data asesmen tidak ditemukan.");

            return View("~/Views/Report/FinalReport.cshtml", reportVm);
        }
    }
}