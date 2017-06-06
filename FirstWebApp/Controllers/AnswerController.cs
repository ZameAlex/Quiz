using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataClasses;
using DataClasses.Models;

namespace FirstWebApp.Controllers
{
    public class AnswerController : Controller
    {
        private static Question TempQuestion;
        private static Quiz TempQuiz;
        public static List<DataClasses.AnsweringRepositories.Records> records;
        public static DataClasses.AnsweringRepositories.Respondents resp;
        private static int TempIDQuiz;
        public class Res
        {
            public string[] Answers { get; set; }
        }


        // GET: Answer
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(int? id)
        {
            records = new List<DataClasses.AnsweringRepositories.Records>();
            ViewData.Add(new KeyValuePair<string, object>("IsLast", false));
            using (DataClasses.Repositories.QuizRepository DbContext = new DataClasses.Repositories.QuizRepository())
            {
                TempQuiz = DbContext.GetItemByURL(id.ToString());
            }
            using (DataClasses.AnsweringRepositories.AQuizRepository DbContext = new DataClasses.AnsweringRepositories.AQuizRepository())
            {
                TempIDQuiz = DbContext.GetItems(0).Where(x => x.Name.Trim() == TempQuiz.Name.Trim()).First().ID;
            }
            TempQuestion = TempQuiz.Questions.First();
            return View(TempQuiz.Questions.First());
        }
        [HttpPost]
        public void Index(Res r)
        {
            DataClasses.AnsweringRepositories.Records rec = new DataClasses.AnsweringRepositories.Records();
            if (TempQuestion==TempQuiz.Questions.First())
            {
                using (DataClasses.AnsweringRepositories.ARespondentsRepository ares = new DataClasses.AnsweringRepositories.ARespondentsRepository())
                {
                    DataClasses.AnsweringRepositories.Respondents res = new DataClasses.AnsweringRepositories.Respondents();
                    res.Name = r.Answers[0];
                    ares.SaveItem(res, 0);
                    resp = res;
                }
     
            }
            rec.IDRespondent = resp.ID;
            rec.IDQuiz = TempIDQuiz;
            using (DataClasses.AnsweringRepositories.AQuestionRepository DbContext = new DataClasses.AnsweringRepositories.AQuestionRepository())
            {
                rec.IDQuestion = DbContext.GetItems(0).Where(x => x.Text.Trim() == TempQuestion.Text.Trim()).First().ID;
            }
            QuestionTypes id = TempQuestion.Type;
            using (DataClasses.AnsweringRepositories.AAnswersRepository DbContext = new DataClasses.AnsweringRepositories.AAnswersRepository())
            {
                switch (id)
                {
                    case QuestionTypes.OneAnswer:
                        rec.IDAnswer = DbContext.GetItems(0).Where(x => x.Number == Convert.ToInt32(r.Answers[0])).First().ID;
                        records.Add(rec);
                        break;
                    case QuestionTypes.SomeAnswers:
                        foreach (var item in r.Answers)
                        {
                            var newRec = new DataClasses.AnsweringRepositories.Records();
                            newRec.IDQuestion = rec.IDQuestion;
                            newRec.IDQuiz = rec.IDQuiz;
                            newRec.IDRespondent = rec.IDRespondent;
                            newRec.IDAnswer = DbContext.GetItems(0).Where(x => x.Number == Convert.ToInt32(item)).First().ID;
                            records.Add(newRec);
                        }
                        break;
                    case QuestionTypes.OpenAnswer:
                        DataClasses.AnsweringRepositories.Answers a = new DataClasses.AnsweringRepositories.Answers();
                        a.Text = r.Answers[0];
                        rec.IDAnswer = DbContext.SaveItem(a, 0);
                        records.Add(rec);
                        break;
                    default:
                        break;
                }
            }
            for (int i = 0; i < TempQuiz.Questions.Count; i++)
            {
                if(TempQuiz.Questions.ElementAt(i)==TempQuestion && TempQuiz.Questions.Count>i+1)
                {
                    TempQuestion = TempQuiz.Questions.ElementAt(i + 1);
                    break;
                }
            }
        }
        [HttpGet]
        public ActionResult Next()
        {
            if (TempQuiz.Questions.Last() != TempQuestion)
                ViewData.Add(new KeyValuePair<string, object>("IsLast", false));
            else
                ViewData.Add(new KeyValuePair<string, object>("IsLast", true));
            return View("Index",TempQuestion);
        }
        [HttpGet]
        public ActionResult Finish()
        {
            foreach (var item in records)
            {
                item.DateTime = DateTime.Now;
            }
            using (DataClasses.AnsweringRepositories.ARecordsRepository DbContext = new DataClasses.AnsweringRepositories.ARecordsRepository())
            {
                DbContext.SaveItems(records,0);
            }
                return RedirectToAction("Index", "Home");
        }
    }
}