using Microsoft.AspNetCore.Mvc;
using ManagemateAPI.Management.M_Authorized_Worker.Input_Objects;
using ManagemateAPI.Management.M_Authorized_Worker.Table_Model;
using ManagemateAPI.Management.M_Authorized_Worker.Manager;

namespace ManagemateAPI.Controllers.Managemate
{

    [ApiController]
    public class Authorized_WorkerController : ControllerBase
    {
        private Authorized_Worker_Manager _DB_Helper;
        public Authorized_WorkerController(IConfiguration configuration)
        {
            _DB_Helper = new Authorized_Worker_Manager(configuration);
        }

        [Route("api/Add_Authorized_Worker")]
        [HttpPost]
        public async Task<IActionResult> Add_Authorized_Worker([FromBody] Add_Authorized_Worker_Data obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {
                    string result = await _DB_Helper.Add_Authorized_Worker(obj);

                    if (result == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }

                    ResponseType responseType = ResponseType.Success;

                    return Ok(Response_Handler.GetAppResponse(responseType, result));

                }
                catch (Exception e)
                {
                    return BadRequest(Response_Handler.GetExceptionResponse(e));
                }
            }
        }

        [Route("api/Edit_Authorized_Worker")]
        [HttpPost]
        public async Task<IActionResult> Edit_Authorized_Worker([FromBody] Edit_Authorized_Worker_Data obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {
                    string result = await _DB_Helper.Edit_Authorized_Worker(obj);

                    if (result == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }

                    ResponseType responseType = ResponseType.Success;

                    return Ok(Response_Handler.GetAppResponse(responseType, result));

                }
                catch (Exception e)
                {
                    return BadRequest(Response_Handler.GetExceptionResponse(e));
                }
            }
        }

        [Route("api/Delete_Auth_Worker")]
        [HttpPost]
        public async Task<IActionResult> Delete_Authorized_Worker([FromBody] Delete_Authorized_Worker_Data obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));
            }
            else
            {
                try
                {
                    string result = await _DB_Helper.Delete_Authorized_Worker(obj);

                    if (result == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }

                    ResponseType responseType = ResponseType.Success;

                    return Ok(Response_Handler.GetAppResponse(responseType, result));
                }
                catch (Exception e)
                {
                    return BadRequest(Response_Handler.GetExceptionResponse(e));
                }
            }
        }

        [Route("api/Get_Authorized_Worker_By_ID")]
        [HttpPost]
        public async Task<IActionResult> Get_Authorized_Worker_By_ID([FromBody] Get_Authorized_Worker_By_ID_Data obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));
            }
            else
            {
                try
                {
                    Authorized_Worker_Model result = await _DB_Helper.Get_Authorized_Worker_By_ID(obj);

                    if (result == null)
                    {
                        throw new Exception("14");
                    }

                    ResponseType responseType = ResponseType.Success;

                    return Ok(Response_Handler.GetAppResponse(responseType, result));
                }
                catch (Exception e)
                {
                    return BadRequest(Response_Handler.GetExceptionResponse(e));
                }
            }
        }

        [Route("api/Get_All_Authorized_Worker")]
        [HttpPost]
        public async Task<IActionResult> Get_All_Authorized_Worker([FromBody] Get_All_Authorized_Worker_Data obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));
            }
            else
            {
                try
                {
                    List<Authorized_Worker_Model_List> result = await _DB_Helper.Get_All_Authorized_Worker(obj);

                    if (result == null)
                    {
                        throw new Exception("14");
                    }

                    ResponseType responseType = ResponseType.Success;

                    return Ok(Response_Handler.GetAppResponse(responseType, result));
                }
                catch (Exception e)
                {
                    return BadRequest(Response_Handler.GetExceptionResponse(e));
                }
            }
        }

        [Route("api/Get_Authorized_Worker_Page_NOT_IN_USE")]
        [HttpPost]
        public async Task<IActionResult> Get_Authorized_Worker_By_Page([FromBody] Get_Authorized_Worker_By_Page_Data obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));
            }
            else
            {
                try
                {
                    List<Authorized_Worker_Model_List> result = await _DB_Helper.Get_Authorized_Worker_By_Page(obj);

                    if (result == null)
                    {
                        throw new Exception("14");
                    }

                    ResponseType responseType = ResponseType.Success;

                    return Ok(Response_Handler.GetAppResponse(responseType, result));
                }
                catch (Exception e)
                {
                    return BadRequest(Response_Handler.GetExceptionResponse(e));
                }
            }
        }

    }
}
