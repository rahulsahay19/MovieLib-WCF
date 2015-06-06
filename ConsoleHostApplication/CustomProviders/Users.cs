
using System.Collections.Generic;

namespace ConsoleHostApplication.CustomProviders
{
    public class Users : List<User>
    {
        public static Users Load()
        {
            Users users = new Users();

            users.Add(new User()
            {
                UserName = "rahul",
                Roles = new List<string>() { "user", "admin" }
            });
            users.Add(new User()
            {
                UserName = "nivi",
                Roles = new List<string>() { "user" }
            });
            users.Add(new User()
            {
                UserName = "aadi",
                Roles = new List<string>() { "user" }
            });

            return users;
        }

        public User FindUser(string userName)
        {
            return Find(u => u.UserName == userName);
        }
    }

    public class User
    {
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
        public bool IsInRole(string role)
        {
            return Roles.Contains(role);
        }
    }    
}
