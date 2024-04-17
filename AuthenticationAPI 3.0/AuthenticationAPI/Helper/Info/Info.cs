namespace AuthenticationAPI.Helper
{
    public static class Info
    {

        //ERROR

        public static string _1_SESSION_NOT_FOUND = "session not found";

        public static string _2_ENCRYPTION_ERROR = "encryption error";
        public static string _3_DECRYPTION_ERROR = "decryption error";

        public static string _4_LOGIN_IN_USE = "login in use";
        public static string _5_EMAIL_IN_USE = "e-mail in use";
        public static string _6_ADD_USER_ERROR = "error while adding user";

        public static string _7_USER_NOT_FOUND = "user not found";
        public static string _8_PASSWORD_ERROR = "incorrect password";        

        public static string _9_CONFIRM_CODE_TIME_ERROR = "confirmation code has expired";
        public static string _10_CONFIRM_CODE_ATTEMPT_ERROR = "too many incorrect attempts";
        public static string _11_CONFIRM_CODE_INVALID = "incorrect confirmation code";
        public static string _12_CONFIRM_CODE_NOT_FOUND = "confirmation code not found";

        public static string _13_VALIDATION_TOKEN_ERROR = "validation token error";

        public static string _14_NULL_ERROR = "null error";

        public static string _17_SESSION_EXPIRED = "session has expired";

        public static string _20_CONFIRM_CODE_DELAY_ERROR = "too short time period between sending e-mails";

        public static string _21_INVALID_PASSWORD_ERROR = "Password is invalid";

        //SUCCESS

        public static string CHANGE_PASSWD_SUCCESSFUL = "successfully changed password";
        public static string CHANGE_USERNAME_SUCCESSFUL = "successfully changed login";
        public static string CHANGE_EMAIL_SUCCESSFULL = "successfully changed e-mail";
        public static string VALID_PASSWORD = "Valid password";

        public static string DELETED_EXPIRED_CODES = "successfully performed operation";

        public static string FREE_ACCOUNT = "free account data";

        public static string SESSION_ACTIVE = "session is active";
        public static string SESSION_CLOSED = "successfully logged out";

    }
}
