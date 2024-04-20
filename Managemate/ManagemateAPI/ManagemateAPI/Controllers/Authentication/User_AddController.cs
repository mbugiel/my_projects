using ManagemateAPI.Database.Context;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using ManagemateAPI.Management.M_User.Input_Objects;
using ManagemateAPI.Information;

namespace ManagemateAPI.Controllers.Authentication
{

    [ApiController]
    public class User_AddController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public User_AddController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // POST api/AddUser
        [Route("api/AddUser")]
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] Add_User_Data newUser)
        {
            if (newUser == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {

                try
                {

                    using (var httpClient = new HttpClient())
                    {

                        StringContent content = new StringContent(JsonSerializer.Serialize(newUser), Encoding.UTF8, "application/json");

                        using (var authResponse = await httpClient.PostAsync(System_Path.AUTHENTICATION_API + "AddUser", content))
                        {

                            var responseAsString = await authResponse.Content.ReadAsStringAsync();

                            if (responseAsString == null)
                            {
                                throw new Exception("14");//_14_NULL_ERROR
                            }
                            else
                            {

                                Api_Response_Session apiResponse = JsonSerializer.Deserialize<Api_Response_Session>(responseAsString);


                                if (apiResponse != null)
                                {

                                    if (apiResponse.code.Equals("0"))
                                    {
                                        ResponseType responseType = ResponseType.Success;

                                        try
                                        {

                                            Database_Creator creator = new Database_Creator(_configuration);
                                            creator.CreateDataBase(apiResponse.responseData.userId);

                                        }
                                        catch (Exception)
                                        {

                                            return Ok(Response_Handler.GetAppResponse(responseType, apiResponse.responseData));
                                        }


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
