using Microsoft.AspNetCore.Mvc;
using ManagemateAPI.Management.M_Item_Type.Input_Objects;
using ManagemateAPI.Management.M_Item_Type.Manager;
using ManagemateAPI.Management.M_Item_Type.Table_Model;

/*
 * This is an endpoint controller dedicated to the Item_Type table.
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
    public class Item_TypeController : ControllerBase
    {
        private Item_Type_Manager _DB_Helper;

        public Item_TypeController(IConfiguration configuration)
        {
            _DB_Helper = new Item_Type_Manager(configuration);
        }

        /*
         * Add_Item_Type endpoint
         * This endpoint is used to add a record to the Item_Type table.
         * 
         * It accepts Add_Item_Type_Data object.
         * The given object is handed over to the Add_Item_Type method in the Item_Type_Manager.
         */
        [Route("api/Add_Item_Type")]
        [HttpPost]
        public async Task<IActionResult> Add_Item_Type([FromBody] Add_Item_Type_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.Add_Item_Type(input_obj);

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
         * Edit_Item_Type endpoint
         * This endpoint is used to edit a record from the Item_Type table.
         * 
         * It accepts Edit_Item_Type_Data object.
         * The given object is handed over to the Edit_Item_Type method in the Item_Type_Manager.
         */
        [Route("api/Edit_Item_Type")]
        [HttpPost]
        public async Task<IActionResult> Edit_Item_Type([FromBody] Edit_Item_Type_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.Edit_Item_Type(input_obj);

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
         * Delete_Item_Type endpoint
         * This endpoint is used to remove record from the Item_Type table.
         * 
         * It accepts Delete_Item_Type_Data object.
         * The given object is handed over to the Delete_Item_Type method in the Item_Type_Manager.
         */
        [Route("api/Delete_Item_Type")]
        [HttpPost]
        public async Task<IActionResult> Delete_Item_Type([FromBody] Delete_Item_Type_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.Delete_Item_Type(input_obj);

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
         * Get_Item_Type_By_ID endpoint
         * This endpoint is used to get a record from to the Item_Type table by its ID.
         * 
         * It accepts Get_Item_Type_By_ID object.
         * The given object is handed over to the Get_Item_Type_By_ID method in the Item_Type_Manager.
         */
        [Route("api/Get_Item_Type_By_ID")]
        [HttpPost]
        public async Task<IActionResult> Get_Item_Type_By_ID([FromBody] Get_Item_Type_By_ID_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    Item_Type_Model result = await _DB_Helper.Get_Item_Type_By_ID(input_obj);

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
         * Get_All_Item_Type endpoint
         * This endpoint is used to to all the records from the Item_Type table.
         * 
         * It accepts Get_All_Item_Type_Data object.
         * The given object is handed over to the Get_All_Item_Type method in the Item_Type_Manager.
         */
        [Route("api/Get_All_Item_Type")]
        [HttpPost]
        public async Task<IActionResult> Get_All_Item_Type([FromBody] Get_All_Item_Type_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    List<Item_Type_Model> result = await _DB_Helper.Get_All_Item_Type(input_obj);

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
    }
}
