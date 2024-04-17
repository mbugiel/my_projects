using Microsoft.AspNetCore.Mvc;
using ManagemateAPI.Management.M_Receipt.Input_Objects;
using ManagemateAPI.Management.M_Receipt.Manager;
using ManagemateAPI.Management.M_Receipt.Table_Model;

namespace ManagemateAPI.Controllers.Managemate
{

    [ApiController]
    public class ReceiptController : ControllerBase
    {
        private Receipt_Manager _DB_Helper;

        public ReceiptController(IConfiguration configuration)
        {
            _DB_Helper = new Receipt_Manager(configuration);
        }


        [Route("api/AddReceipt")]
        [HttpPost]
        public async Task<IActionResult> AddReceipt([FromBody] Add_Receipt_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.AddReceipt(obj);

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



        [Route("api/EditReceipt")]
        [HttpPost]
        public async Task<IActionResult> EditReceipt([FromBody] Edit_Receipt_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.EditReceipt(obj);

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



        [Route("api/DeleteReceipt")]
        [HttpPost]
        public async Task<IActionResult> DeleteReceipt([FromBody] Delete_Receipt_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.DeleteReceipt(obj);

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



        [Route("api/GetReceipts")]
        [HttpPost]
        public async Task<IActionResult> GetReceipts([FromBody] Get_Receipts_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    List<Receipt_Model_List> result = await _DB_Helper.GetReceipts(obj);

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



        [Route("api/Get_Receipt_List_In_Out")]
        [HttpPost]
        public async Task<IActionResult> Get_Receipt_List_In_Out([FromBody] Get_Receipt_List_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    List<Receipt_Model_List> result = await _DB_Helper.Get_Receipt_List_In_Out(obj);

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



        [Route("api/GetReceiptById")]
        [HttpPost]
        public async Task<IActionResult> GetReceiptById([FromBody] Get_Receipt_By_Id_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    Receipt_Model result = await _DB_Helper.GetReceiptById(obj);

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
