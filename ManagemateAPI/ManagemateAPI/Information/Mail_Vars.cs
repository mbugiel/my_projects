namespace ManagemateAPI.Information
{
    public static class Mail_Vars
    {
        public static string ADD_USER_SUBJECT = "Weryfikacja adresu e-mail";
        public static string TWO_STEP_LOGIN_SUBJECT = "Logowanie dwuetapowe";
        public static string MAIL_SidE_TEXT = "Dodatkowy tekst na potrzeby niektórych serwerów.";
        public static string VALidATION_CODE = "#";


        private static string HTML_1;
        private static string HTML_2;

        public static string GetTemplate1()
        {
            if (HTML_1 == null || HTML_1.Length == 0) HTML_1 = File.ReadAllText(System_Path.HTML_TEMPLATE_PATH_1);
            return HTML_1;
        }

        public static string GetTemplate2()
        {
            if (HTML_2 == null || HTML_2.Length == 0) HTML_2 = File.ReadAllText(System_Path.HTML_TEMPLATE_PATH_2);
            return HTML_2;
        }

    }
}
