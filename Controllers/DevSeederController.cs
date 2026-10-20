using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using SangtuariCareerCompass.Data;
using SangtuariCareerCompass.Models;

namespace SangtuariCareerCompass.Controllers
{
    public class DevSeederController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DevSeederController(ApplicationDbContext context) { _context = context; }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Generate(string schoolName, string grade, int count)
        {
            if (string.IsNullOrEmpty(grade))
            {
                TempData["Message"] = "Error: Kelas wajib dipilih!";
                return RedirectToAction("Index");
            }

            string[] firstNames =
            {
                // The Lord of the Rings
                "Aragorn", "Frodo", "Samwise", "Legolas", "Gandalf","Boromir", "Gimli", "Eowyn", "Arwen", "Theoden",
                // Project Hail Mary
                "Ryland", "Eva", "Stratt", "Grace", "Rocky",
                // Sherlock Holmes
                "Sherlock", "John", "Irene", "Mycroft", "Moriarty","Enola", "Eurus", "Gregory", "Sebastian", "Mary",
                // Knives Out Series
                "Benoit", "Marta", "Ransom", "Harlan", "Helen","Andi", "Duke", "Birdie", "Cassandra", "Hugh",
                // Inception
                "Dom", "Arthur", "Ariadne", "Eames", "Mal","Robert", "Saito", "Yusuf", "Miles", "Maurice",
                // Interstellar
                "Cooper", "Murph", "Brand", "Amelia", "TARS","CASE", "Tom", "Donald", "Romilly", "Mann",
                // Memento
                "Leonard", "Natalie", "Teddy", "Sammy", "Burt",
                // Peaky Blinders
                "Thomas", "Arthur", "John", "Ada", "Polly","Michael", "Alfie", "Finn", "Grace", "Esme",
                // MCU
                "Tony", "Steve", "Thor", "Bruce", "Natasha","Clint", "Peter", "Wanda", "Vision", "Stephen","Carol", "TChalla", "Shuri", "Sam", "Bucky","Scott", "Hope", "Loki", "Nick", "Yelena","Daredevil", "Frank", "Marc", "Kamala", "Kate",
                // X-Men / Fox Marvel
                "Logan", "Wade", "Charles", "Erik", "Jean","Scott", "Ororo", "Kurt", "Rogue", "Remy","Hank", "Bobby", "Laura", "Negasonic", "Peter","Colossus", "Katherine", "Alex", "Mystique", "Victor"
            };
            string[] lastNames =
            {
                // The Lord of the Rings
                "Baggins", "Gamgee", "Brandybuck", "Took", "Aragorn","Legolas", "Gimli", "Undomiel", "Rohan", "Gondor",
                // Project Hail Mary
                "Grace", "Stratt", "Rockysmith", "Yao", "Ilyukhina",
                // Sherlock Holmes
                "Holmes", "Watson", "Moriarty", "Adler", "Lestrade","Hudson", "Mycroft", "Moran", "Blackwood", "Hatherley",
                // Knives Out Series
                "Blanc", "Cabrera", "Drysdale", "Thrombey", "Brand","Jay", "Birdie", "Wicks", "Hollabrooke", "Bron",
                // Inception
                "Cobb", "Eames", "Fischer", "Ariadne", "Saito","Miles", "Yusuf", "Browning", "Mann", "Lind",
                // Interstellar
                "Cooper", "Brand", "Edmunds", "Mann", "Romilly","Doyle", "Murph", "Jenkins", "Getty", "Henderson",
                // Memento
                "Shelby", "Gammell", "Teddy", "Leonard", "Dodd",
                // Peaky Blinders
                "Shelby", "Gray", "Changretta", "Solomons", "Gold","Thorne", "Mosley", "Nelson", "Kimber", "McCavern",
                // MCU
                "Stark", "Rogers", "Odinson", "Banner", "Romanoff","Barton", "Parker", "Maximoff", "Strange", "Danvers","TChalla", "Wilson", "Barnes", "Lang", "Laufeyson","Fury", "Drew", "Murdock", "Castle", "Moonknight",
                // X-Men / Fox Marvel
                "Howlett", "Wilson", "Xavier", "Lehnsherr", "Grey","Summers", "Munroe", "Wagner", "LeBeau", "McCoy","Drake", "Kinney", "Rasputin", "Pryde", "Raven","Creed", "Lensherr", "Worthington", "MacTaggert", "Trask"
            };
            Random rnd = new Random();

