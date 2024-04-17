using AuthenticationAPI.Database;
using AuthenticationAPI.Helper;
using AuthenticationAPI.Helper.Service;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{

    [ApiController]
    public class EncryptionController : ControllerBase
    {


        private readonly DbHelper _DbHelper;

        public EncryptionController(DatabaseContext context)
        {

            _DbHelper = new DbHelper(context);

        }



        [Route("api/EncryptData")]
        [HttpPost]
        public IActionResult EncryptData([FromBody] EncryptData clientData)
        {
            if (clientData == null)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {

                try
                {
                    ResponseType responseType = ResponseType.Success;

                    byte[] EncryptedData = _DbHelper.EncryptData(clientData);


                    if (EncryptedData == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }

                    return Ok(ResponseHandler.GetAppResponse(responseType, EncryptedData));


                }
                catch (Exception e)
                {

                    return BadRequest(ResponseHandler.GetExceptionResponse(e));
                }

            }


        }



        [Route("api/DecryptData")]
        [HttpPost]
        public IActionResult DecryptData([FromBody] DecryptData clientData)
        {
            if (clientData == null)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {

                try
                {
                    ResponseType responseType = ResponseType.Success;

                    string DecryptedData = _DbHelper.DecryptData(clientData);

                    if (DecryptedData == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }

                    return Ok(ResponseHandler.GetAppResponse(responseType, DecryptedData));


                }
                catch (Exception e)
                {

                    return BadRequest(ResponseHandler.GetExceptionResponse(e));
                }

            }


        }



        [Route("api/EncryptDataList")]
        [HttpPost]
        public IActionResult EncryptDataList([FromBody] EncryptDataList clientData)
        {
            if (clientData == null)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {

                try
                {
                    ResponseType responseType = ResponseType.Success;

                    List<EncryptedObject> EncryptedData = _DbHelper.EncryptDataList(clientData);

                    if (EncryptedData == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }

                    return Ok(ResponseHandler.GetAppResponse(responseType, EncryptedData));


                }
                catch (Exception e)
                {

                    return BadRequest(ResponseHandler.GetExceptionResponse(e));
                }

            }


        }



        [Route("api/DecryptDataList")]
        [HttpPost]
        public IActionResult DecryptDataList([FromBody] DecryptDataList clientData)
        {
            if (clientData == null)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {

                try
                {
                    ResponseType responseType = ResponseType.Success;

                    List<DecryptedObject> DecryptedData = _DbHelper.DecryptDataList(clientData);

                    if (DecryptedData == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }

                    return Ok(ResponseHandler.GetAppResponse(responseType, DecryptedData));


                }
                catch (Exception e)
                {

                    return BadRequest(ResponseHandler.GetExceptionResponse(e));
                }

            }


        }



    }
}
