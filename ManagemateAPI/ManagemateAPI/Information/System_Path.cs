namespace ManagemateAPI.Information
{
    public static class System_Path
    {

        public static string AUTHENTICATION_API = "http://localhost:443/api/";//"http://130.61.92.146:443/api/";//
        public static string HTML_TEMPLATE_PATH_1 = Directory.GetCurrentDirectory() + "/Templates/email.html";//@"\Templates\email.html";//
        public static string HTML_TEMPLATE_PATH_2 = Directory.GetCurrentDirectory() + "/Templates/loginEmail.html";//@"\Templates\loginEmail.html";//
        public static string ENCRYPT_KEYS_PATH = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/keys";//@"F:\Projekty\Magazyn\storage\AuthenticationAPI 2.0\Postgres\keys";//
        public static string PASSWD_PATH = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/passwd";//@"F:\Projekty\Magazyn\storage\AuthenticationAPI 2.0\Postgres\passwd";//
        public static string APPCODE_PATH = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/appcode";//@"F:\Projekty\Magazyn\storage\AuthenticationAPI 2.0\Postgres\appcode";//

    }
}
