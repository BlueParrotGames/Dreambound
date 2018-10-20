using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreambound.Networking.LoginSystem
{
    public static class UserData
    {
        public static string Username { get; internal set; }
        public static int ID { get; internal set; }
        public static GamePerks Perks { get; internal set; }

        public static void SetUserData(LoginData loginData)
        {
            Username = loginData.Username;
            ID = loginData.UserId;
            Perks = loginData.GamePerks;
        }
    }
}