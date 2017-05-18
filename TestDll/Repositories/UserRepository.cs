using System;
using System.Collections.Generic;
using System.Linq;
using DataClasses.Models;

namespace DataClasses.Repositories
{
    public class UserRepository : IRepository<Models.User>
    {
        private static Models.User ConvertingFromDB(Users user)
        {
            Models.User result = new User();
            result.ID = user.ID;
            result.Login = user.Login;
            result.Password = user.Password;
            result.Role = user.Role;
            using (QuizRepository q = new QuizRepository())
            {
                result.Quizes = q.GetItems(user.ID);
            }
            
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


        public void SaveItems(ICollection<Models.User> users,int id)
        {
            ICollection<Users> result = new List<Users>();
            foreach(Models.User item in users)
            {
                result.Add(ConvertingToDB(item));
            }
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                DbContext.Users.AddRange(result);
                DbContext.SaveChanges();
                for (int i = 0; i < users.Count; i++)
                    users.ElementAt(i).ID = result.ElementAt(i).ID;
            }
        }

        public int SaveItem(Models.User user,int id)
        {
            Users result = ConvertingToDB(user);
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                DbContext.Users.Add(result);
                DbContext.SaveChanges();
                user.ID = result.ID;
                return result.ID;
            }
        }

        public User GetItem(int ID,int id)
        {
            User result = new User();
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                result = ConvertingFromDB(DbContext.Users.Find(ID));
            }
            return result;
        }

        public ICollection<User> GetItems(int id)
        {
            List<Users> users = new List<Users>();
            using (QuizEntities1 DbContext = new QuizEntities1())
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

        public void DeleteItem(User user,int id)
        {
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                DbContext.Users.Remove(DbContext.Users.Find(user.ID));
                DbContext.SaveChanges();
            }
        }

        public void UpdateItem(User newUser, int ID)
        {
            using (QuizEntities1 DbContext = new QuizEntities1())
            {
                Users user;
                user = DbContext.Users.Find(ID);
                user.Login = newUser.Login;
                user.Password = newUser.Password;
                DbContext.Entry(user).State = System.Data.Entity.EntityState.Modified;
                DbContext.SaveChanges();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
