using Microsoft.AspNetCore.Mvc;
using ManagemateAPI.Management.M_Receipt.Input_Objects;
using ManagemateAPI.Management.M_Receipt.Manager;
using ManagemateAPI.Management.M_Receipt.Table_Model;

/*
 * This is an endpoint controller dedicated to the Receipt table.
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
    public class ReceiptController : ControllerBase
    {
        private Receipt_Manager _DB_Helper;

        public ReceiptController(IConfiguration configuration)
        {
            _DB_Helper = new Receipt_Manager(configuration);
        }

        /*
         * Add_Authorized_Worker endpoint
         * This endpoint is used to add a record to the Receipt table.
         * 
         * It accepts Add_Authorized_Worker_Data object.
         * The given object is handed over to the Add_Authorized_Worker method in the Receipt_Manager.
         */
        [Route("api/Add_Receipt")]
        [HttpPost]
        public async Task<IActionResult> Add_Receipt([FromBody] Add_Receipt_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    Receipt_Model_Id result = await _DB_Helper.Add_Receipt(obj);

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
         * Edit_Authorized_Worker endpoint
         * This endpoint is used to edit a record from the Receipt table.
         * 
         * It accepts Edit_Authorized_Worker_Data object.
         * The given object is handed over to the Edit_Authorized_Worker method in the Receipt_Manager.
         */
        [Route("api/Edit_Receipt")]
        [HttpPost]
        public async Task<IActionResult> Edit_Receipt([FromBody] Edit_Receipt_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.Edit_Receipt(obj);

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
         * Change_Reservation_State endpoint
         * This endpoint is used to switch between two states of reservation field in the Receipt table.
         * 
         * It accepts Change_Reservation_State_Data object.
         * The given object is handed over to the Change_Reservation_State method in the Receipt_Manager.
         */
        [Route("api/Change_Reservation_State")]
        [HttpPost]
        public async Task<IActionResult> Change_Reservation_State([FromBody] Change_Reservation_State_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.Change_Reservation_State(obj);

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
         * Delete_Authorized_Worker endpoint
         * This endpoint is used to remove record from the Receipt table.
         * 
         * It accepts Delete_Authorized_Worker_Data object.
         * The given object is handed over to the Delete_Authorized_Worker method in the Receipt_Manager.
         */
        [Route("api/Delete_Receipt")]
        [HttpPost]
        public async Task<IActionResult> Delete_Receipt([FromBody] Delete_Receipt_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.Delete_Receipt(obj);

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
         * Get_Authorized_Worker_By_ID endpoint
         * This endpoint is used to get a record from to the Receipt table by its ID.
         * 
         * It accepts Get_Authorized_Worker_By_ID object.
         * The given object is handed over to the Get_Authorized_Worker_By_ID method in the Receipt_Manager.
         */
        [Route("api/Get_Receipt_By_ID")]
        [HttpPost]
        public async Task<IActionResult> Get_Receipt_By_ID([FromBody] Get_Receipt_By_ID_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                Receipt_Model result = await _DB_Helper.Get_Receipt_By_ID(obj);

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
         * Get_All_Authorized_Worker endpoint
         * This endpoint is used to to all the records from the Receipt table.
         * 
         * It accepts Get_All_Authorized_Worker_Data object.
         * The given object is handed over to the Get_All_Authorized_Worker method in the Receipt_Manager.
         */
        [Route("api/Get_In_Out_Receipt")]
        [HttpPost]
        public async Task<IActionResult> Get_All_Receipt([FromBody] Get_In_Out_Receipt_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    List<Receipt_Model_List> result = await _DB_Helper.Get_In_Out_Receipt(obj);

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

        //==================================NOT+IN+USE==================================\\
        [Route("api/Get_all_by_page_Receipts")]
        [HttpPost]
        public async Task<IActionResult> Get_all_by_page_Receipts([FromBody] Get_Receipts_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    List<Receipt_Model_List> result = await _DB_Helper.Get_all_by_page_Receipts(obj);

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
