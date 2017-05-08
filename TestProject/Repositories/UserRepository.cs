using System.Collections.Generic;
using System.Linq;
using TestProject.Models;

namespace TestProject.Repositories
{
    public static class UserRepository
    {
        private static Models.User ConvertingFromDB(Users user)
        {
            Models.User result = new User();
            result.ID = user.ID;
            result.Login = user.Login;
            result.Password = user.Password;
            result.Role = user.Role;
            result.Quizes = QuizRepository.GetItems(user.ID);
            return result;
        }
        private static Users ConvertingToDB(Models.User user)
        {
            Users result = new Users();
            if (user.ID.HasValue)
                result.ID = (int)user.ID;
            result.Login = user.Login;
            result.Password = user.Password;
            result.Role = user.Role;
            return result;
        }


        public static void SaveItems(ICollection<Models.User> users)
        {
            ICollection<Users> result = new List<Users>();
            foreach(Models.User item in users)
            {
                result.Add(ConvertingToDB(item));
            }
            using (QuizEntities DbContext = new QuizEntities())
            {
                DbContext.Users.AddRange(result);
                DbContext.SaveChanges();
                for (int i = 0; i < users.Count; i++)
                    users.ElementAt(i).ID = result.ElementAt(i).ID;
            }
        }

        public static int SaveItem(Models.User user)
        {
            Users result = ConvertingToDB(user);
            using (QuizEntities DbContext = new QuizEntities())
            {
                DbContext.Users.Add(result);
                DbContext.SaveChanges();
                user.ID = result.ID;
                return result.ID;
            }
        }

        public static User GetItem(int ID)
        {
            User result = new User();
            using (QuizEntities DbContext = new QuizEntities())
            {
                result = ConvertingFromDB(DbContext.Users.Find(ID));
            }
            return result;
        }

        public static ICollection<User> GetItems()
        {
            List<Users> users = new List<Users>();
            using (QuizEntities DbContext = new QuizEntities())
            {
                users = DbContext.Users.ToList();
            }
            List<User> result = new List<User>();
            foreach(Users item in users)
            {
                result.Add(ConvertingFromDB(item));
            }
            return result;
        }

        public static void DeleteItem(User user)
        {
            using (QuizEntities DbContext = new QuizEntities())
            {
                DbContext.Users.Remove(DbContext.Users.Find(user.ID));
                DbContext.SaveChanges();
            }
        }

        public static void UpdateItem(User newUser, int ID)
        {
            using (QuizEntities DbContext = new QuizEntities())
            {
                Users user;
                user = DbContext.Users.Find(ID);
                user.Login = newUser.Login;
                user.Password = newUser.Password;
                DbContext.Entry(user).State = System.Data.Entity.EntityState.Modified;
                DbContext.SaveChanges();
            }
        }
    }
}
