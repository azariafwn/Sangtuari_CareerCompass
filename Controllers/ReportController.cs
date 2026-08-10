using Microsoft.AspNetCore.Mvc;
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
    }
}