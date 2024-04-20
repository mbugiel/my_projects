using AuthenticationAPI.Database;
using AuthenticationAPI.Helper;
using AuthenticationAPI.Helper.Service;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{

    [ApiController]
    public class UserController : ControllerBase
    {


        private readonly DbHelper _DbHelper;

        public UserController(DatabaseContext context)
        {

            _DbHelper = new DbHelper(context);

        }


        [Route("api/ActiveSession")]
        [HttpPost]
        public IActionResult ActiveSession([FromBody] SessionData session) 
        {

            if (session == null)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = _DbHelper.ActiveSession(session);

                    if (result == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }

                    ResponseType responseType = ResponseType.Success;

                    return Ok(ResponseHandler.GetAppResponse(responseType, result));
                    

                }
                catch (Exception e)
                {

                    return BadRequest(ResponseHandler.GetExceptionResponse(e));

                }

            }

        }



        [Route("api/UserExist")]
        [HttpPost]
        public IActionResult UserExist([FromBody] UserExistData newUserCheck)
        {
            if (newUserCheck == null)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {

                try
                {

                    string result = _DbHelper.UserExist(newUserCheck);

                    if (result == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }

                    ResponseType responseType = ResponseType.Success;

                    return Ok(ResponseHandler.GetAppResponse(responseType, result));

                }
                catch (Exception e)
                {

                    return BadRequest(ResponseHandler.GetExceptionResponse(e));
                }

            }

        }



        [Route("api/AddUser")]
        [HttpPost]
        public IActionResult AddUser([FromBody] AddUserData newUser)
        {
            if (newUser == null)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {

                try
                {

                    SessionData session = _DbHelper.AddUser(newUser);

                    if (session == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }

                    ResponseType responseType = ResponseType.Success;

                    return Ok(ResponseHandler.GetAppResponse(responseType, session));

                }
                catch (Exception e)
                {

                    return BadRequest(ResponseHandler.GetExceptionResponse(e));
                }

            }

        }



        [Route("api/Login")]
        [HttpPost]
        public IActionResult Login([FromBody] LoginData loginData)
        {
            if (loginData == null)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {

                try
                {
                    ResponseType responseType = ResponseType.Success;

                    SessionData loginCheck = _DbHelper.Login(loginData);

                    if (loginCheck == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }

                    return Ok(ResponseHandler.GetAppResponse(responseType, loginCheck));


                }
                catch (Exception e)
                {

                    return BadRequest(ResponseHandler.GetExceptionResponse(e));
                }

            }


        }


        [Route("api/HasTwoStepLogin")]
        [HttpPost]
        public IActionResult HasTwoStepLogin([FromBody] HasTwoStepLoginData loginData)
        {
            if (loginData == null)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {

                try
                {
                    ResponseType responseType = ResponseType.Success;

                    string loginCheck = _DbHelper.HasTwoStepLogin(loginData);

                    if (loginCheck == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }

                    return Ok(ResponseHandler.GetAppResponse(responseType, loginCheck));


                }
                catch (Exception e)
                {

                    return BadRequest(ResponseHandler.GetExceptionResponse(e));
                }

            }


        }



        [Route("api/Logout")]
        [HttpPost]
        public IActionResult Logout([FromBody] SessionData session)
        {

            if (session == null)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {
                    ResponseType responseType = ResponseType.Success;

                    var result = _DbHelper.Logout(session);

                    return Ok(ResponseHandler.GetAppResponse(responseType, result));

                }
                catch (Exception e)
                {

                    return BadRequest(ResponseHandler.GetExceptionResponse(e));

                }

            }

        }



        [Route("api/UpdatePassword")]
        [HttpPost]
        public IActionResult UpdatePassword([FromBody] UpdatePasswdData updateData)
        {
            ResponseType responseType = ResponseType.Success;

            if (updateData == null)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {
                    var result = _DbHelper.UpdatePassword(updateData.SessionData, updateData.Password, updateData.newPassword);

                    if (result == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }

                    return Ok(ResponseHandler.GetAppResponse(responseType, result));

                }
                catch (Exception e)
                {

                    return BadRequest(ResponseHandler.GetExceptionResponse(e));
                }


            }

        }



        [Route("api/ValidatePassword")]
        [HttpPost]
        public IActionResult ValidatePassword([FromBody] ValidatePasswordData passwdData)
        {
            ResponseType responseType = ResponseType.Success;

            if (passwdData == null)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {
                    var result = _DbHelper.ValidatePassword(passwdData);

                    if (result == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }

                    return Ok(ResponseHandler.GetAppResponse(responseType, result));

                }
                catch (Exception e)
                {

                    return BadRequest(ResponseHandler.GetExceptionResponse(e));
                }


            }

        }



        [Route("api/UpdateUsername")]
        [HttpPost]
        public IActionResult UpdateUsername([FromBody] UpdateUsernameData updateData)
        {
            ResponseType responseType = ResponseType.Success;

            if (updateData == null)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {
                    var result = _DbHelper.UpdateUsername(updateData.SessionData, updateData.newUsername);

                    if (result == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }

                    return Ok(ResponseHandler.GetAppResponse(responseType, result));

                }
                catch (Exception e)
                {

                    return BadRequest(ResponseHandler.GetExceptionResponse(e));
                }


            }

        }



        [Route("api/UpdateEmail")]
        [HttpPost]
        public IActionResult UpdateEmail([FromBody] UpdateEmailData updateData)
        {
            ResponseType responseType = ResponseType.Success;

            if (updateData == null)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {
                    var result = _DbHelper.UpdateEmail(updateData.SessionData, updateData.newEmail, updateData.EmailToken);

                    if (result == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }

                    return Ok(ResponseHandler.GetAppResponse(responseType, result));

                }
                catch (Exception e)
                {

                    return BadRequest(ResponseHandler.GetExceptionResponse(e));
                }


            }

        }



        [Route("api/SetPassword")]
        [HttpPost]
        public IActionResult SetPassword([FromBody] SetPasswdData setData)
        {
            ResponseType responseType = ResponseType.Success;

            if (setData == null)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {
                    var result = _DbHelper.SetPassword(setData);

                    if (result == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }

                    return Ok(ResponseHandler.GetAppResponse(responseType, result));

                }
                catch (Exception e)
                {

                    return BadRequest(ResponseHandler.GetExceptionResponse(e));
                }


            }

        }



    }
}
