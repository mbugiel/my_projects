using AuthenticationAPI.Database;
using AuthenticationAPI.Helper;
using AuthenticationAPI.Helper.Service;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{

    [ApiController]
    public class EmailController : ControllerBase
    {


        private readonly DbHelper _DbHelper;

        public EmailController(DatabaseContext context)
        {

            _DbHelper = new DbHelper(context);

        }



        [Route("api/SaveCode")]
        [HttpPost]
        public IActionResult SaveCode([FromBody] SaveCodeData data)
        {
            if (data == null)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {

                try
                {

                    string code = _DbHelper.SaveConfirmationCode(data.Email, data.ValidationCode);

                    if (code == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }

                    ResponseType responseType = ResponseType.Success;

                    return Ok(ResponseHandler.GetAppResponse(responseType, code));

                }
                catch (Exception e)
                {

                    return BadRequest(ResponseHandler.GetExceptionResponse(e));
                }

            }

        }



    }
}
