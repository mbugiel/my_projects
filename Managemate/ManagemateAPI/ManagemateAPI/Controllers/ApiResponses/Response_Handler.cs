using ManagemateAPI.Information;

namespace ManagemateAPI.Controllers
{
    public class Response_Handler
    {
        public static Api_Response GetExceptionResponse(Exception ex)
        {

            Api_Response response = new Api_Response();

            response.code = ex.Message;
            response.responseData = null;

            switch (ex.Message)
            {
                case "1":
                    response.message = Info._1_SESSION_NOT_FOUND; break;

                case "2":
                    response.message = Info._2_ENCRYPTION_ERROR; break;

                case "3":
                    response.message = Info._3_DECRYPTION_ERROR; break;

                case "4":
                    response.message = Info._4_LOGIN_IN_USE; break;

                case "5":
                    response.message = Info._5_EMAIL_IN_USE; break;

                case "6":
                    response.message = Info._6_ADD_USER_ERROR; break;

                case "7":
                    response.message = Info._7_USER_NOT_FOUND; break;

                case "8":
                    response.message = Info._8_PASSWORD_ERROR; break;

                case "9":
                    response.message = Info._9_CONFIRM_CODE_TIME_ERROR; break;

                case "10":
                    response.message = Info._10_CONFIRM_CODE_ATTEMPT_ERROR; break;

                case "11":
                    response.message = Info._11_CONFIRM_CODE_INVALid; break;

                case "12":
                    response.message = Info._12_CONFIRM_CODE_NOT_FOUND; break;

                case "13":
                    response.message = Info._13_NO_ITEMS_ON_RECEIPT; break;

                case "14":
                    response.message = Info._14_NULL_ERROR; break;

                case "15":
                    response.message = Info._15_CONFIRM_CODE_READ_TEMPLATE_ERROR; break;

                case "16":
                    response.message = Info._16_SERVER_RESPONSE_ERROR; break;

                case "17":
                    response.message = Info._17_SESSION_EXPIRED; break;

                case "18":
                    response.message = Info._18_DUPLICATE_ERROR; break;

                case "19":
                    response.message = Info._19_OBJECT_NOT_FOUND; break;

                case "20":
                    response.message = Info._20_CONFIRM_CODE_DELAY_ERROR; break;

                case "21":
                    response.message = Info._21_INVALID_PASSWORD_ERROR; break;

                case "22":
                    response.message = Info._22_ADD_RECEIPT_ERROR; break;

                case "23":
                    response.message = Info._23_COMPANY_EXISTS_ERROR; break;

                case "24":
                    response.message = Info._24_TOO_MANY_RETURNED_ERROR; break;

                case "25":
                    response.message = Info._25_INCORRECT_ITEM_TRADING_ERROR; break;

                case "26":
                    response.message = Info._26_FILE_READ_ERROR; break;

                case "27":
                    response.message = Info._27_FOLDER_CREATE_ERROR; break;

                case "28":
                    response.message = Info._28_JSON_ERROR; break;

                case "29":
                    response.message = Info._29_FILE_TOO_LARGE; break;

                default:
                    response.message = "Failure";
                    break;
            }

            return response;
        }

        public static Api_Response GetAppResponse(ResponseType type, object? contract)
        {
            Api_Response response;

            response = new Api_Response { responseData = contract };
            switch (type)
            {
                case ResponseType.Success:
                    response.code = "0";
                    response.message = "Success";

                    break;
                case ResponseType.NotFound:
                    response.code = "2";
                    response.message = "Not Found";

                    break;
            }
            return response;
        }
    }
}
