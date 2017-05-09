using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnsweringQuizes.AnsweringRepositories
{
    public class AQuestionRepository : IRepository<Questions>
    {
        public void DeleteItem(Questions item, int ID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
                DbContext.Questions.Remove(item);
                DbContext.SaveChanges();
            }
        }

        public void Dispose()
        {

        }

        public Questions GetItem(int ID, int ItemID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
                return DbContext.Questions.Find(ID);
            }
        }

        public ICollection<Questions> GetItems(int ID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
                return DbContext.Questions.ToList();
            }
        }

        public int SaveItem(Questions item, int ID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
                DbContext.Questions.Add(item);
                DbContext.SaveChanges();
                return item.ID;
            }
        }

        public void SaveItems(ICollection<Questions> items, int ID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
                DbContext.Questions.AddRange(items);
                DbContext.SaveChanges();
            }
        }

        public void UpdateItem(Questions newItem, int ID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
                Questions question;
                question = DbContext.Questions.Find(ID);
                question.Text = newItem.Text;
                DbContext.Entry(question).State = System.Data.Entity.EntityState.Modified;
                DbContext.SaveChanges();
            }
        }
    }
}
