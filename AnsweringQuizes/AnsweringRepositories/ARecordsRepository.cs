using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnsweringQuizes.AnsweringRepositories
{
    public class ARecordsRepository : IRepository<Records>
    {
        public void DeleteItem(Records item, int ID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
                DbContext.Records.Remove(item);
                DbContext.SaveChanges();
            }
        }

        public void Dispose()
        {

        }

        public Records GetItem(int ID, int ItemID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
                return DbContext.Records.Find(ID);
            }
        }

        public ICollection<Records> GetItems(int ID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
                return DbContext.Records.ToList();
            }
        }

        public int SaveItem(Records item, int ID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
                DbContext.Records.Add(item);
                DbContext.SaveChanges();
                return item.ID;
            }
        }

        public void SaveItems(ICollection<Records> items, int ID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
                DbContext.Records.AddRange(items);
                DbContext.SaveChanges();
            }
        }

        public void UpdateItem(Records newItem, int ID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
                Records question;
                question = DbContext.Records.Find(ID);
                question.IDAnswer = newItem.IDAnswer;
                question.IDQuestion = newItem.IDQuestion;
                question.IDQuiz = newItem.IDQuiz;
                question.IDRespondent = newItem.IDRespondent;
                DbContext.Entry(question).State = System.Data.Entity.EntityState.Modified;
                DbContext.SaveChanges();
            }
        }
    }
}
