namespace BPS.User.Database
{
    class Database
    {
        public const string ServerIP = "178.84.28.2";
        public const string ServerPort = "3350";
        public const string DatabaseName = "auth";
        public const string Uid = "remote";
        public const string Password = "mf8k66DwgDa48GLx2Vdu5FqLN8vNmeHBuuKMBbcCw5VZYmQDr9ZNPcdmnZZNd2v894d2XGQ5RY5qZrZD8nNf4xNDqfmAEXae2np8rDKzBvHdpXrsesG8s3avRVbUs9W42deXmUtaujhVj9Sr64ttjZTH4M8BEJZwnYX4d4TYYMvAJUjTRMLT6WZz7JGGZcY5FpjvHD2fkENSNGEpKkrYNrXNDkXwTR6s2LM9YthTXQwYStSTRaWeXRCgNuVdVSrd";

        public const string ConnectionString = "server=" + ServerIP + ";port=" + ServerPort + ";database=" + DatabaseName + ";uid=" + Uid + ";password=" + Password + ";SslMode=none";
    }
}
