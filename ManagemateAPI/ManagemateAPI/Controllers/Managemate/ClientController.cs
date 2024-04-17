using Microsoft.AspNetCore.Mvc;
using ManagemateAPI.Management.M_Client.Input_Objects;
using ManagemateAPI.Management.M_Client.Manager;
using ManagemateAPI.Helper.InputObjects.Client;
using ManagemateAPI.Management.M_Client.Table_Model;

namespace ManagemateAPI.Controllers.Managemate
{

    [ApiController]
    public class ClientController : ControllerBase
    {
        private Client_Manager _DB_Helper;
        public ClientController(IConfiguration configuration)
        {
            _DB_Helper = new Client_Manager(configuration);
        }

        [Route("api/AddClient")]
        [HttpPost]
        public async Task<IActionResult> AddClient([FromBody] Add_Client_Data obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {
                    string result = await _DB_Helper.AddClient(obj);

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

        [Route("api/EditClient")]
        [HttpPost]
        public async Task<IActionResult> Edit_Client([FromBody] Edit_Client_Data obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {
                    string result = await _DB_Helper.Edit_Client(obj);

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

        [Route("api/DeleteClient")]
        [HttpPost]
        public async Task<IActionResult> Delete_Client([FromBody] Delete_Client_Data obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));
            }
            else
            {
                try
                {
                    string result = await _DB_Helper.Delete_Client(obj);

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

        [Route("api/Get_Client_by_ID")]
        [HttpPost]
        public async Task<IActionResult> Get_Client_by_ID([FromBody] Get_Client_By_ID obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));
            }
            else
            {
                try
                {
                    Client_Model result = await _DB_Helper.Get_Client_by_ID(obj);

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

        [Route("api/Get_All_Clients")]
        [HttpPost]
        public async Task<IActionResult> Get_All_Items([FromBody] Get_All_Clients_Data obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));
            }
            else
            {
                try
                {
                    List<Client_Model_List> result = await _DB_Helper.Get_All_Clients(obj);

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

        //Nieużywane
        [Route("api/Get_Client_Page")]
        [HttpPost]
        public async Task<IActionResult> Get_Client_Page([FromBody] Get_Client_Page obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));
            }
            else
            {
                try
                {
                    List<Client_Model_List> result = await _DB_Helper.Get_Client_Page(obj);

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
