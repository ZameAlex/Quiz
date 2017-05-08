using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Models;
using TestProject.Repositories;

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
            User x = new User();
            x.Login = "Asd";
            x.Password = "QWe";
            x.Role = 1;

            List<Answer> answers1 = new List<Answer>();
            Answer a = new Answer();
            a.Text = "first";
            a.Type = QuestionTypes.OneAnswer;
            answers1.Add(a);
            a = new Answer();
            a.Type = QuestionTypes.OneAnswer;
            a.Text = "second";
            answers1.Add(a);

    
            Rule r = new Rule();
            r.Answers.Add(answers1[0]);
            r.AnswersStr = "10";
            r.NextQuestion = 2;
            Rule r1 = new Rule();
            r1.NextQuestion = 3;
            r1.AnswersStr = "01";
            r1.Answers.Add(answers1[1]);
            r.NextRule = r1.DBID;
            List<Models.Rule> rules = new List<Rule> { r, r1 };

            Models.Question q = new Models.Question();
            q.Text = "fq";
            q.Type = QuestionTypes.OneAnswer;
            q.Answers = answers1;
            q.Rules = new List<Rule> { r, r1 };
            Models.Question q1 = new Models.Question();
            q1.Text = "sq";
            q1.Type = QuestionTypes.OpenAnswer;
            Models.Question q2 = new Models.Question();
            q2.Text = "tq";
            q2.Type = QuestionTypes.OpenAnswer;
            List<Models.Question> questions = new List<Models.Question> { q, q1, q2 };


            Quiz qu = new Quiz();
            qu.Name = "Test";
            qu.Questions = new List<Models.Question> { q, q1, q2 };
            qu.URL = "123";
           // int id = QuizRepository.SaveItem(qu, 1);
            //.SaveItems(questions, id);
            //AnswerRepository.SaveItems(answers1, (int)questions[0].DBID);
            RuleRepository.SaveItems(rules, 1);

            
            Console.ReadKey();
        }
    }
}
