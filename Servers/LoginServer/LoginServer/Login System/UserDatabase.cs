using System.Data;

using MySql.Data.MySqlClient;
using BPS.LoginServer.Utility;
using BPS.Debugging;

namespace BPS.User.Database
{
    public enum LoginState { wrongUsernameOrPassword = 0, SuccelfullLogin = 1, CannotConnectToDatabase = 2, UserAlreadyLoggedIn = 3 };
    public enum GamePerks { None = 0, Default, VIP, Developer };

    class UserDatabase
    {
        private static MySqlConnection _sqlConnection = new MySqlConnection(Database.ConnectionString);
        private static MySqlDataReader _sqlDataReader;

        public static LoginData Login(byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            return Login(buffer.ReadString(), buffer.ReadString());
        }

        public static LoginData Login(string email, string password)
        {
            if (_sqlDataReader != null)
                _sqlDataReader.Close();

            MySqlCommand sqlCommand = new MySqlCommand("select * from account where email='" + email + "' and password='" + password + "';", _sqlConnection);

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
                return new LoginData(LoginState.SuccelfullLogin, _sqlDataReader.GetString("login_name"), _sqlDataReader.GetInt32("user_id"), (GamePerks)_sqlDataReader.GetInt32("db_account_level"));
            }
            else if (count > 1)
            {
                return new LoginData(LoginState.CannotConnectToDatabase, "", 0, GamePerks.None);
            }
            else
            {
                return new LoginData(LoginState.wrongUsernameOrPassword, "", 0, GamePerks.None);
            }
        }
        public static void Dispose()
        {
            _sqlConnection.Dispose();
        }
    }
}
