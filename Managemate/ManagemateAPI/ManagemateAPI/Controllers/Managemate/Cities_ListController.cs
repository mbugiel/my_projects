using Microsoft.AspNetCore.Mvc;
using ManagemateAPI.Management.M_Cities_List.Input_Objects;
using ManagemateAPI.Management.M_Cities_List.Manager;
using ManagemateAPI.Management.M_Cities_List.Table_Model;

/*
 * This is an endpoint controller dedicated to the Cities_List table.
 * 
 * It contains methods for endpoints
 * - Add 
 * - Edit
 * - Delete
 * - Get by ID
 * - Get all 
 */
namespace ManagemateAPI.Controllers.Managemate
{

    [ApiController]
    public class Cities_ListController : ControllerBase
    {
        private Cities_List_Manager _DB_Helper;
        public Cities_ListController(IConfiguration configuration)
        {
            _DB_Helper = new Cities_List_Manager(configuration);
        }

        /*
         * Add_Cities_List endpoint
         * This endpoint is used to add a record to the Cities_List table.
         * 
         * It accepts Add_Cities_List_Data object.
         * The given object is handed over to the Add_Cities_List method in the Cities_List_Manager.
         */
        [Route("api/Add_Cities_List")]
        [HttpPost]
        public async Task<IActionResult> Add_Cities_List([FromBody] Add_Cities_List_Data obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {
                    string result = await _DB_Helper.Add_Cities_List(obj);

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

        /*
         * Edit_Cities_List endpoint
         * This endpoint is used to edit a record from the Cities_List table.
         * 
         * It accepts Edit_Cities_List_Data object.
         * The given object is handed over to the Edit_Cities_List method in the Cities_List_Manager.
         */
        [Route("api/Edit_Cities_List")]
        [HttpPost]
        public async Task<IActionResult> Edit_Cities_List([FromBody] Edit_Cities_List_Data obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {
                    string result = await _DB_Helper.Edit_Cities_List(obj);

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

        /*
         * Delete_Cities_List endpoint
         * This endpoint is used to remove record from the Cities_List table.
         * 
         * It accepts Delete_Cities_List_Data object.
         * The given object is handed over to the Delete_Cities_List method in the Cities_List_Manager.
         */
        [Route("api/Delete_Cities_List")]
        [HttpPost]
        public async Task<IActionResult> Delete_Cities_List([FromBody] Delete_Cities_List_Data obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));
            }
            else
            {
                try
                {
                    string result = await _DB_Helper.Delete_Cities_List(obj);

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

        /*
         * Get_Cities_List_By_ID endpoint
         * This endpoint is used to get a record from to the Cities_List table by its ID.
         * 
         * It accepts Get_Cities_List_By_ID object.
         * The given object is handed over to the Get_Cities_List_By_ID method in the Cities_List_Manager.
         */
        [Route("api/Get_Cities_List_By_ID")]
        [HttpPost]
        public async Task<IActionResult> Get_Cities_List_By_ID([FromBody] Get_Cities_List_By_ID obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));
            }
            else
            {
                try
                {
                    Cities_List_Model result = await _DB_Helper.Get_Cities_List_By_ID(obj);

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

        /*
         * Get_All_Cities_List endpoint
         * This endpoint is used to to all the records from the Cities_List table.
         * 
         * It accepts Get_All_Cities_List_Data object.
         * The given object is handed over to the Get_All_Cities_List method in the Cities_List_Manager.
         */
        [Route("api/Get_All_Cities_List")]
        [HttpPost]
        public async Task<IActionResult> Get_All_Cities_List([FromBody] Get_All_Cities_List_Data obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));
            }
            else
            {
                try
                {
                    List<Cities_List_Model_List> result = await _DB_Helper.Get_All_Cities_List(obj);

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


















        //Not in use
        [Route("api/Get_Cities_List_By_Page")]
        [HttpPost]
        public async Task<IActionResult> Get_Cities_List_By_Page([FromBody] Get_Cities_List_By_Page obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));
            }
            else
            {
                try
                {
                    List<Cities_List_Model_List> result = await _DB_Helper.Get_Cities_List_By_Page(obj);

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
