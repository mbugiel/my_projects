using ManagemateAPI.Controllers;
using System.Text.Json;
using System.Text;
using ManagemateAPI.Management.M_Session.Input_Objects;
using ManagemateAPI.Information;

namespace ManagemateAPI.Management.M_Session.Manager
{
    public static class Session_Checker
    {

        public static async Task<bool> ActiveSession(Session_Data session)
        {

            if (session == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {

                using (var httpClient = new HttpClient())
                {

                    StringContent content = new StringContent(JsonSerializer.Serialize(session), Encoding.UTF8, "application/json");

                    using (var authResponse = await httpClient.PostAsync(System_Path.AUTHENTICATION_API + "ActiveSession", content))
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
                                    return true;
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

        }



    }
}
