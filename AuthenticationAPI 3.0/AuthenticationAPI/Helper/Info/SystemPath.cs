namespace AuthenticationAPI.Helper
{
    public static class SystemPath
    {
        public static string ENCRYPT_KEYS_PATH = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/keys";//@"F:\Projekty\Magazyn\storage\AuthenticationAPI 2.0\Postgres\keys";//
        public static string PASSWD_PATH = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/passwd";//@"F:\Projekty\Magazyn\storage\AuthenticationAPI 2.0\Postgres\passwd";//
    }
}
