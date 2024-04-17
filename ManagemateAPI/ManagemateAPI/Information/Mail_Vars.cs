namespace ManagemateAPI.Information
{
    public static class Mail_Vars
    {
        public static string ADD_USER_SUBJECT = "Weryfikacja adresu e-mail";
        public static string TWO_STEP_LOGIN_SUBJECT = "Logowanie dwuetapowe";
        public static string MAIL_SidE_TEXT = "Dodatkowy tekst na potrzeby niektórych serwerów.";
        public static string VALidATION_CODE = "bYUITVyuvvyuVYU6756vkhv76ugyUGYACF2782167B33D6E31AF6AEAF700A1CABA32555F55372150D353113C9F5150F7281A8967C79180FE88E7143EEF1A1BDADB50F54C6476547148CEC1BF7F6B6D7FFB69303DDAF06EE9B11BEFA099FB1DD64F99A24EA236006B9A9790B9A4141CDE8D86085B2D5C71A0954E4CB17E05BE590935FFB87F6768E21C828D4A12BCD4B5747318D95C2CA603085AE7591E67E5B61491919358073A8575AF4D44F36C9C3B2517B1D605222C92AAD96A5BB3E914CFDCD10684A975DE943B7B104010B27754BDC91694ADA3DDD6AE39EB5831104FCF38530A7B922AC626793F66A2070239B3BC35C47D33B360E6A2BDE946BF1FBF09925134CE6AD51012F660D47A2B44DE81LL8901huiREDYXD+5Ypo+9jgvhc89+JG233278F";


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
