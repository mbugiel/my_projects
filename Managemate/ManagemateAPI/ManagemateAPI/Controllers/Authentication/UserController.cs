using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using ManagemateAPI.Management.M_Session.Input_Objects;
using ManagemateAPI.Management.M_User.Input_Objects;
using ManagemateAPI.Information;

namespace ManagemateAPI.Controllers.Authentication
{

    [ApiController]
    public class UserController : ControllerBase
    {


        [Route("api/UserExist")]
        [HttpPost]
        public async Task<IActionResult> UserExist([FromBody] User_Exist_Data newUserCheck)
        {
            if (newUserCheck == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {

                try
                {

                    using (var httpClient = new HttpClient())
                    {

                        StringContent content = new StringContent(JsonSerializer.Serialize(newUserCheck), Encoding.UTF8, "application/json");

                        using (var authResponse = await httpClient.PostAsync(System_Path.AUTHENTICATION_API + "UserExist", content))
                        {

                            var responseAsString = await authResponse.Content.ReadAsStringAsync();

                            if (responseAsString == null)
                            {
                                throw new Exception("14");//_14_NULL_ERROR
                            }
                            else
                            {

                                Api_Response apiResponse = JsonSerializer.Deserialize<Api_Response>(responseAsString);


                                if (apiResponse != null)
                                {

                                    if (apiResponse.code.Equals("0"))
                                    {
                                        ResponseType responseType = ResponseType.Success;
                                        return Ok(Response_Handler.GetAppResponse(responseType, apiResponse.responseData));
                                    }
                                    else
                                    {
                                        throw new Exception(apiResponse.code);
                                    }

                                }
                                else
                                {
                                    throw new Exception("16");//_16_SERVER_RESPONSE_ERROR
                                }

                            }


                        }



                    }

                }
                catch (Exception e)
                {

                    return BadRequest(Response_Handler.GetExceptionResponse(e));
                }

            }

        }




        [Route("api/HasTwoStepLogin")]
        [HttpPost]
        public async Task<IActionResult> HasTwoStepLogin([FromBody] Has_Two_Step_Login_Data loginData)
        {
            if (loginData == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {

                try
                {

                    using (var httpClient = new HttpClient())
                    {

                        StringContent content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

                        using (var authResponse = await httpClient.PostAsync(System_Path.AUTHENTICATION_API + "HasTwoStepLogin", content))
                        {

                            var responseAsString = await authResponse.Content.ReadAsStringAsync();

                            if (responseAsString == null)
                            {
                                throw new Exception("14");//_14_NULL_ERROR
                            }
                            else
                            {

                                Api_Response apiResponse = JsonSerializer.Deserialize<Api_Response>(responseAsString);


                                if (apiResponse != null)
                                {

                                    if (apiResponse.code.Equals("0"))
                                    {
                                        ResponseType responseType = ResponseType.Success;
                                        return Ok(Response_Handler.GetAppResponse(responseType, apiResponse.responseData));
                                    }
                                    else
                                    {
                                        throw new Exception(apiResponse.code);
                                    }

                                }
                                else
                                {
                                    throw new Exception("16");//_16_SERVER_RESPONSE_ERROR
                                }

                            }


                        }



                    }

                }
                catch (Exception e)
                {

                    return BadRequest(Response_Handler.GetExceptionResponse(e));
                }

            }


        }




        [Route("api/Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Login_Data loginData)
        {
            if (loginData == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {

                try
                {

                    using (var httpClient = new HttpClient())
                    {

                        StringContent content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

                        using (var authResponse = await httpClient.PostAsync(System_Path.AUTHENTICATION_API + "Login", content))
                        {

                            var responseAsString = await authResponse.Content.ReadAsStringAsync();

                            if (responseAsString == null)
                            {
                                throw new Exception("14");//_14_NULL_ERROR
                            }
                            else
                            {

                                Api_Response apiResponse = JsonSerializer.Deserialize<Api_Response>(responseAsString);


                                if (apiResponse != null)
                                {

                                    if (apiResponse.code.Equals("0"))
                                    {
                                        ResponseType responseType = ResponseType.Success;
                                        return Ok(Response_Handler.GetAppResponse(responseType, apiResponse.responseData));
                                    }
                                    else
                                    {
                                        throw new Exception(apiResponse.code);
                                    }

                                }
                                else
                                {
                                    throw new Exception("16");//_16_SERVER_RESPONSE_ERROR
                                }

                            }


                        }



                    }

                }
                catch (Exception e)
                {

                    return BadRequest(Response_Handler.GetExceptionResponse(e));
                }

            }


        }




        [Route("api/Logout")]
        [HttpPost]
        public async Task<IActionResult> Logout([FromBody] Session_Data session)
        {

            if (session == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    using (var httpClient = new HttpClient())
                    {

                        StringContent content = new StringContent(JsonSerializer.Serialize(session), Encoding.UTF8, "application/json");

                        using (var authResponse = await httpClient.PostAsync(System_Path.AUTHENTICATION_API + "Logout", content))
                        {

                            var responseAsString = await authResponse.Content.ReadAsStringAsync();

                            if (responseAsString == null)
                            {
                                throw new Exception("14");//_14_NULL_ERROR
                            }
                            else
                            {

                                Api_Response apiResponse = JsonSerializer.Deserialize<Api_Response>(responseAsString);


                                if (apiResponse != null)
                                {

                                    if (apiResponse.code.Equals("0"))
                                    {
                                        ResponseType responseType = ResponseType.Success;
                                        return Ok(Response_Handler.GetAppResponse(responseType, apiResponse.responseData));
                                    }
                                    else
                                    {
                                        throw new Exception(apiResponse.code);
                                    }

                                }
                                else
                                {
                                    throw new Exception("16");//_16_SERVER_RESPONSE_ERROR
                                }

                            }


                        }



                    }

                }
                catch (Exception e)
                {

                    return BadRequest(Response_Handler.GetExceptionResponse(e));

                }

            }

        }




        [Route("api/UpdatePassword")]
        [HttpPost]
        public async Task<IActionResult> UpdatePassword([FromBody] Update_Passwd_Data updateData)
        {

            if (updateData == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    using (var httpClient = new HttpClient())
                    {

                        StringContent content = new StringContent(JsonSerializer.Serialize(updateData), Encoding.UTF8, "application/json");

                        using (var authResponse = await httpClient.PostAsync(System_Path.AUTHENTICATION_API + "UpdatePassword", content))
                        {

                            var responseAsString = await authResponse.Content.ReadAsStringAsync();

                            if (responseAsString == null)
                            {
                                throw new Exception("14");//_14_NULL_ERROR
                            }
                            else
                            {

                                Api_Response apiResponse = JsonSerializer.Deserialize<Api_Response>(responseAsString);


                                if (apiResponse != null)
                                {

                                    if (apiResponse.code.Equals("0"))
                                    {
                                        ResponseType responseType = ResponseType.Success;
                                        return Ok(Response_Handler.GetAppResponse(responseType, apiResponse.responseData));
                                    }
                                    else
                                    {
                                        throw new Exception(apiResponse.code);
                                    }

                                }
                                else
                                {
                                    throw new Exception("16");//_16_SERVER_RESPONSE_ERROR
                                }

                            }


                        }



                    }


                }
                catch (Exception e)
                {

                    return BadRequest(Response_Handler.GetExceptionResponse(e));
                }


            }

        }



        // POST api/ConfirmEmail
        [Route("api/SetPassword")]
        [HttpPost]
        public async Task<IActionResult> SetPassword([FromBody] Set_Passwd_Data setData)
        {

            if (setData == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    using (var httpClient = new HttpClient())
                    {

                        StringContent content = new StringContent(JsonSerializer.Serialize(setData), Encoding.UTF8, "application/json");

                        using (var authResponse = await httpClient.PostAsync(System_Path.AUTHENTICATION_API + "SetPassword", content))
                        {

                            var responseAsString = await authResponse.Content.ReadAsStringAsync();

                            if (responseAsString == null)
                            {
                                throw new Exception("14");//_14_NULL_ERROR
                            }
                            else
                            {

                                Api_Response apiResponse = JsonSerializer.Deserialize<Api_Response>(responseAsString);


                                if (apiResponse != null)
                                {

                                    if (apiResponse.code.Equals("0"))
                                    {
                                        ResponseType responseType = ResponseType.Success;
                                        return Ok(Response_Handler.GetAppResponse(responseType, apiResponse.responseData));
                                    }
                                    else
                                    {
                                        throw new Exception(apiResponse.code);
                                    }

                                }
                                else
                                {
                                    throw new Exception("16");//_16_SERVER_RESPONSE_ERROR
                                }

                            }


                        }



                    }

                }
                catch (Exception e)
                {

                    return BadRequest(Response_Handler.GetExceptionResponse(e));
                }


            }

        }




        [Route("api/ValidatePassword")]
        [HttpPost]
        public async Task<IActionResult> ValidatePassword([FromBody] Validate_Password_Data validateData)
        {

            if (validateData == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    using (var httpClient = new HttpClient())
                    {

                        StringContent content = new StringContent(JsonSerializer.Serialize(validateData), Encoding.UTF8, "application/json");

                        using (var authResponse = await httpClient.PostAsync(System_Path.AUTHENTICATION_API + "ValidatePassword", content))
                        {

                            var responseAsString = await authResponse.Content.ReadAsStringAsync();

                            if (responseAsString == null)
                            {
                                throw new Exception("14");//_14_NULL_ERROR
                            }
                            else
                            {

                                Api_Response apiResponse = JsonSerializer.Deserialize<Api_Response>(responseAsString);


                                if (apiResponse != null)
                                {

                                    if (apiResponse.code.Equals("0"))
                                    {
                                        ResponseType responseType = ResponseType.Success;
                                        return Ok(Response_Handler.GetAppResponse(responseType, apiResponse.responseData));
                                    }
                                    else
                                    {
                                        throw new Exception(apiResponse.code);
                                    }

                                }
                                else
                                {
                                    throw new Exception("16");//_16_SERVER_RESPONSE_ERROR
                                }

                            }


                        }



                    }

                }
                catch (Exception e)
                {

                    return BadRequest(Response_Handler.GetExceptionResponse(e));
                }


            }

        }



        // POST api/ConfirmEmail
        [Route("api/UpdateUsername")]
        [HttpPost]
        public async Task<IActionResult> UpdateUsername([FromBody] Update_Username_Data updateData)
        {

            if (updateData == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    using (var httpClient = new HttpClient())
                    {

                        StringContent content = new StringContent(JsonSerializer.Serialize(updateData), Encoding.UTF8, "application/json");

                        using (var authResponse = await httpClient.PostAsync(System_Path.AUTHENTICATION_API + "UpdateUsername", content))
                        {

                            var responseAsString = await authResponse.Content.ReadAsStringAsync();

                            if (responseAsString == null)
                            {
                                throw new Exception("14");//_14_NULL_ERROR
                            }
                            else
                            {

                                Api_Response apiResponse = JsonSerializer.Deserialize<Api_Response>(responseAsString);


                                if (apiResponse != null)
                                {

                                    if (apiResponse.code.Equals("0"))
                                    {
                                        ResponseType responseType = ResponseType.Success;
                                        return Ok(Response_Handler.GetAppResponse(responseType, apiResponse.responseData));
                                    }
                                    else
                                    {
                                        throw new Exception(apiResponse.code);
                                    }

                                }
                                else
                                {
                                    throw new Exception("16");//_16_SERVER_RESPONSE_ERROR
                                }

                            }


                        }



                    }

                }
                catch (Exception e)
                {

                    return BadRequest(Response_Handler.GetExceptionResponse(e));
                }


            }

        }



