using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnsweringQuizes.AnsweringRepositories
{
    public class AQuizRepository : IRepository<Quizes>
    {
        public void DeleteItem(Quizes item, int ID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
                DbContext.Quizes.Remove(item);
                DbContext.SaveChanges();
            }
        }

        public void Dispose()
        {

        }

        public Quizes GetItem(int ID, int ItemID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
                return DbContext.Quizes.Find(ID);
            }
        }

        public ICollection<Quizes> GetItems(int ID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
                return DbContext.Quizes.ToList();
            }
        }

        public int SaveItem(Quizes item, int ID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
                DbContext.Quizes.Add(item);
                DbContext.SaveChanges();
                return item.ID;
            }
        }

        public void SaveItems(ICollection<Quizes> items, int ID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
                DbContext.Quizes.AddRange(items);
                DbContext.SaveChanges();
            }
        }

        public void UpdateItem(Quizes newItem, int ID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
                Quizes question;
                question = DbContext.Quizes.Find(ID);
                question.Name = newItem.Name;
                DbContext.Entry(question).State = System.Data.Entity.EntityState.Modified;
                DbContext.SaveChanges();
            }
        }
    }
}
