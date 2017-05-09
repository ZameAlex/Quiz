using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnsweringQuizes.AnsweringRepositories
{
    public class AAnswersRepository : IRepository<Answers>
    {
        public void DeleteItem(Answers item, int ID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
                DbContext.Answers.Remove(item);
                DbContext.SaveChanges();
            }
        }

        public void Dispose()
        {
           
        }

        public Answers GetItem(int ID, int ItemID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
               return DbContext.Answers.Find(ID);
            }
        }

        public ICollection<Answers> GetItems(int ID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
                return DbContext.Answers.ToList();
            }
        }

        public int SaveItem(Answers item, int ID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
                DbContext.Answers.Add(item);
                DbContext.SaveChanges();
                return item.ID;
            }
        }

        public void SaveItems(ICollection<Answers> items, int ID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
                DbContext.Answers.AddRange(items);
                DbContext.SaveChanges();
            }
        }

        public void UpdateItem(Answers newItem, int ID)
        {
            using (QuizAnswersEntities DbContext = new QuizAnswersEntities())
            {
                Answers answer;
                answer = DbContext.Answers.Find(ID);
                answer.Text = newItem.Text;
                answer.Number = newItem.Number;
                DbContext.Entry(answer).State = System.Data.Entity.EntityState.Modified;
                DbContext.SaveChanges();
            }
        }
    }
}