        // POST api/ConfirmEmail
        [Route("api/UpdateEmail")]
        [HttpPost]
        public async Task<IActionResult> UpdateEmail([FromBody] Update_Email_Data updateData)
        {

            if (updateData == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {

                try
                {

                    using (var httpClient = new HttpClient())
                    {

                        StringContent content = new StringContent(JsonSerializer.Serialize(updateData), Encoding.UTF8, "application/json");

                        using (var authResponse = await httpClient.PostAsync(System_Path.AUTHENTICATION_API + "UpdateEmail", content))
                        {

                            var responseAsString = await authResponse.Content.ReadAsStringAsync();

                            if (responseAsString == null)
                            {
                                throw new Exception("14");//_14_NULL_ERROR
                            }
                            else
                            {

                                Api_Response apiResponse = JsonSerializer.Deserialize<Api_Response>(responseAsString);


                                if (apiResponse != null)
                                {

                                    if (apiResponse.code.Equals("0"))
                                    {
                                        ResponseType responseType = ResponseType.Success;
                                        return Ok(Response_Handler.GetAppResponse(responseType, apiResponse.responseData));
                                    }
                                    else
                                    {
                                        throw new Exception(apiResponse.code);
                                    }

                                }
                                else
                                {
                                    throw new Exception("16");//_16_SERVER_RESPONSE_ERROR
                                }

                            }


                        }



                    }


                }
                catch (Exception e)
                {

                    return BadRequest(Response_Handler.GetExceptionResponse(e));
                }


            }

        }



    }
}
