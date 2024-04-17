using Microsoft.AspNetCore.Mvc;
using ManagemateAPI.Management.M_Order.Input_Objects;
using ManagemateAPI.Management.M_Order.Manager;
using ManagemateAPI.Management.M_Order.Table_Model;

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


        [Route("api/AddOrder")]
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] Add_Order_Data order)
        {

            if (order == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.AddOrder(order);

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



        [Route("api/EditOrder")]
        [HttpPost]
        public async Task<IActionResult> EditOrder([FromBody] Edit_Order_Data order)
        {

            if (order == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.EditOrder(order);

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



        [Route("api/DeleteOrder")]
        [HttpPost]
        public async Task<IActionResult> DeleteOrder([FromBody] Delete_Order_Data order)
        {

            if (order == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.DeleteOrder(order);

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



        [Route("api/GetOrders")]
        [HttpPost]
        public async Task<IActionResult> GetOrders([FromBody] Get_Orders_Data order)
        {

            if (order == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    List<Order_Model_List> result = await _DB_Helper.GetOrders(order);

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



        [Route("api/GetOrderById")]
        [HttpPost]
        public async Task<IActionResult> GetOrderById([FromBody] Get_Order_By_Id_Data order)
        {

            if (order == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    Order_Model result = await _DB_Helper.GetOrderById(order);

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
