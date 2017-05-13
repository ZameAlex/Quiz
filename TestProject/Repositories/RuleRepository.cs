using System;
using System.Collections.Generic;
using System.Linq;
using TestProject.Models;
using TestProject.AnsweringRepositories;

namespace TestProject.Repositories
{
    public class RuleRepository : IRepository<Rule>
    {
        private static Models.Rule ConvertingFromDB(NextQuestion rule)
        {
            Models.Rule result = new Models.Rule();
            result.DBID = result.ID;
            result.NextQuestion = rule.IDNextQuestion;
            if (rule.IDNextRule.HasValue)
                result.NextRule = (int)rule.IDNextRule;
            else
                rule.IDNextRule = -1;
            result.AnswersStr = rule.Answers;
            return result;
        }

        private static NextQuestion ConvertingToDB(Models.Rule rule, int ID)
        {
            NextQuestion result = new NextQuestion();
            if(rule.ID.HasValue)
                result.ID = (int)rule.DBID;
            result.IDNextQuestion = rule.NextQuestion;
            result.IDNextRule = rule.NextRule;
            result.IDQuestion = ID;
            result.Answers = rule.AnswersStr;
            return result;
        }


        public void SaveItems(ICollection<Models.Rule> rules,int ID)
        {
            ICollection<NextQuestion> result = new List<NextQuestion>();
            foreach(Models.Rule item in rules)
            {
                result.Add(ConvertingToDB(item, ID));
            }
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                DbContext.NextQuestion.AddRange(result);
                DbContext.SaveChanges();
                for (int i = 0; i < rules.Count; i++)
                    rules.ElementAt(i).DBID = result.ElementAt(i).ID;
                for (int i = 0; i < rules.Count - 1; i++)
                {
                    rules.ElementAt(i).NextRule = rules.ElementAt(i + 1).DBID;
                    UpdateItem(rules.ElementAt(i), (int)rules.ElementAt(i).DBID);
                }
                

            }
        }

        public int SaveItem(Models.Rule rule, int ID)
        {
            NextQuestion result = ConvertingToDB(rule, ID);
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                DbContext.NextQuestion.Add(result);
                DbContext.SaveChanges();
                rule.DBID = result.ID;
                return result.ID;
            }
        }

        public Rule GetItem(int ID, int QuestionID)
        {
            Rule result = new Rule();
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                result = ConvertingFromDB(DbContext.Question.Find(QuestionID).NextQuestion.ElementAt(ID));
            }
            return result;
        }

        public ICollection<Rule> GetItems(int ID)
        {
            List<NextQuestion> rules = new List<NextQuestion>();
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                rules = DbContext.Question.Find(ID).NextQuestion.ToList();
            }
            List<Rule> result = new List<Rule>();
            foreach (NextQuestion item in rules)
            {
                result.Add(ConvertingFromDB(item));
            }
            return result;
        }

        public void DeleteItem(Rule rule, int ID)
        {
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                DbContext.NextQuestion.Remove(ConvertingToDB(rule, ID));
                DbContext.SaveChanges();
            }
        }

        public void UpdateItem(Rule newRule, int ID)
        {
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                NextQuestion rule;
                rule = DbContext.NextQuestion.Find(ID);
                rule.IDNextQuestion = newRule.NextQuestion;
                rule.IDNextRule = newRule.NextRule;
                rule.Answers = newRule.AnswersStr;
                DbContext.Entry(rule).State = System.Data.Entity.EntityState.Modified;
                DbContext.SaveChanges();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
