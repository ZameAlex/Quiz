using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataClasses.Repositories;
using DataClasses.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using FirstWebApp.Models;
using System.Net;
using System.Net.Mime;

namespace FirstWebApp.Controllers
{
    public class ProfileController : Controller
    {
        public class Res
        {
            public string Text { get; set; }
            public string[] Answers { get; set; }
            public int Id { get; set; }
        }
        private static  Quiz TempQuiz;
        private static int QuestionId;
        private static int UserID;

        [Authorize]
        public ActionResult Index()
        {
            List<Quiz> quizes = new List<Quiz>();
            using (UserRepository DbContext = new UserRepository())
            {
                int? id = DbContext.GetItems(0).Where(x => x.Login.Trim() == User.Identity.Name).Select(x => x.ID).First();
                if(id.HasValue)
                {
                    UserID = (int)id;
                    using (QuizRepository QRep = new QuizRepository())
                    {
                        quizes = QRep.GetItems((int)id).ToList();
                    }
                }
            }
            return View(quizes);
        }

        #region Sync

        #region Create

        #region Get
        [HttpGet]
        public ActionResult CreateQuestionare()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CreateQuestion()
        {
            return View();
        }
        
        #endregion Get

        #region Post
        [HttpPost]
        public ActionResult CreateQuestionare(string name)
        {
            
            Quiz quiz = new Quiz();
            quiz.Name = name;
            quiz.URL = Guid.NewGuid().ToString();
            using (QuizRepository DbContext = new QuizRepository())
            using (UserRepository userRep = new UserRepository())
            {
                int id = 0;
                List<DataClasses.Models.User> users = userRep.GetItems(0).ToList();
                foreach (User u in users)
                {
                    if (User.Identity.Name == u.Login.Trim())
                        id = (int)u.ID;
                }
                DbContext.SaveItem(quiz, id);
                TempQuiz = quiz;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CreateQuestion(Res res)
        {
            DataClasses.Models.Question q = new DataClasses.Models.Question();
            TempQuiz.Questions.Add(q);
            using (QuestionRepository DbContext = new QuestionRepository())
            using (AnswerRepository Answers = new AnswerRepository())
            {
                if (res.Id == 0)
                    q.Type = QuestionTypes.OneAnswer;
                if (res.Id == 1)
                    q.Type = QuestionTypes.SomeAnswers;
                if (res.Id == 2)
                    q.Type = QuestionTypes.OpenAnswer;
                q.Text = res.Text;
                DbContext.SaveItem(q, (int)TempQuiz.DBID);

                if (res.Answers != null)
                    foreach (var item in res.Answers)
                    {
                        var answer = new Answer();
                        answer.Text = item;
                        if (res.Id == 0)
                            answer.Type = QuestionTypes.OneAnswer;
                        if (res.Id == 1)
                            answer.Type = QuestionTypes.SomeAnswers;
                        q.Answers.Add(answer);
                        Answers.SaveItem(answer, (int)q.DBID);
                    }
                
            }
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json("Message sent!", MediaTypeNames.Text.Plain);
        }
        
        #endregion Create

        #endregion Post

        #region Edit

        #region Get
        [HttpGet]
        public ActionResult EditQuestionare()
        {
            return View("EditQuestionare", TempQuiz);
        }
        [HttpGet]
        public ActionResult EditQuestion()
        {
            return View(TempQuiz.Questions.Where(x => x.DBID == QuestionId).First());
        }

        [HttpGet]
        public ActionResult Questions()
        {
            return View(TempQuiz);
        }

        #endregion Get

        #region Post
        [HttpPost]
        public ActionResult EditQuestionare(string name)
        {
            if(name!=null)
                TempQuiz.Name = name;
            using (QuizRepository DbContext = new QuizRepository())
            {
                DbContext.UpdateItem(TempQuiz, (int)TempQuiz.DBID);
            }
            return RedirectToAction("Questions");
        }
       
        [HttpPost]
        public ActionResult EditQuestion(string text, List<string> answers)
        {
            DataClasses.Models.Question q = TempQuiz.Questions.Where(x => x.DBID == QuestionId).First();
            q.Text = text;
            using (QuestionRepository DbContext = new QuestionRepository())
            using (AnswerRepository Answers = new AnswerRepository())
            {
                DbContext.UpdateItem(q, QuestionId);
                if (answers != null)
                    foreach (var item in answers)
                    {
                        Answer answer;
                        try
                        {
                            answer = q.Answers.ElementAt(answers.IndexOf(item));
                        }
                        catch (Exception e)
                        {
                            answer = new Answer();
                            q.Answers.Add(answer);
                        }
                        answer.Text = item;
                        Answers.UpdateItem(answer, (int)answer.ID);
                    }
            }
            QuestionId = (int)TempQuiz.Questions.SkipWhile(x => x.DBID != QuestionId).ElementAt(1).DBID;
            return RedirectToAction("EditQuestion");
        }

        public ActionResult SaveQuiz(string text, List<string> answers)
        {
            return RedirectToAction("Index");
        }

        #endregion Post

        #endregion Edit

       
        #endregion Sync

        #region Async
        [HttpPost]
        public void Edit(int ID)
        {
            using (QuizRepository DbContext = new QuizRepository())
            {
                TempQuiz = DbContext.GetItems(UserID).Where(x => x.DBID == ID).First();
            }
        }

        public void EditQ(int ID)
        {
            QuestionId = ID;
        }

        [HttpPost]
        public void DeleteQuestion(int ID)
        {
            using (QuestionRepository DbContext = new QuestionRepository())
            {
                DbContext.DeleteItem(TempQuiz.Questions.Where(x => x.DBID == ID).First(), (int)TempQuiz.DBID);
            }
            TempQuiz.Questions.Remove(TempQuiz.Questions.Where(x => x.DBID == ID).First());
        }

        [HttpPost]
        public void Delete(int ID)
        {
            using (QuizRepository DbContext = new QuizRepository())
            {
                DbContext.DeleteItem(DbContext.GetItems(UserID).Where(x => x.DBID == ID).First(), UserID);
            }
        }

        /*[HttpPost]
        public ActionResult DeleteQuestion(int ID)
        {

        }*/
        #endregion Async
    }
}