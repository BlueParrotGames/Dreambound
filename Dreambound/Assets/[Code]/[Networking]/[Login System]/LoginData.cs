using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreambound.Networking.LoginSystem
{
    public enum LoginState { wrongUsernameOrPassword = 0, SuccelfullLogin = 1, CannotConnectToDatabase = 2, UserAlreadyLoggedIn = 3 };
    public enum GamePerks { None = 0, Default, VIP, Developer };

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
