using System.Collections.Generic;

namespace DataClasses.Models
{
    public class Quiz
    {
        public Quiz()
        {
            Questions = new List<Question>();
        }
        public int? ID { get; set; }
        public int? DBID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
