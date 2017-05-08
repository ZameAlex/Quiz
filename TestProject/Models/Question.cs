using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Models
{
    public enum QuestionTypes { OneAnswer, SomeAnswers, OpenAnswer }
    public class Question
    {

        public Question()
        {
            Answers = new List<Answer>();
            Rules = new List<Rule>();
        }
        #region Need for DB
        public int? ID { get; set; }
        public int? DBID { get; set; }
        public QuestionTypes Type { get; set; }
        public string Text { get; set; }
        #endregion Need For DB

        #region Not necessary
        public ICollection<Answer> Answers { get; set; }
        public ICollection<Rule> Rules { get; set; }
        #endregion Not necessary
    }
}
