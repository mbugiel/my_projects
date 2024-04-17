using AuthenticationAPI.Helper;

namespace AuthenticationAPI.Controllers
{
    public class ResponseHandler
    {
        public static ApiResponse GetExceptionResponse(Exception ex)
        {

            ApiResponse response = new ApiResponse();
            
            response.Code = ex.Message;
            response.ResponseData = null;

            switch (ex.Message)
            {
                case "1":
                    response.Message = Info._1_SESSION_NOT_FOUND; break;

                case "2":
                    response.Message = Info._2_ENCRYPTION_ERROR; break;

                case "3":
                    response.Message = Info._3_DECRYPTION_ERROR; break;

                case "4":
                    response.Message = Info._4_LOGIN_IN_USE; break;

                case "5":
                    response.Message = Info._5_EMAIL_IN_USE; break;

                case "6":
                    response.Message = Info._6_ADD_USER_ERROR; break;

                case "7":
                    response.Message = Info._7_USER_NOT_FOUND; break;

                case "8":
                    response.Message = Info._8_PASSWORD_ERROR; break;

                case "9":
                    response.Message = Info._9_CONFIRM_CODE_TIME_ERROR; break;

                case "10":
                    response.Message = Info._10_CONFIRM_CODE_ATTEMPT_ERROR; break;

                case "11":
                    response.Message = Info._11_CONFIRM_CODE_INVALID; break;

                case "12":
                    response.Message = Info._12_CONFIRM_CODE_NOT_FOUND; break;

                case "13":
                    response.Message = Info._13_VALIDATION_TOKEN_ERROR; break;

                case "14":
                    response.Message = Info._14_NULL_ERROR; break;

                case "17":
                    response.Message = Info._17_SESSION_EXPIRED; break;

                case "20":
                    response.Message = Info._20_CONFIRM_CODE_DELAY_ERROR; break;

                case "21":
                    response.Message = Info._21_INVALID_PASSWORD_ERROR; break;

                default:
                    response.Message = "Failure";
                    break;
            }

            return response;
        }

        public static ApiResponse GetAppResponse(ResponseType type, object? contract)
        {
            ApiResponse response;

            response = new ApiResponse { ResponseData = contract };
            switch (type)
            {
                case ResponseType.Success:
                    response.Code = "0";
                    response.Message = "Success";

                    break;
                case ResponseType.NotFound:
                    response.Code = "2";
                    response.Message = "Not Found";

                    break;
            }
            return response;
        }
    }
}