            string assessmentType = "Advanced";
            int targetAge = 20;
            bool isSmp = false;
            bool isSma = false;

            if (grade.Contains("7") || grade.Contains("8") || grade.Contains("9"))
            {
                assessmentType = "Exploration";
                targetAge = grade.Contains("7") ? 13 : (grade.Contains("8") ? 14 : 15);
                isSmp = true;
            }
            else if (grade.Contains("10") || grade.Contains("11") || grade.Contains("12"))
            {
                assessmentType = "Discovery";
                targetAge = grade.Contains("10") ? 16 : (grade.Contains("11") ? 17 : 18);
                isSma = true;
            }

            int birthYear = 2026 - targetAge;
            string jsonAdditional = "{\"FatherJob\":\"PNS\",\"MotherJob\":\"Wiraswasta\",\"Hobby\":\"Membaca\",\"DreamJob\":\"Software Engineer\",\"LikedSubjects\":[\"Matematika\",\"Fisika\",\"Informatika\"],\"DislikedSubjects\":[\"Seni\",\"Sejarah\",\"Biologi\"]}";
            var docAdditional = JsonDocument.Parse(jsonAdditional);

            // Master template jawaban
            var allAnswers = new Dictionary<string, string>
            {
                // IST Subtests (1-9) untuk SMA
                { "IST_SubTest_01", "{\"Q1\": \"c\", \"Q2\": \"c\", \"Q3\": \"c\", \"Q4\": \"b\", \"Q5\": \"d\", \"Q6\": \"c\", \"Q7\": \"c\", \"Q8\": \"e\", \"Q9\": \"e\", \"Q10\": \"c\", \"Q11\": \"e\", \"Q12\": \"c\", \"Q13\": \"d\", \"Q14\": \"e\", \"Q15\": \"e\", \"Q16\": \"d\", \"Q17\": \"c\", \"Q18\": \"d\", \"Q19\": \"b\", \"Q20\": \"b\"}" },
                { "IST_SubTest_02", "{\"Q21\": \"e\", \"Q22\": \"c\", \"Q23\": \"d\", \"Q24\": \"a\", \"Q25\": \"c\", \"Q26\": \"a\", \"Q27\": \"d\", \"Q28\": \"b\", \"Q29\": \"b\", \"Q30\": \"c\", \"Q31\": \"c\", \"Q32\": \"d\", \"Q33\": \"c\", \"Q34\": \"e\", \"Q35\": \"d\", \"Q36\": \"c\", \"Q37\": \"e\", \"Q38\": \"b\", \"Q39\": \"d\", \"Q40\": \"d\"}" },
                { "IST_SubTest_03", "{\"Q41\": \"b\", \"Q42\": \"d\", \"Q43\": \"a\", \"Q44\": \"e\", \"Q45\": \"e\", \"Q46\": \"d\", \"Q47\": \"a\", \"Q48\": \"d\", \"Q49\": \"b\", \"Q50\": \"b\", \"Q51\": \"d\", \"Q52\": \"b\", \"Q53\": \"c\", \"Q54\": \"e\", \"Q55\": \"a\", \"Q56\": \"a\", \"Q57\": \"b\", \"Q58\": \"b\", \"Q59\": \"b\", \"Q60\": \"d\"}" },
                { "IST_SubTest_04", "{\"Q61\": \"Sed corrupti eu bea\", \"Q62\": \"Ipsam nostrud cillum\", \"Q63\": \"Nulla dolore veniam\", \"Q64\": \"Error dolor dolor no\", \"Q65\": \"Et magna velit paria\", \"Q66\": \"Et mollit cumque do\", \"Q67\": \"Et earum cupidatat v\", \"Q68\": \"Sit quis qui quam ei\", \"Q69\": \"Eos eius commodo qu\", \"Q70\": \"Incidunt soluta eni\", \"Q71\": \"Repudiandae quis non\", \"Q72\": \"Unde deleniti do inc\", \"Q73\": \"Nesciunt et quia au\", \"Q74\": \"In dolore officiis e\", \"Q75\": \"Eiusmod incidunt vo\", \"Q76\": \"Ipsa omnis aut quia\"}" },
                { "IST_SubTest_05", "{\"Q77\": \"85\", \"Q78\": \"26\", \"Q79\": \"59\", \"Q80\": \"52\", \"Q81\": \"48\", \"Q82\": \"24\", \"Q83\": \"20\", \"Q84\": \"31\", \"Q85\": \"77\", \"Q86\": \"45\", \"Q87\": \"62\", \"Q88\": \"14\", \"Q89\": \"30\", \"Q90\": \"51\", \"Q91\": \"92\", \"Q92\": \"2\", \"Q93\": \"12\", \"Q94\": \"34\", \"Q95\": \"28\", \"Q96\": \"16\"}" },
                { "IST_SubTest_06", "{\"Q97\": \"31\", \"Q98\": \"37\", \"Q99\": \"8\", \"Q100\": \"56\", \"Q101\": \"82\", \"Q102\": \"20\", \"Q103\": \"57\", \"Q104\": \"96\", \"Q105\": \"97\", \"Q106\": \"93\", \"Q107\": \"64\", \"Q108\": \"25\", \"Q109\": \"19\", \"Q110\": \"52\", \"Q111\": \"100\", \"Q112\": \"26\", \"Q113\": \"1\", \"Q114\": \"76\", \"Q115\": \"71\", \"Q116\": \"64\"}" },
                { "IST_SubTest_07", "{\"Q117\": \"b\", \"Q118\": \"b\", \"Q119\": \"b\", \"Q120\": \"c\", \"Q121\": \"d\", \"Q122\": \"a\", \"Q123\": \"a\", \"Q124\": \"b\", \"Q125\": \"e\", \"Q126\": \"a\", \"Q127\": \"b\", \"Q128\": \"a\", \"Q129\": \"b\", \"Q130\": \"a\", \"Q131\": \"c\", \"Q132\": \"b\", \"Q133\": \"a\", \"Q134\": \"a\", \"Q135\": \"b\", \"Q136\": \"d\"}" },
                { "IST_SubTest_08", "{\"Q137\": \"c\", \"Q138\": \"e\", \"Q139\": \"d\", \"Q140\": \"a\", \"Q141\": \"c\", \"Q142\": \"a\", \"Q143\": \"d\", \"Q144\": \"d\", \"Q145\": \"e\", \"Q146\": \"a\", \"Q147\": \"c\", \"Q148\": \"d\", \"Q149\": \"b\", \"Q150\": \"d\", \"Q151\": \"e\", \"Q152\": \"b\", \"Q153\": \"b\", \"Q154\": \"a\", \"Q155\": \"e\", \"Q156\": \"c\"}" },
                { "IST_SubTest_09", "{\"Q157\": \"e\", \"Q158\": \"e\", \"Q159\": \"e\", \"Q160\": \"b\", \"Q161\": \"c\", \"Q162\": \"d\", \"Q163\": \"d\", \"Q164\": \"d\", \"Q165\": \"e\", \"Q166\": \"b\", \"Q167\": \"d\", \"Q168\": \"c\", \"Q169\": \"a\", \"Q170\": \"a\", \"Q171\": \"a\", \"Q172\": \"d\", \"Q173\": \"b\", \"Q174\": \"a\", \"Q175\": \"b\", \"Q176\": \"a\"}" },
                { "PAPI_Kostick", "{\"Q1\": \"B\", \"Q2\": \"B\", \"Q3\": \"A\", \"Q4\": \"A\", \"Q5\": \"B\", \"Q6\": \"A\", \"Q7\": \"B\", \"Q8\": \"A\", \"Q9\": \"A\", \"Q10\": \"A\", \"Q11\": \"A\", \"Q12\": \"B\", \"Q13\": \"A\", \"Q14\": \"B\", \"Q15\": \"A\", \"Q16\": \"B\", \"Q17\": \"B\", \"Q18\": \"A\", \"Q19\": \"B\", \"Q20\": \"B\", \"Q21\": \"A\", \"Q22\": \"A\", \"Q23\": \"B\", \"Q24\": \"B\", \"Q25\": \"A\", \"Q26\": \"A\", \"Q27\": \"B\", \"Q28\": \"B\", \"Q29\": \"A\", \"Q30\": \"B\", \"Q31\": \"B\", \"Q32\": \"B\", \"Q33\": \"B\", \"Q34\": \"B\", \"Q35\": \"A\", \"Q36\": \"B\", \"Q37\": \"A\", \"Q38\": \"A\", \"Q39\": \"B\", \"Q40\": \"B\", \"Q41\": \"A\", \"Q42\": \"A\", \"Q43\": \"B\", \"Q44\": \"B\", \"Q45\": \"A\", \"Q46\": \"B\", \"Q47\": \"A\", \"Q48\": \"B\", \"Q49\": \"B\", \"Q50\": \"B\", \"Q51\": \"B\", \"Q52\": \"B\", \"Q53\": \"A\", \"Q54\": \"B\", \"Q55\": \"B\", \"Q56\": \"B\", \"Q57\": \"A\", \"Q58\": \"B\", \"Q59\": \"B\", \"Q60\": \"A\", \"Q61\": \"A\", \"Q62\": \"B\", \"Q63\": \"A\", \"Q64\": \"A\", \"Q65\": \"A\", \"Q66\": \"B\", \"Q67\": \"B\", \"Q68\": \"A\", \"Q69\": \"A\", \"Q70\": \"B\", \"Q71\": \"A\", \"Q72\": \"B\", \"Q73\": \"B\", \"Q74\": \"B\", \"Q75\": \"A\", \"Q76\": \"A\", \"Q77\": \"B\", \"Q78\": \"A\", \"Q79\": \"B\", \"Q80\": \"A\", \"Q81\": \"A\", \"Q82\": \"A\", \"Q83\": \"A\", \"Q84\": \"B\", \"Q85\": \"A\", \"Q86\": \"A\", \"Q87\": \"A\", \"Q88\": \"A\", \"Q89\": \"B\", \"Q90\": \"A\"}" },
                
                // CFIT & EAS untuk SMP
                { "CFIT_SubTest_01", "{\"Q1\": \"d\", \"Q2\": \"e\", \"Q3\": \"e\", \"Q4\": \"a\", \"Q5\": \"d\", \"Q6\": \"e\", \"Q7\": \"a\", \"Q8\": \"a\", \"Q9\": \"a\", \"Q10\": \"d\", \"Q11\": \"b\", \"Q12\": \"d\"}" },
                { "CFIT_SubTest_02", "{\"Q1\": \"e\", \"Q2\": \"d\", \"Q3\": \"d\", \"Q4\": \"d\", \"Q5\": \"d\", \"Q6\": \"c\", \"Q7\": \"c\", \"Q8\": \"d\", \"Q9\": \"a\", \"Q10\": \"c\", \"Q11\": \"d\", \"Q12\": \"c\", \"Q13\": \"c\", \"Q14\": \"c\"}" },
                { "CFIT_SubTest_03", "{\"Q1\": \"a\", \"Q2\": \"e\", \"Q3\": \"a\", \"Q4\": \"e\", \"Q5\": \"e\", \"Q6\": \"d\", \"Q7\": \"d\", \"Q8\": \"b\", \"Q9\": \"b\", \"Q10\": \"d\", \"Q11\": \"c\", \"Q12\": \"d\"}" },
                { "CFIT_SubTest_04", "{\"Q1\": \"b\", \"Q2\": \"b\", \"Q3\": \"e\", \"Q4\": \"c\", \"Q5\": \"b\", \"Q6\": \"e\", \"Q7\": \"d\", \"Q8\": \"e\"}" },
                { "EAS-1", "{\"Q1\": \"tebal\", \"Q2\": \"pola\", \"Q3\": \"muda\", \"Q4\": \"pakan\", \"Q5\": \"sangsi\", \"Q6\": \"tidak suka\", \"Q7\": \"manja\", \"Q8\": \"pengemis\", \"Q9\": \"lemah\", \"Q10\": \"melingkupi\", \"Q11\": \"menggoda\", \"Q12\": \"menyimpang\", \"Q13\": \"yang menentukan\", \"Q14\": \"cuka\", \"Q15\": \"mencegah\", \"Q16\": \"tidak takut\", \"Q17\": \"efektif\", \"Q18\": \"mencicipi\", \"Q19\": \"rayap\", \"Q20\": \"lembut\", \"Q21\": \"berkesinambungan\", \"Q22\": \"acak\", \"Q23\": \"menulari\", \"Q24\": \"diam\", \"Q25\": \"keji\", \"Q26\": \"kebencian\", \"Q27\": \"meninggalkan\", \"Q28\": \"instan\", \"Q29\": \"bui\", \"Q30\": \"firasat\"}" },
                { "EAS-2", "{\"PART I_Q1\": \"X\", \"PART I_Q2\": \"2\", \"PART I_Q3\": \"21\", \"PART I_Q4\": \"6\", \"PART I_Q5\": \"X\", \"PART I_Q6\": \"15\", \"PART I_Q7\": \"59\", \"PART I_Q8\": \"6\", \"PART I_Q9\": \"46\", \"PART II_Q1\": \".28\", \"PART II_Q2\": \".39\", \"PART II_Q3\": \"X\", \"PART II_Q4\": \"6.0\", \"PART II_Q5\": \".028\", \"PART II_Q6\": \"1.3\", \"PART II_Q7\": \".047\", \"PART II_Q8\": \".42\", \"PART II_Q9\": \"13\", \"PART I_Q10\": \"X\", \"PART I_Q11\": \"62\", \"PART I_Q12\": \"17\", \"PART I_Q13\": \"74\", \"PART I_Q14\": \"35\", \"PART I_Q15\": \"159\", \"PART I_Q16\": \"15\", \"PART I_Q17\": \"75\", \"PART I_Q18\": \"66\", \"PART I_Q19\": \"200\", \"PART I_Q20\": \"X\", \"PART I_Q21\": \"73\", \"PART I_Q22\": \"66\", \"PART I_Q23\": \"262\", \"PART I_Q24\": \"18\", \"PART I_Q25\": \"443\", \"PART III_Q1\": \"2/6\", \"PART III_Q2\": \"1/3\", \"PART III_Q3\": \"X\", \"PART III_Q4\": \"3/4\", \"PART III_Q5\": \"1/12\", \"PART III_Q6\": \"2/8\", \"PART III_Q7\": \"1/3\", \"PART III_Q8\": \"1/8\", \"PART III_Q9\": \"2/12\", \"PART II_Q10\": \"10\", \"PART II_Q11\": \"25\", \"PART II_Q12\": \"1.08\", \"PART II_Q13\": \".075\", \"PART II_Q14\": \"3\", \"PART II_Q15\": \".053\", \"PART II_Q16\": \"3.32\", \"PART II_Q17\": \"1.44\", \"PART II_Q18\": \"15\", \"PART II_Q19\": \"X\", \"PART II_Q20\": \"28.0\", \"PART II_Q21\": \"1.87\", \"PART II_Q22\": \"2.11\", \"PART II_Q23\": \"X\", \"PART II_Q24\": \"11.0\", \"PART II_Q25\": \"9.0\", \"PART III_Q10\": \"1/3\", \"PART III_Q11\": \"2/8\", \"PART III_Q12\": \"1\", \"PART III_Q13\": \"3/7\", \"PART III_Q14\": \"4/9\", \"PART III_Q15\": \"8/12\", \"PART III_Q16\": \"1/27\", \"PART III_Q17\": \"8/10\", \"PART III_Q18\": \"1\", \"PART III_Q19\": \"2/3\", \"PART III_Q20\": \"5/6\", \"PART III_Q21\": \"X\", \"PART III_Q22\": \"1 4/3\", \"PART III_Q23\": \"2 1/8\", \"PART III_Q24\": \"3 2/3\", \"PART III_Q25\": \"X\"}" },
                { "EAS-5", "{\"Q1_BlockA\": \"6\", \"Q2_BlockB\": \"5\", \"Q3_BlockC\": \"4\", \"Q4_BlockD\": \"3\", \"Q5_BlockE\": \"1\", \"Q6_BlockA\": \"8\", \"Q7_BlockB\": \"3\", \"Q8_BlockC\": \"8\", \"Q9_BlockD\": \"1\", \"Q10_BlockE\": \"4\", \"Q11_BlockA\": \"4\", \"Q12_BlockB\": \"9\", \"Q13_BlockC\": \"8\", \"Q14_BlockD\": \"9\", \"Q15_BlockE\": \"4\", \"Q16_BlockA\": \"2\", \"Q17_BlockB\": \"3\", \"Q18_BlockC\": \"8\", \"Q19_BlockD\": \"3\", \"Q20_BlockE\": \"10\", \"Q21_BlockA\": \"3\", \"Q22_BlockB\": \"4\", \"Q23_BlockC\": \"8\", \"Q24_BlockD\": \"9\", \"Q25_BlockE\": \"7\", \"Q26_BlockA\": \"10\", \"Q27_BlockB\": \"9\", \"Q28_BlockC\": \"9\", \"Q29_BlockD\": \"10\", \"Q30_BlockE\": \"2\", \"Q31_BlockA\": \"3\", \"Q32_BlockB\": \"10\", \"Q33_BlockC\": \"2\", \"Q34_BlockD\": \"10\", \"Q35_BlockE\": \"6\", \"Q36_BlockA\": \"7\", \"Q37_BlockB\": \"6\", \"Q38_BlockC\": \"6\", \"Q39_BlockD\": \"5\", \"Q40_BlockE\": \"6\", \"Q41_BlockA\": \"1\", \"Q42_BlockB\": \"1\", \"Q43_BlockC\": \"6\", \"Q44_BlockD\": \"6\", \"Q45_BlockE\": \"1\", \"Q46_BlockA\": \"8\", \"Q47_BlockB\": \"6\", \"Q48_BlockC\": \"9\", \"Q49_BlockD\": \"2\", \"Q50_BlockE\": \"9\"}" },
                
                // VARK & Holland (Berlaku untuk SMP dan SMA)
                { "VARK", "{\"Scores\": {\"A\": 8, \"K\": 7, \"R\": 7, \"V\": 7}, \"UserResponses\": {\"Q1\": [\"a\", \"b\"], \"Q2\": [\"c\"], \"Q3\": [\"a\", \"d\"], \"Q4\": [\"b\", \"c\", \"d\"], \"Q5\": [], \"Q6\": [\"b\"], \"Q7\": [\"a\", \"b\"], \"Q8\": [\"a\", \"b\"], \"Q9\": [\"b\", \"d\"], \"Q10\": [\"b\", \"d\"], \"Q11\": [\"a\", \"b\", \"d\"], \"Q12\": [\"a\", \"b\"], \"Q13\": [\"b\", \"d\"], \"Q14\": [\"a\"], \"Q15\": [\"a\", \"d\"], \"Q16\": [\"a\", \"c\"]}}" },
                { "SDS_Holland", "{\"Scores\": {\"A\": 8, \"C\": 8, \"E\": 6, \"I\": 11, \"R\": 8, \"S\": 10}, \"Consistency\": \"Tinggi\", \"UserResponses\": {\"Q1\": \"TIDAK\", \"Q2\": \"YA\", \"Q3\": \"TIDAK\", \"Q4\": \"YA\", \"Q5\": \"TIDAK\", \"Q6\": \"TIDAK\", \"Q7\": \"YA\", \"Q8\": \"YA\", \"Q9\": \"TIDAK\", \"Q10\": \"YA\", \"Q11\": \"YA\", \"Q12\": \"TIDAK\", \"Q13\": \"YA\", \"Q14\": \"TIDAK\", \"Q15\": \"TIDAK\", \"Q16\": \"TIDAK\", \"Q17\": \"YA\", \"Q18\": \"YA\", \"Q19\": \"TIDAK\", \"Q20\": \"TIDAK\", \"Q21\": \"YA\", \"Q22\": \"TIDAK\", \"Q23\": \"YA\", \"Q24\": \"TIDAK\", \"Q25\": \"YA\", \"Q26\": \"YA\", \"Q27\": \"YA\", \"Q28\": \"TIDAK\", \"Q29\": \"TIDAK\", \"Q30\": \"TIDAK\", \"Q31\": \"TIDAK\", \"Q32\": \"YA\", \"Q33\": \"TIDAK\", \"Q34\": \"YA\", \"Q35\": \"YA\", \"Q36\": \"YA\", \"Q37\": \"TIDAK\", \"Q38\": \"YA\", \"Q39\": \"TIDAK\", \"Q40\": \"TIDAK\", \"Q41\": \"YA\", \"Q42\": \"YA\", \"Q43\": \"YA\", \"Q44\": \"YA\", \"Q45\": \"YA\", \"Q46\": \"TIDAK\", \"Q47\": \"TIDAK\", \"Q48\": \"TIDAK\", \"Q49\": \"YA\", \"Q50\": \"TIDAK\", \"Q51\": \"TIDAK\", \"Q52\": \"YA\", \"Q53\": \"TIDAK\", \"Q54\": \"TIDAK\", \"Q55\": \"YA\", \"Q56\": \"TIDAK\", \"Q57\": \"YA\", \"Q58\": \"TIDAK\", \"Q59\": \"YA\", \"Q60\": \"YA\", \"Q61\": \"YA\", \"Q62\": \"YA\", \"Q63\": \"YA\", \"Q64\": \"TIDAK\", \"Q65\": \"TIDAK\", \"Q66\": \"YA\", \"Q67\": \"TIDAK\", \"Q68\": \"TIDAK\", \"Q69\": \"TIDAK\", \"Q70\": \"YA\", \"Q71\": \"TIDAK\", \"Q72\": \"YA\", \"Q73\": \"YA\", \"Q74\": \"TIDAK\", \"Q75\": \"YA\", \"Q76\": \"YA\", \"Q77\": \"TIDAK\", \"Q78\": \"TIDAK\", \"Q79\": \"TIDAK\", \"Q80\": \"YA\", \"Q81\": \"TIDAK\", \"Q82\": \"TIDAK\", \"Q83\": \"TIDAK\", \"Q84\": \"YA\", \"Q85\": \"TIDAK\", \"Q86\": \"TIDAK\", \"Q87\": \"TIDAK\", \"Q88\": \"TIDAK\", \"Q89\": \"YA\", \"Q90\": \"TIDAK\", \"Q91\": \"YA\", \"Q92\": \"TIDAK\", \"Q93\": \"YA\", \"Q94\": \"YA\", \"Q95\": \"TIDAK\", \"Q96\": \"YA\", \"Q97\": \"YA\", \"Q98\": \"TIDAK\", \"Q99\": \"YA\", \"Q100\": \"TIDAK\", \"Q101\": \"TIDAK\", \"Q102\": \"TIDAK\", \"Q103\": \"YA\", \"Q104\": \"YA\", \"Q105\": \"TIDAK\", \"Q106\": \"YA\", \"Q107\": \"YA\", \"Q108\": \"TIDAK\"}}" }
            };

