﻿using System.Collections.Generic;
using System.Linq;

namespace DataClasses.AnsweringRepositories
{
    public class ARespondentsRepository : IRepository<Respondents>
    {
        public void DeleteItem(Respondents item, int ID)
        {
            using (QuizAnswersEntities1 DbContext = new QuizAnswersEntities1())
            {
                DbContext.Respondents.Remove(item);
                DbContext.SaveChanges();
            }
        }

        public void Dispose()
        {

        }

        public Respondents GetItem(int ID, int ItemID)
        {
            using (QuizAnswersEntities1 DbContext = new QuizAnswersEntities1())
            {
                return DbContext.Respondents.Find(ID);
            }
        }

        public ICollection<Respondents> GetItems(int ID)
        {
            using (QuizAnswersEntities1 DbContext = new QuizAnswersEntities1())
            {
                return DbContext.Respondents.ToList();
            }
        }

        public int SaveItem(Respondents item, int ID)
        {
            using (QuizAnswersEntities1 DbContext = new QuizAnswersEntities1())
            {
                DbContext.Respondents.Add(item);
                DbContext.SaveChanges();
                return item.ID;
            }
        }

        public void SaveItems(ICollection<Respondents> items, int ID)
        {
            using (QuizAnswersEntities1 DbContext = new QuizAnswersEntities1())
            {
                DbContext.Respondents.AddRange(items);
                DbContext.SaveChanges();
            }
        }

        public void UpdateItem(Respondents newItem, int ID)
        {
            using (QuizAnswersEntities1 DbContext = new QuizAnswersEntities1())
            {
                Respondents question;
                question = DbContext.Respondents.Find(ID);
                question.Name = newItem.Name;
                DbContext.Entry(question).State = System.Data.Entity.EntityState.Modified;
                DbContext.SaveChanges();
            }
        }
    }
}
