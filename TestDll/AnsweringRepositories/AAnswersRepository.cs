using System.Collections.Generic;
using System.Linq;


namespace DataClasses.AnsweringRepositories
{
    public class AAnswersRepository : IRepository<Answers>
    {
        public void DeleteItem(Answers item, int ID)
        {
            using (QuizAnswersEntities1 DbContext = new QuizAnswersEntities1())
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
            using (QuizAnswersEntities1 DbContext = new QuizAnswersEntities1())
            {
               return DbContext.Answers.Find(ID);
            }
        }

        public ICollection<Answers> GetItems(int ID)
        {
            using (QuizAnswersEntities1 DbContext = new QuizAnswersEntities1())
            {
                return DbContext.Answers.ToList();
            }
        }

        public int SaveItem(Answers item, int ID)
        {
            using (QuizAnswersEntities1 DbContext = new QuizAnswersEntities1())
            {
                DbContext.Answers.Add(item);
                DbContext.SaveChanges();
                return item.ID;
            }
        }

        public void SaveItems(ICollection<Answers> items, int ID)
        {
            using (QuizAnswersEntities1 DbContext = new QuizAnswersEntities1())
            {
                DbContext.Answers.AddRange(items);
                DbContext.SaveChanges();
            }
        }

        public void UpdateItem(Answers newItem, int ID)
        {
            using (QuizAnswersEntities1 DbContext = new QuizAnswersEntities1())
            {
                Answers answer;
                answer = DbContext.Answers.Where(x => x.Number == ID).First();
                answer.Text = newItem.Text;
                DbContext.Entry(answer).State = System.Data.Entity.EntityState.Modified;
                DbContext.SaveChanges();
            }
        }
    }
}
