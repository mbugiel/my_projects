using Microsoft.AspNetCore.Mvc;
using ManagemateAPI.Management.M_Item_On_Receipt.Input_Objects;
using ManagemateAPI.Management.M_Item_On_Receipt.Manager;
using ManagemateAPI.Management.M_Item_On_Receipt.Table_Model;

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


        [Route("api/AddItemOnReceipt")]
        [HttpPost]
        public async Task<IActionResult> AddItemOnReceipt([FromBody] Add_Item_On_Receipt_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.AddItemOnReceipt(input_obj);

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



        [Route("api/EditItemOnReceipt")]
        [HttpPost]
        public async Task<IActionResult> EditItemOnReceipt([FromBody] Edit_Item_On_Receipt_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.EditItemOnReceipt(input_obj);

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



        [Route("api/DeleteItemOnReceipt")]
        [HttpPost]
        public async Task<IActionResult> DeleteOrder([FromBody] Delete_Item_On_Receipt_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.DeleteItemOnReceipt(input_obj);

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



        [Route("api/GetItemsOnReceipt")]
        [HttpPost]
        public async Task<IActionResult> GetOrders([FromBody] Get_Items_On_Receipt_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    List<Item_On_Receipt_Model> result = await _DB_Helper.GetItemsOnReceipt(input_obj);

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
