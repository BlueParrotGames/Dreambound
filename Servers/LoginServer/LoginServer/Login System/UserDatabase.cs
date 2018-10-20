using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using BPS.LoginServer.Utility;

namespace BPS.User.Database
{
    public enum LoginState { wrongUsernameOrPassword = 0, SuccelfullLogin = 1, CannotConnectToDatabase = 2, UserAlreadyLoggedIn = 3};
    public enum GamePerks { None = 0, Default, VIP, Developer };

    class UserDatabase
    {
        private static MySqlConnection _sqlConnection = new MySqlConnection(Database.ConnectionString);
        private static MySqlDataReader _sqlDataReader;

        public static LoginData Login(byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteBytes(data);
            buffer.ReadInt();
            buffer.ReadInt();

            string username = buffer.ReadString();
            string password = buffer.ReadString();
            string email = buffer.ReadString();

            return Login(username, password, email);
        }

        public static LoginData Login(string username, string password, string email)
        {
            _sqlConnection.Open();

            MySqlCommand sqlCommand = new MySqlCommand("select * from account where login_name='" + username + "' and password='" + password + "' and email='" + email + "';", _sqlConnection);

            if (_sqlConnection.State == ConnectionState.Closed)
                _sqlConnection.Open();

            _sqlDataReader = sqlCommand.ExecuteReader();

            int count = 0;
            while (_sqlDataReader.Read())
            {
                count++;
            }

            if (count == 1)
            {
                return new LoginData(LoginState.SuccelfullLogin, username, _sqlDataReader.GetInt32("user_id"), (GamePerks)_sqlDataReader.GetInt32("db_account_level"));
            }
            else if (count > 1)
            {
                return new LoginData(LoginState.CannotConnectToDatabase, null, 0, GamePerks.None);
            }
            else
            {
                return new LoginData(LoginState.wrongUsernameOrPassword, null, 0, GamePerks.None);
            }
        }

        public static void Dispose()
        {
            _sqlConnection.Dispose();
        }
    }
}
