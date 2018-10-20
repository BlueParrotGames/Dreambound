using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPS.User.Database
{
    public struct LoginData
    {
        public readonly LoginState LoginState;
        public readonly string Username;
        public readonly int UserId;
        public readonly GamePerks GamePerks;

        public LoginData(LoginState loginState, string username, int userID, GamePerks gamePerks)
        {
            LoginState = loginState;
            Username = username;
            UserId = userID;
            GamePerks = gamePerks;
        }
    }
}
