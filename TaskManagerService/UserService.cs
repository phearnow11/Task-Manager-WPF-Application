using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerRepo;
using TaskManagerRepo.Models;

namespace TaskManagerService
{
    public class UserService
    {
        public UserService() { }

        public User? Login(string username, string password)
        {
            UserRepo userRepo = new UserRepo();
            return userRepo.GetUserByUsernamePassword(username, password);
        }
    }
}
