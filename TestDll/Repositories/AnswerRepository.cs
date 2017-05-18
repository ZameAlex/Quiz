using System.Collections.Generic;
using System.Linq;
using DataClasses.Models;

namespace DataClasses.Repositories
{
    public class AnswerRepository : IRepository<Answer>
    {
        
        private static Models.Answer ConvertingFromDB(Answers answer)
        {
            Models.Answer result = new Answer();
            result.DBID = answer.ID;
            result.Text = answer.Text;
            switch (answer.Type)
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
        private static Answers ConvertingToDB(Models.Answer answer, int ID)
        {
            Answers result = new Answers();
            if(answer.ID.HasValue)
                result.ID = (int)answer.DBID;
            result.IDQuestion = ID;
            result.Text = answer.Text;
            switch (answer.Type)
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


        public void SaveItems(ICollection<Models.Answer> answers, int ID)
        {
            ICollection<Answers> result = new List<Answers>();
            foreach(Models.Answer item in answers)
            {
                result.Add(ConvertingToDB(item, ID));
            }
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                DbContext.Answers.AddRange(result);
                DbContext.SaveChanges();
                for (int i = 0; i < answers.Count; i++)
                    answers.ElementAt(i).DBID = result.ElementAt(i).ID;
            }
        }

        public int SaveItem(Models.Answer answer, int ID)
        {
            Answers result = ConvertingToDB(answer, ID);
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                DbContext.Answers.Add(result);
                DbContext.SaveChanges();
                answer.DBID = result.ID;
                return result.ID;
            }
        }


        public Answer GetItem(int ID, int QuestionID)
        {
            Answer result = new Answer();
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                result = ConvertingFromDB(DbContext.Question.Find(QuestionID).Answers.ElementAt(ID));
            }
            return result;
        }

        public ICollection<Answer> GetItems(int ID)
        {
            List<Answers> answers = new List<Answers>();
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                answers = DbContext.Question.Find(ID).Answers.ToList();
            }
            List<Answer> result = new List<Answer>();
            foreach(Answers item in answers)
            {
                result.Add(ConvertingFromDB(item));
            }
            return result;
        }

        public void DeleteItem(Answer answer, int ID)
        {
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                DbContext.Answers.Remove(ConvertingToDB(answer,ID));
                DbContext.SaveChanges();
            }
        }

        public void UpdateItem(Answer newAnswer, int ID)
        {
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                Answers answer;
                answer = DbContext.Answers.Find(ID);
                answer.Text = newAnswer.Text;
                DbContext.Entry(answer).State = System.Data.Entity.EntityState.Modified;
                DbContext.SaveChanges();
            }
        }

        public void Dispose()
        {
            
        }
    }
}
