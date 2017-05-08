﻿using System.Collections.Generic;
using System.Linq;

namespace TestProject.Repositories
{
    public static class QuizRepository
    {
        private static Models.Quiz ConvertingFromDB(Questionare questionare)
        {
            Models.Quiz result = new Models.Quiz();
            result.DBID = questionare.ID;
            result.Name = questionare.Name;
            result.Questions = QuestionRepository.GetItems(questionare.ID);
            result.URL = questionare.URL;
            return result;
        }
        private static Questionare ConvertingToDB(Models.Quiz questionare, int ID)
        {
            Questionare result = new Questionare();
            if(questionare.ID.HasValue)
                result.ID = (int)questionare.DBID;
            result.IDUser = ID;
            result.Name = questionare.Name;
            return result;
        }


        public static void SaveItems(ICollection<Models.Quiz> questionares, int ID)
        {
            ICollection<Questionare> result = new List<Questionare>();
            foreach (Models.Quiz item in questionares)
            {
                result.Add(ConvertingToDB(item, ID));
            }
            using (QuizEntities DbContext = new QuizEntities())
            {
                DbContext.Questionare.AddRange(result);
                DbContext.SaveChanges();
                for (int i = 0; i < questionares.Count; i++)
                    questionares.ElementAt(i).DBID = result.ElementAt(i).ID;
            }
        }

        public static int SaveItem(Models.Quiz questionare, int ID)
        {
            Questionare result = ConvertingToDB(questionare, ID);
            using (QuizEntities DbContext = new QuizEntities())
            {
                DbContext.Questionare.Add(result);
                DbContext.SaveChanges();
                questionare.DBID = result.ID;
                return result.ID;
            }
        }

        public static Models.Quiz GetItem(int ID, int QuestionareID)
        {
            Models.Quiz result = new Models.Quiz();
            using (QuizEntities DbContext = new QuizEntities())
            {
                result = ConvertingFromDB(DbContext.Users.Find(QuestionareID).Questionare.ElementAt(ID));
            }
            return result;
        }

        public static ICollection<Models.Quiz> GetItems(int ID)
        {
            List<Questionare> questionares = new List<Questionare>();
            using (QuizEntities DbContext = new QuizEntities())
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

        public static void DeleteItem(Models.Quiz questionare, int ID)
        {
            using (QuizEntities DbContext = new QuizEntities())
            {
                DbContext.Questionare.Remove(ConvertingToDB(questionare, ID));
                DbContext.SaveChanges();
            }
        }

        public static void UpdateItem(Models.Quiz newQuestion, int ID)
        {
            using (QuizEntities DbContext = new QuizEntities())
            {
                Questionare questionare;
                questionare = DbContext.Questionare.Find(ID);
                questionare.Name = newQuestion.Name;
                DbContext.Entry(questionare).State = System.Data.Entity.EntityState.Modified;
                DbContext.SaveChanges();
            }
        }
    }
}
