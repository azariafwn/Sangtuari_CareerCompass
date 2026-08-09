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
    }
}