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
            ViewData["SubTestName"] = "IST 07: FA (Figurenauswahl)";
            ViewBag.UserAssessmentId = userAssessmentId;
            return View("~/Views/Quiz/ComingSoon.cshtml");
        }

        public IActionResult IstSubTest8(Guid userAssessmentId)
        {
            ViewData["SubTestName"] = "IST 08: WU (Würfelaufgaben)";
            ViewBag.UserAssessmentId = userAssessmentId;
            return View("~/Views/Quiz/ComingSoon.cshtml");
        }

        public IActionResult IstSubTest9(Guid userAssessmentId)
        {
            ViewData["SubTestName"] = "IST 09: ME (Merkaufgaben)";
            ViewBag.UserAssessmentId = userAssessmentId;
            return View("~/Views/Quiz/ComingSoon.cshtml");
        }
    }
}