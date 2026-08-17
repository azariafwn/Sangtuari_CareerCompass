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

        public async Task<IActionResult> IstResult(Guid userAssessmentId)
        {
            if (userAssessmentId == Guid.Empty) return RedirectToAction("Index", "Assessment");

            // 1. Eksekusi query LINQ di ViewModel Builder
            var reportVm = await IstReportViewModel.BuildFromDatabaseAsync(_context, userAssessmentId);
            if (reportVm == null) return NotFound("Data asesmen tidak ditemukan.");

            // 2. Eksekusi Scoring Engine Domain Logic
            _istEngine.ProcessScoring(reportVm);

            // 3. Simpan Hasil ke Tabel Database "UserTestResults"
            var resultEntity = new UserTestResult
            {
                UserAssessmentId = userAssessmentId,
                TestCategory = "IST",
                OverallScore = reportVm.CalculatedIQ,
                Classification = reportVm.IQIntelligenceLevel,
                ResultDetails = JsonDocument.Parse(JsonSerializer.Serialize(reportVm.SubTestResults))
            };

            _context.UserTestResults.Add(resultEntity);
            await _context.SaveChangesAsync();

            return View("~/Views/Report/IstResult.cshtml", reportVm);
        }

        public async Task<IActionResult> VarkResult(Guid userAssessmentId)
        {
            if (userAssessmentId == Guid.Empty) return RedirectToAction("Index", "Assessment");

            // 1. LINQ Fetch via ViewModel Builder
            var reportVm = await VarkReportViewModel.BuildFromDatabaseAsync(_context, userAssessmentId);
            if (reportVm == null) return NotFound("Data asesmen VARK tidak ditemukan.");

            // 2. Pure Scoring Engine Execution
            var varkEngine = new VarkScoringEngine();
            varkEngine.ProcessScoring(reportVm);

            // 3. Simpan Hasil Akhir ke Tabel UserTestResults
            var resultEntity = new UserTestResult
            {
                UserAssessmentId = userAssessmentId,
                TestCategory = "VARK",
                OverallScore = reportVm.CategoryScores.Max(c => c.Score),
                Classification = reportVm.DominantCategoryText,
                ResultDetails = JsonDocument.Parse(JsonSerializer.Serialize(reportVm.CategoryScores))
            };

            _context.UserTestResults.Add(resultEntity);
            await _context.SaveChangesAsync();

            return View("~/Views/Report/VarkResult.cshtml", reportVm);
        }
        
        public async Task<IActionResult> SdsResult(Guid userAssessmentId)
        {
            if (userAssessmentId == Guid.Empty) return RedirectToAction("Index", "Assessment");

            // 1. Eksekusi LINQ Fetch di ViewModel Builder
            var reportVm = await SdsHollandReportViewModel.BuildFromDatabaseAsync(_context, userAssessmentId);
            if (reportVm == null) return NotFound("Data asesmen SDS Holland tidak ditemukan.");

            // 2. Pure Scoring Engine Execution
            var sdsEngine = new SdsHollandScoringEngine();
            sdsEngine.ProcessScoring(reportVm);

            // 3. Simpan Ringkasan Hasil ke Tabel Database UserTestResults
            var existingResult = await _context.UserTestResults
                .FirstOrDefaultAsync(r => r.UserAssessmentId == userAssessmentId && r.TestCategory == "SDS_Holland");

            if (existingResult == null)
            {
                var resultEntity = new UserTestResult
                {
                    UserAssessmentId = userAssessmentId,
                    TestCategory = "SDS_Holland",
                    OverallScore = reportVm.TotalElevationScore,
                    Classification = reportVm.SummaryCode,
                    ResultDetails = JsonDocument.Parse(JsonSerializer.Serialize(new
                    {
                        SummaryCode = reportVm.SummaryCode,
                        ProfileElevation = reportVm.ProfileElevationCategory,
                        Consistency = reportVm.ConsistencyDegree,
                        Scores = reportVm.RiasecScores
                    }))
                };
                _context.UserTestResults.Add(resultEntity);
                await _context.SaveChangesAsync();
            }

            return View("~/Views/Report/SdsResult.cshtml", reportVm);
        }
    }
}