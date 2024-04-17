using AuthenticationAPI.Database;
using AuthenticationAPI.Helper;
using AuthenticationAPI.Helper.Service;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{

    [ApiController]
    public class CronController : ControllerBase
    {


        private readonly DbHelper _DbHelper;

        public CronController(DatabaseContext context)
        {

            _DbHelper = new DbHelper(context);

        }


        [Route("api/DeleteExpiredCodes")]
        [HttpPost]
        public IActionResult DeleteExpiredCodes()
        {

            try
            {

                _DbHelper.DeleteExpiredCodes();

                ResponseType responseType = ResponseType.Success;

                return Ok(ResponseHandler.GetAppResponse(responseType, Info.DELETED_EXPIRED_CODES));

            }
            catch (Exception e)
            {

                return BadRequest(ResponseHandler.GetExceptionResponse(e));
            }

        }


        [Route("api/DeleteExpiredSessions")]
        [HttpPost]
        public IActionResult DeleteExpiredSessions()
        {

            try
            {

                _DbHelper.DeleteExpiredSessions();

                ResponseType responseType = ResponseType.Success;

                return Ok(ResponseHandler.GetAppResponse(responseType, Info.DELETED_EXPIRED_CODES));

            }
            catch (Exception e)
            {

                return BadRequest(ResponseHandler.GetExceptionResponse(e));
            }

        }



    }
}
