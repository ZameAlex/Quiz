using System;
using System.Collections.Generic;
using System.Linq;

namespace DataClasses.Repositories
{
    public class QuizRepository : IRepository<Models.Quiz>
    {
        private static Models.Quiz ConvertingFromDB(Questionare questionare)
        {
            Models.Quiz result = new Models.Quiz();
            result.DBID = questionare.ID;
            result.Name = questionare.Name;
            using (QuestionRepository qr = new QuestionRepository())
            {
                result.Questions = qr.GetItems(questionare.ID);
            }
            result.URL = questionare.URL;
            return result;
        }
        private static Questionare ConvertingToDB(Models.Quiz questionare, int ID)
        {
            Questionare result = new Questionare();
            if(questionare.DBID.HasValue)
                result.ID = (int)questionare.DBID;
            result.IDUser = ID;
            result.Name = questionare.Name;
            //result.URL = questionare.URL;
            result.Type = null;
            return result;
        }


        public void SaveItems(ICollection<Models.Quiz> questionares, int ID)
        {
            ICollection<Questionare> result = new List<Questionare>();
            foreach (Models.Quiz item in questionares)
            {
                result.Add(ConvertingToDB(item, ID));
            }
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                DbContext.Questionare.AddRange(result);
                DbContext.SaveChanges();
                for (int i = 0; i < questionares.Count; i++)
                    questionares.ElementAt(i).DBID = result.ElementAt(i).ID;
            }
        }

        public int SaveItem(Models.Quiz questionare, int ID)
        {
            Questionare result = ConvertingToDB(questionare, ID);
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                DbContext.Questionare.Add(result);
                DbContext.SaveChanges();
                questionare.DBID = result.ID;
                return result.ID;
            }
        }

        public Models.Quiz GetItem(int ID, int QuestionareID)
        {
            Models.Quiz result = new Models.Quiz();
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                result = ConvertingFromDB(DbContext.Users.Find(QuestionareID).Questionare.ElementAt(ID));
            }
            return result;
        }

        public Models.Quiz GetItemByURL(string URL)
        {
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                return ConvertingFromDB(DbContext.Questionare.Where(x => x.URL.Trim() == URL.Trim()).First());
            }
        }

        public ICollection<Models.Quiz> GetItems(int ID)
        {
            List<Questionare> questionares = new List<Questionare>();
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                questionares = DbContext.Users.Find(ID).Questionare.ToList();
            }
            List<Models.Quiz> result = new List<Models.Quiz>();
            foreach (Questionare item in questionares)
            {
                result.Add(ConvertingFromDB(item));
            }
            return result;
        }

        public void DeleteItem(Models.Quiz questionare, int ID)
        {
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                Questionare result = DbContext.Questionare.Find(ConvertingToDB(questionare, ID).ID);
                using (QuestionRepository q = new QuestionRepository())
                {
                    foreach (var item in questionare.Questions)
                        q.DeleteItem(item, (int)questionare.DBID);
                }
                    DbContext.Questionare.Remove(result);
                DbContext.SaveChanges();
            }
        }

        public void UpdateItem(Models.Quiz newQuestion, int ID)
        {
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                Questionare questionare;
                questionare = DbContext.Questionare.Find(ID);
                questionare.Name = newQuestion.Name;
                DbContext.Entry(questionare).State = System.Data.Entity.EntityState.Modified;
                DbContext.SaveChanges();
            }
        }

        public void Dispose()
        {
            
        }
    }
}
