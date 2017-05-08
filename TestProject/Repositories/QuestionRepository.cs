using System.Collections.Generic;
using System.Linq;
using TestProject.Models;
namespace TestProject.Repositories
{
    public static class QuestionRepository
    {
        private static Models.Question ConvertingFromDB(Question question)
        {
            Models.Question result = new Models.Question();
            result.DBID = question.ID;
            result.Text = question.Text;
            result.Rules = RuleRepository.GetItems(question.ID);
            result.Answers = AnswerRepository.GetItems(question.ID);
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
            if (question.ID.HasValue)
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


        public static void SaveItems(ICollection<Models.Question> questions, int ID)
        {
            ICollection<Question> result = new List<Question>();
            foreach (Models.Question item in questions)
            {
                result.Add(ConvertingToDB(item, ID));
            }
            using (QuizEntities DbContext = new QuizEntities())
            {
                DbContext.Question.AddRange(result);
                DbContext.SaveChanges();
                for(int i=0;i<questions.Count;i++)
                    questions.ElementAt(i).DBID=result.ElementAt(i).ID;
            }
        }
        public static int SaveItem(Models.Question question, int ID)
        {
            Question result = ConvertingToDB(question, ID);
            using (QuizEntities DbContext = new QuizEntities())
            {
                DbContext.Question.Add(result);
                DbContext.SaveChanges();
                question.DBID = result.ID;
                return result.ID;
            }
        }

        public static Models.Question GetItem(int ID, int QuestionareID)
        {
            Models.Question result = new Models.Question();
            using (QuizEntities DbContext = new QuizEntities())
            {
                result = ConvertingFromDB(DbContext.Questionare.Find(QuestionareID).Question.ElementAt(ID));
            }
            return result;
        }

        public static ICollection<Models.Question> GetItems(int ID)
        {
            List<Question> questions = new List<Question>();
            using (QuizEntities DbContext = new QuizEntities())
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

        public static void DeleteItem(Models.Question Question, int ID)
        {
            using (QuizEntities DbContext = new QuizEntities())
            {
                DbContext.Question.Remove(ConvertingToDB(Question, ID));
                DbContext.SaveChanges();
            }
        }

        public static void UpdateItem(Models.Question newQuestion, int ID)
        {
            using (QuizEntities DbContext = new QuizEntities())
            {
                Question question;
                question = DbContext.Question.Find(ID);
                question.Text = newQuestion.Text;
                DbContext.Entry(question).State = System.Data.Entity.EntityState.Modified;
                DbContext.SaveChanges();
            }
        }
    }
}
