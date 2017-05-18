
namespace DataClasses.Models
{
    public class Answer
    {

        public int? ID { get; set; }
        public int? DBID { get; set; }
        public string Text { get; set; }
        public QuestionTypes Type { get; set; }
    }
}
