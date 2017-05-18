using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataClasses.Models;
using DataClasses.Repositories;
using DataClasses.AnsweringRepositories;

namespace TestProject
{


    class Program
    {
        static void Write(object x)
        {
            var y = x.GetType().GetProperties();
            foreach (System.Reflection.PropertyInfo item in y)
            {
                Console.WriteLine(item.GetValue(x));
            }
        }
        static void Main(string[] args)
        {
            DataClasses.Models.Answer ans = new DataClasses.Models.Answer();
            ans.Text = "dfsg";
            ans.Type = (int)QuestionTypes.OneAnswer;
            using (AnswerRepository ar = new AnswerRepository())
            {
                ar.SaveItem(ans,1);
            }
            int i = 1;
            Console.WriteLine("i={0}",i++);
            Quizes quiz = new Quizes();
            quiz.ID = 1;
            quiz.Name = "Quiz1";
            Questions q = new Questions();
            q.Text = "WTF?";
            DataClasses.AnsweringRepositories.Answers a = new DataClasses.AnsweringRepositories.Answers();
            a.ID = 18;
            a.Text = "1";
            a.Number = 1;
            DataClasses.AnsweringRepositories.Answers a1 = new DataClasses.AnsweringRepositories.Answers();
            a1.Text = "2";
            a1.Number = 2;
            DataClasses.AnsweringRepositories.Answers a2 = new DataClasses.AnsweringRepositories.Answers();
            a2.Text = "3";
            a2.Number = 3;
            Respondents r = new Respondents();
            r.Name = "Alex";
            r.Surname = "Zame";
            int ida;
            using (AAnswersRepository DB = new AAnswersRepository())
            {
                ida = DB.SaveItem(a, 0);
            }
            /*using (AQuizRepository DB = new AQuizRepository())
            {
                idquiz = DB.SaveItem(quiz, 0);
            }
            using (AQuestionRepository DB = new AQuestionRepository())
            {
               idq = DB.SaveItem(q, 0);
            }
            using (ARespondentsRepository DB = new ARespondentsRepository())
            {
                idr = DB.SaveItem(r, 0);
            }*/
            Records rec = new Records();
            rec.IDAnswer = ida;
            rec.IDQuestion = 1;
            rec.IDQuiz = 1;
            rec.IDRespondent = 1;
            rec.DateTime = DateTime.Now;
            using (ARecordsRepository Db = new ARecordsRepository())
            {
                Db.SaveItem(rec, 0);
            }
            Write(rec);
            Console.ReadKey();
        }
    }
}
