using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPS.User.Database
{
    struct LoginCredentials
    {
        public readonly string Username;
        public readonly string Password;
        public readonly string Email;

        public LoginCredentials(string username, string password, string email)
        {
            Username = username;
            Password = password;
            Email = email;
        }
    }
}
