using Microsoft.AspNetCore.Mvc;
using ManagemateAPI.Management.M_Item_On_Receipt.Input_Objects;
using ManagemateAPI.Management.M_Item_On_Receipt.Manager;
using ManagemateAPI.Management.M_Item_On_Receipt.Table_Model;

/*
 * This is an endpoint controller dedicated to the Item_On_Receipt table.
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
    public class Item_On_ReceiptController : ControllerBase
    {
        private Item_On_Receipt_Manager _DB_Helper;

        public Item_On_ReceiptController(IConfiguration configuration)
        {
            _DB_Helper = new Item_On_Receipt_Manager(configuration);
        }

        /*
         * Add_Item_On_Receipt endpoint
         * This endpoint is used to add a record to the Item_On_Receipt table.
         * 
         * It accepts Add_Item_On_Receipt_Data object.
         * The given object is handed over to the Add_Item_On_Receipt method in the Item_On_Receipt_Manager.
         */
        [Route("api/Add_Item_On_Receipt")]
        [HttpPost]
        public async Task<IActionResult> Add_Item_On_Receipt([FromBody] Add_Item_On_Receipt_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    Item_On_Receipt_ID_Model result = await _DB_Helper.Add_Item_On_Receipt(input_obj);

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
         * Edit_Item_On_Receipt endpoint
         * This endpoint is used to edit a record from the Item_On_Receipt table.
         * 
         * It accepts Edit_Item_On_Receipt_Data object.
         * The given object is handed over to the Edit_Item_On_Receipt method in the Item_On_Receipt_Manager.
         */
        [Route("api/Edit_Item_On_Receipt")]
        [HttpPost]
        public async Task<IActionResult> Edit_Item_On_Receipt([FromBody] Edit_Item_On_Receipt_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.Edit_Item_On_Receipt(input_obj);

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
         * Delete_Item_On_Receipt endpoint
         * This endpoint is used to remove record from the Item_On_Receipt table.
         * 
         * It accepts Delete_Item_On_Receipt_Data object.
         * The given object is handed over to the Delete_Item_On_Receipt method in the Item_On_Receipt_Manager.
         */
        [Route("api/Delete_Item_On_Receipt")]
        [HttpPost]
        public async Task<IActionResult> Delete_Item_On_Receipt([FromBody] Delete_Item_On_Receipt_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.Delete_Item_On_Receipt(input_obj);

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
         * Get_Item_On_Receipt_By_ID endpoint
         * This endpoint is used to get a record from to the Item_On_Receipt table by its ID.
         * 
         * It accepts Get_Item_On_Receipt_By_ID object.
         * The given object is handed over to the Get_Item_On_Receipt_By_ID method in the Item_On_Receipt_Manager.
         */
        [Route("api/Get_Items_From_Receipt")]
        [HttpPost]
        public async Task<IActionResult> Get_Items_On_Receipt_By_ID([FromBody] Get_Items_From_Receipt_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    List<Item_On_Receipt_Model> result = await _DB_Helper.Get_Items_From_Receipt(input_obj);

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
         * Get_Item_On_Receipt_By_ID endpoint
         * This endpoint is used to get a record from to the Item_On_Receipt table by its ID.
         * 
         * It accepts Get_Item_On_Receipt_By_ID object.
         * The given object is handed over to the Get_Item_On_Receipt_By_ID method in the Item_On_Receipt_Manager.
         */
        [Route("api/Get_Available_Items_To_Return")]
        [HttpPost]
        public async Task<IActionResult> Get_Available_Items_To_Return([FromBody] Get_Available_Items_To_Return_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    List<Item_On_Receipt_Return_Available_Model> result = await _DB_Helper.Get_Available_Items_To_Return(input_obj);

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
