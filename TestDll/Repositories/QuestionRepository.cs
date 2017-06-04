using System;
using System.Collections.Generic;
using System.Linq;
using DataClasses.Models;

namespace DataClasses.Repositories
{
    public class QuestionRepository:IRepository<Models.Question>
    {
        private static Models.Question ConvertingFromDB(Question question)
        {
            Models.Question result = new Models.Question();
            result.DBID = question.ID;
            result.Text = question.Text;
            using (RuleRepository rr = new RuleRepository())
            using (AnswerRepository ar = new AnswerRepository())
            {
                result.Rules = rr.GetItems(question.ID);
                result.Answers = ar.GetItems(question.ID);
            }
            
            switch (question.Type)
            {
                case 1:
                    result.Type = QuestionTypes.OneAnswer;
                    break;
                case 2:
                    result.Type = QuestionTypes.SomeAnswers;
                    break;
                case 3:
                    result.Type = QuestionTypes.OpenAnswer;
                    break;
                default:
                    break;
            }
            return result;
        }
        private static Question ConvertingToDB(Models.Question question, int ID)
        {
            Question result = new Question();
            if (question.DBID.HasValue)
                result.ID = (int)question.DBID;
            result.IDQuestionare = ID;
            result.Text = question.Text;
            switch (question.Type)
            {
                case QuestionTypes.OneAnswer:
                    result.Type = 1;
                    break;
                case QuestionTypes.SomeAnswers:
                    result.Type = 2;
                    break;
                case QuestionTypes.OpenAnswer:
                    result.Type = 3;
                    break;
                default:
                    break;
            }
            return result;
        }


        public void SaveItems(ICollection<Models.Question> questions, int ID)
        {
            ICollection<Question> result = new List<Question>();
            foreach (Models.Question item in questions)
            {
                result.Add(ConvertingToDB(item, ID));
            }
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                DbContext.Question.AddRange(result);
                DbContext.SaveChanges();
                for(int i=0;i<questions.Count;i++)
                    questions.ElementAt(i).DBID=result.ElementAt(i).ID;
            }
        }
        public int SaveItem(Models.Question question, int ID)
        {
            Question result = ConvertingToDB(question, ID);
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                DbContext.Question.Add(result);
                DbContext.SaveChanges();
                question.DBID = result.ID;
                return result.ID;
            }
        }

        public Models.Question GetItem(int ID, int QuestionareID)
        {
            Models.Question result = new Models.Question();
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                result = ConvertingFromDB(DbContext.Questionare.Find(QuestionareID).Question.ElementAt(ID));
            }
            return result;
        }

        public ICollection<Models.Question> GetItems(int ID)
        {
            List<Question> questions = new List<Question>();
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                questions = DbContext.Questionare.Find(ID).Question.ToList();
            }
            List<Models.Question> result = new List<Models.Question>();
            foreach (Question item in questions)
            {
                result.Add(ConvertingFromDB(item));
            }
            return result;
        }

        public void DeleteItem(Models.Question question, int ID)
        {
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                Question result = DbContext.Question.Find(question.DBID);
                if(result.Type!=3)
                    using (AnswerRepository an = new AnswerRepository())
                    {
                        foreach (var item in question.Answers)
                            an.DeleteItem(item, (int)question.DBID);
                    }
                        DbContext.Question.Remove(result);
                DbContext.SaveChanges();
            }
        }

        public void UpdateItem(Models.Question newQuestion, int ID)
        {
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                Question question;
                question = DbContext.Question.Find(ID);
                question.Text = newQuestion.Text;
                question.Type = (int)newQuestion.Type + 1;
                DbContext.Entry(question).State = System.Data.Entity.EntityState.Modified;
                DbContext.SaveChanges();
            }
        }

        public void Dispose()
        {
           
        }
    }
}