            for (int i = 0; i < count; i++)
            {
                string name = $"{firstNames[rnd.Next(firstNames.Length)]} {lastNames[rnd.Next(lastNames.Length)]} {i + 1}";
                var birthDate = DateTime.SpecifyKind(new DateTime(birthYear, 6, 15), DateTimeKind.Utc);

                var user = new UserAssessment
                {
                    AssessmentType = assessmentType,
                    FullName = name,
                    Email = $"user{rnd.Next(1000, 9999)}@test.com",
                    Gender = i % 2 == 0 ? "Perempuan" : "Laki-laki",
                    BirthDate = birthDate,
                    SchoolName = schoolName,
                    ClassName = grade,
                    Major = "IPA",
                    AdditionalData = docAdditional,
                    CreatedAt = DateTime.UtcNow
                };

                _context.UserAssessments.Add(user);
                _context.SaveChanges();

                // Filter sub-tes berdasarkan jenjang
                foreach (var template in allAnswers)
                {
                    bool isIstOrPapi = template.Key.StartsWith("IST_") || template.Key == "PAPI_Kostick";
                    bool isCfitOrEas = template.Key.StartsWith("CFIT_") || template.Key.StartsWith("EAS-");

                    if (isSmp && isIstOrPapi) continue; // SMP tidak pakai IST & PAPI
                    if (isSma && isCfitOrEas) continue; // SMA tidak pakai CFIT & EAS

                    var userAnswer = new UserAnswer
                    {
                        UserAssessmentId = user.Id,
                        SubTestName = template.Key,
                        Answers = JsonDocument.Parse(template.Value),
                        SubmittedAt = DateTime.UtcNow
                    };
                    _context.UserAnswers.Add(userAnswer);
                }

                // Masukkan juga template UserTestResults agar diagram psikogram, VARK, Holland & Konsistensi ter-render
                if (isSmp)
                {
                    _context.UserTestResults.Add(new UserTestResult
                    {
                        UserAssessmentId = user.Id,
                        TestCategory = "CFIT",
                        OverallScore = 55,
                        Classification = "Average",
                        ResultDetails = JsonDocument.Parse("{\"AgeInMonths\": 180, \"SubTestScores\": [{\"TotalSoal\": 12, \"SubTestName\": \"Subtes 1\", \"CorrectCount\": 8}, {\"TotalSoal\": 14, \"SubTestName\": \"Subtes 2\", \"CorrectCount\": 10}, {\"TotalSoal\": 12, \"SubTestName\": \"Subtes 3\", \"CorrectCount\": 9}, {\"TotalSoal\": 8, \"SubTestName\": \"Subtes 4\", \"CorrectCount\": 6}], \"TotalRawScore\": 33}")
                    });
                    _context.UserTestResults.Add(new UserTestResult
                    {
                        UserAssessmentId = user.Id,
                        TestCategory = "EAS",
                        OverallScore = 85,
                        Classification = "Baik",
                        ResultDetails = JsonDocument.Parse("[{\"Aspek\": \"Kemampuan Verbal\", \"Category\": \"Baik\", \"RawScore\": 22, \"Percentile\": 75, \"SubTestName\": \"EAS 1\"}, {\"Aspek\": \"Kemampuan Numerik\", \"Category\": \"Baik\", \"RawScore\": 20, \"Percentile\": 70, \"SubTestName\": \"EAS 2\"}, {\"Aspek\": \"Visual Spasial\", \"Category\": \"Baik\", \"RawScore\": 35, \"Percentile\": 80, \"SubTestName\": \"EAS 5\"}]")
                    });
                }

                // Hasil VARK & Holland (Umum untuk SMP/SMA)
                _context.UserTestResults.Add(new UserTestResult
                {
                    UserAssessmentId = user.Id,
                    TestCategory = "VARK",
                    OverallScore = 10,
                    Classification = "Aural/auditori",
                    ResultDetails = JsonDocument.Parse("[{\"Score\": 7, \"Percentage\": 24.1, \"CategoryCode\": \"V\", \"CategoryName\": \"Visual\"}, {\"Score\": 8, \"Percentage\": 27.6, \"CategoryCode\": \"A\", \"CategoryName\": \"Aural/auditori\"}, {\"Score\": 7, \"Percentage\": 24.1, \"CategoryCode\": \"R\", \"CategoryName\": \"Read/Write\"}, {\"Score\": 7, \"Percentage\": 24.1, \"CategoryCode\": \"K\", \"CategoryName\": \"Kinesthetic\"}]")
                });
                _context.UserTestResults.Add(new UserTestResult
                {
                    UserAssessmentId = user.Id,
                    TestCategory = "SDS_Holland",
                    OverallScore = 11,
                    Classification = "RIA",
                    ResultDetails = JsonDocument.Parse("{\"Scores\": [{\"Code\": \"R\", \"Name\": \"Realistic\", \"Score\": 8, \"Percentage\": 15.7}, {\"Code\": \"I\", \"Name\": \"Investigative\", \"Score\": 11, \"Percentage\": 21.6}, {\"Score\": \"A\", \"Name\": \"Artistic\", \"Score\": 8, \"Percentage\": 15.7}, {\"Code\": \"S\", \"Name\": \"Social\", \"Score\": 10, \"Percentage\": 19.6}, {\"Code\": \"E\", \"Name\": \"Enterprising\", \"Score\": 6, \"Percentage\": 11.8}, {\"Code\": \"C\", \"Name\": \"Conventional\", \"Score\": 8, \"Percentage\": 15.7}], \"Consistency\": \"Tinggi\", \"SummaryCode\": \"RIA\"}")
                });
            }
            _context.SaveChanges();

            TempData["Message"] = $"Berhasil generate {count} data dummy lengkap dengan jawaban & test result ({assessmentType}) untuk kelas {grade}!";
            return RedirectToAction("Index");
        }
    }
}