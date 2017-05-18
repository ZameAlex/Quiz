using System.Collections.Generic;

namespace DataClasses.Models
{
    public class Rule
    {
        public Rule()
        {
            Answers = new List<Answer>();
        }
        public int? ID { get; set; }
        public int? DBID { get; set; }
        public int NextQuestion { get; set; }
        public int? NextRule { get; set; }
        public Question Next { get; set; }
        public Rule NextRuleObj { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public string AnswersStr { get; set; }
    }
}
