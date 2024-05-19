using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using ManagemateAPI.Management.M_Item.Input_Objects;
using ManagemateAPI.Management.M_Item.Manager;
using ManagemateAPI.Management.M_Item.Table_Model;

/*
 * This is an endpoint controller dedicated to the Item table.
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
    public class ItemController : ControllerBase
    {
        private Item_Manager _DB_Helper;

        public ItemController(IConfiguration configuration)
        {
            _DB_Helper = new Item_Manager(configuration);
        }

        /*
         * Add_Item endpoint
         * This endpoint is used to add a record to the Item table.
         * 
         * It accepts Add_Item_Data object.
         * The given object is handed over to the Add_Item method in the Item_Manager.
         */
        [Route("api/Add_Item")]
        [HttpPost]
        public async Task<IActionResult> Add_Item([FromBody] Add_Item_Data item)
        {

            if (item == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.Add_Item(item);

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
         * Edit_Item endpoint
         * This endpoint is used to edit a record from the Item table.
         * 
         * It accepts Edit_Item_Data object.
         * The given object is handed over to the Edit_Item method in the Item_Manager.
         */
        [Route("api/Edit_Item")]
        [HttpPost]
        public async Task<IActionResult> Edit_Item([FromBody] Edit_Item_Data item)
        {
            if (item == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {
                    string result = await _DB_Helper.Edit_Item(item);

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
         * Delete_Item endpoint
         * This endpoint is used to remove record from the Item table.
         * 
         * It accepts Delete_Item_Data object.
         * The given object is handed over to the Delete_Item method in the Item_Manager.
         */
        [Route("api/Delete_Item")]
        [HttpPost]
        public async Task<IActionResult> Delete_Item([FromBody] Delete_Item_Data item)
        {
            if (item == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));
            }
            else
            {
                try
                {
                    string result = await _DB_Helper.Delete_Item(item);

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
         * Get_Item_By_ID endpoint
         * This endpoint is used to get a record from to the Item table by its ID.
         * 
         * It accepts Get_Item_By_ID object.
         * The given object is handed over to the Get_Item_By_ID method in the Item_Manager.
         */
        [Route("api/Get_Item_by_ID")]
        [HttpPost]
        public async Task<IActionResult> Get_Item_by_ID([FromBody] Get_Item_By_ID_Data obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));
            }
            else
            {
                try
                {
                    Item_Model result = await _DB_Helper.Get_Item_by_ID(obj);

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
         * Get_All_Item_Or_Service endpoint
         * This endpoint is used to to all the records from the Item table.
         * 
         * It accepts Get_All_Item_Data object.
         * The given object is handed over to the Get_All_Item_Or_Service method in the Item_Manager.
         */
        [Route("api/Get_All_Item_Or_Service")]
        [HttpPost]
        public async Task<IActionResult> Get_All_Item_Or_Service([FromBody] Get_All_Item_Data obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));
            }
            else
            {
                try
                {
                    List<Item_Model_List> result = await _DB_Helper.Get_All_Item_Or_Service(obj);

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


















        //------------------------------------------------------------- Currently not in use -------------------------------------------------------------

        [Route("api/Get_Item_Page")]
        [HttpPost]
        public async Task<IActionResult> Get_Item_Page([FromBody] Get_Item_By_Page_Data obj)
        {
            if(obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));
            }
            else
            {
                try
                {
                    List<Item_Model_List> result = await _DB_Helper.Get_Item_Page(obj);

                    if (result == null)
                    {
                        throw new Exception("14");
                    }

                    ResponseType responseType = ResponseType.Success;

                    return Ok(Response_Handler.GetAppResponse(responseType,result));
                }
                catch (Exception e)
                {
                    return BadRequest(Response_Handler.GetExceptionResponse(e));
                }
            }
        }

        [Route("api/Get_Number_Of_Items")]
        [HttpPost]
        public async Task<IActionResult> Get_Number_Of_Items([FromBody] Get_Number_Of_Items_Data obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));
            }
            else
            {
                try
                {
                    long result = await _DB_Helper.Get_Number_Of_Items(obj);

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
