using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestProject.Models
{
    public class Answer
    {

        public int? ID { get; set; }
        public int? DBID { get; set; }
        public string Text { get; set; }
        public QuestionTypes Type { get; set; }
    }
}
