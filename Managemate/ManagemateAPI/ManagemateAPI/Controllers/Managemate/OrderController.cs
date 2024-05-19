using Microsoft.AspNetCore.Mvc;
using ManagemateAPI.Management.M_Order.Input_Objects;
using ManagemateAPI.Management.M_Order.Manager;
using ManagemateAPI.Management.M_Order.Table_Model;

/*
 * This is an endpoint controller dedicated to the Order table.
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
    public class OrderController : ControllerBase
    {
        private Order_Manager _DB_Helper;

        public OrderController(IConfiguration configuration)
        {
            _DB_Helper = new Order_Manager(configuration);
        }

        /*
         * Add_Order endpoint
         * This endpoint is used to add a record to the Order table.
         * 
         * It accepts Add_Order_Data object.
         * The given object is handed over to the Add_Order method in the Order_Manager.
         */
        [Route("api/Add_Order")]
        [HttpPost]
        public async Task<IActionResult> Add_Order([FromBody] Add_Order_Data order)
        {

            if (order == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.Add_Order(order);

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
         * Edit_Order endpoint
         * This endpoint is used to edit a record from the Order table.
         * 
         * It accepts Edit_Order_Data object.
         * The given object is handed over to the Edit_Order method in the Order_Manager.
         */
        [Route("api/Edit_Order")]
        [HttpPost]
        public async Task<IActionResult> Edit_Order([FromBody] Edit_Order_Data order)
        {

            if (order == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.Edit_Order(order);

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
         * Delete_Order endpoint
         * This endpoint is used to remove record from the Order table.
         * 
         * It accepts Delete_Order_Data object.
         * The given object is handed over to the Delete_Order method in the Order_Manager.
         */
        [Route("api/Delete_Order")]
        [HttpPost]
        public async Task<IActionResult> Delete_Order([FromBody] Delete_Order_Data order)
        {

            if (order == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.Delete_Order(order);

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
         * Get_Order_By_ID endpoint
         * This endpoint is used to get a record from to the Order table by its ID.
         * 
         * It accepts Get_Order_By_ID object.
         * The given object is handed over to the Get_Order_By_ID method in the Order_Manager.
         */
        [Route("api/Get_Order_By_ID")]
        [HttpPost]
        public async Task<IActionResult> Get_Order_By_ID([FromBody] Get_Order_By_Id_Data order)
        {

            if (order == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    Order_Model result = await _DB_Helper.Get_Order_By_ID(order);

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
         * Get_All_Order endpoint
         * This endpoint is used to to all the records from the Order table.
         * 
         * It accepts Get_All_Order_Data object.
         * The given object is handed over to the Get_All_Order method in the Order_Manager.
         */
        [Route("api/Get_All_Order")]
        [HttpPost]
        public async Task<IActionResult> Get_All_Order([FromBody] Get_All_Order_Data order)
        {

            if (order == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    List<Order_Model_List> result = await _DB_Helper.Get_All_Order(order);

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
