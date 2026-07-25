using Microsoft.AspNetCore.Mvc;
using SangtuariCareerCompass.Data;
using SangtuariCareerCompass.Models;
using SangtuariCareerCompass.Models.DTOs;
using System.Text.Json;

namespace SangtuariCareerCompass.Controllers
{
    public class AssessmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AssessmentController(ApplicationDbContext context) => _context = context;

        // Page 1: Pilih Jenis Asesmen
        public IActionResult Index() => View();

        // Page 2: Data Diri Dasar
        public IActionResult BiodataBasic(string type)
        {
            if (string.IsNullOrEmpty(type)) return RedirectToAction("Index");
            ViewBag.AssessmentType = type;
            return View();
        }

        // Page 3: Data Tambahan
        [HttpPost]
        public IActionResult BiodataAdvanced(AssessmentSubmissionDto tempDto)
        {
            return View(tempDto);
        }

        // Halaman Coming Soon untuk Advanced
        public IActionResult ComingSoon() => View();

        // Endpoint Final untuk Simpan ke PostgreSQL JSONB
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitFinal([FromBody] AssessmentSubmissionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Data tidak valid." });

            try
            {
                var additionalDataObj = new
                {
                    dto.FatherJob,
                    dto.MotherJob,
                    dto.Hobby,
                    dto.Goals,
                    dto.LikedSubjects,
                    dto.DislikedSubjects
                };

                var assessment = new UserAssessment
                {
                    AssessmentType = dto.AssessmentType,
                    Email = dto.Email,
                    FullName = dto.FullName,
                    Gender = dto.Gender,
                    BirthDate = DateTime.SpecifyKind(dto.BirthDate, DateTimeKind.Utc),
                    SchoolName = dto.SchoolName,
                    ClassName = dto.ClassName,
                    Major = dto.Major,
                    AdditionalData = JsonDocument.Parse(JsonSerializer.Serialize(additionalDataObj))
                };

                _context.UserAssessments.Add(assessment);
                await _context.SaveChangesAsync();

                // Kembalikan ID assessment yang baru dibuat agar frontend bisa mengarahkannya ke tes pertama
                return Ok(new { success = true, userAssessmentId = assessment.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitSubTest([FromBody] SubTestSubmissionDto dto)
        {
            if (dto == null || dto.UserAssessmentId == Guid.Empty || string.IsNullOrEmpty(dto.SubTestName))
            {
                return BadRequest(new { success = false, message = "Payload data tidak valid." });
            }

            try
            {
                // Parse JsonElement DTO ke JsonDocument untuk kolom JSONB PostgreSQL
                var answersJsonDoc = JsonDocument.Parse(dto.Answers.GetRawText());

                var userAnswer = new UserAnswer
                {
                    UserAssessmentId = dto.UserAssessmentId,
                    SubTestName = dto.SubTestName,
                    Answers = answersJsonDoc,
                    SubmittedAt = DateTime.UtcNow
                };

                _context.UserAnswers.Add(userAnswer);
                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}