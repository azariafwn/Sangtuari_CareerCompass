using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace SangtuariCareerCompass.Controllers
{
    public class QuizController : Controller
    {
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
        public IActionResult Cfit(Guid userAssessmentId)
        {
            ViewData["SubTestName"] = "CFIT (Culture Fair Intelligence Test)";
            return View("~/Views/Quiz/ComingSoon.cshtml");
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
    }
}