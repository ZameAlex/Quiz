using System.Collections.Generic;

namespace DataClasses.Models
{
    public class User
    {
        public User()
        {
            Quizes = new List<Quiz>();
        }
        public int? ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public ICollection<Quiz> Quizes { get; set; }
    }
}
