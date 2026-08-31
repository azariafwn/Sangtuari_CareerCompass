using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SangtuariCareerCompass.Data;
using SangtuariCareerCompass.Models;
using SangtuariCareerCompass.Models.DTOs;
using SangtuariCareerCompass.Services.Scoring;
using SangtuariCareerCompass.ViewModels;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Security.Claims;
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // --- LOGIKA DASHBOARD STATISTIK ---
            var totalQueue = await _context.UserAssessments
                .Where(u => u.AssessmentType == "Discovery")
                .CountAsync();

            ViewBag.TotalQueue = totalQueue;
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Queue()
        {
            // --- LOGIKA TABEL ANTREAN (Pindahan dari Index sebelumnya) ---
            var assessments = await _context.UserAssessments
                .AsNoTracking()
                .Where(u => u.AssessmentType == "Discovery")
                .OrderByDescending(u => u.Id)
                .ToListAsync();

            var queueList = new List<dynamic>();

            foreach (var user in assessments)
            {
                bool isIstJudged = await _context.UserTestResults.AnyAsync(r => r.UserAssessmentId == user.Id && r.TestCategory == "IST");
                bool isPapiJudged = await _context.UserTestResults.AnyAsync(r => r.UserAssessmentId == user.Id && r.TestCategory == "PAPI_Kostick");

                queueList.Add(new
                {
                    user.Id,
                    user.FullName,
                    user.SchoolName,
                    IsIstJudged = isIstJudged,
                    IsPapiJudged = isPapiJudged
                });
            }

            return View(queueList);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // 1. Cari user di database
            var user = await _context.PsychologistUsers.FirstOrDefaultAsync(u => u.Email == model.Email);

            // 2. Verifikasi hash BCrypt
            if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                // 3. Buat "KTP" (Claims) untuk user ini
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // 4. Terbitkan Cookie Login
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index");
            }

            // Jika gagal
            ModelState.AddModelError(string.Empty, "Email atau password salah.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
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