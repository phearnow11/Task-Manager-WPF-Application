using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerRepo.Models;

namespace TaskManagerRepo
{
    public class UserRepo
    {
        public UserRepo() { }

        public User? GetUserByUsernamePassword(string username, string password)
        {
            TaskManagerContext context = new TaskManagerContext();
            List<User> users = context.Users.Where(u => u.Username == username && String.Compare(u.Password, password) == 0).ToList();
            if(users.Count > 0) { return users[0]; }
            return null;
        }
    }
}
