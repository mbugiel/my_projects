using Microsoft.AspNetCore.Mvc;
using ManagemateAPI.Management.M_Item_Counting_Type.Input_Objects;
using ManagemateAPI.Management.M_Item_Counting_Type.Manager;
using ManagemateAPI.Management.M_Item_Counting_Type.Table_Model;

namespace ManagemateAPI.Controllers.Managemate
{

    [ApiController]
    public class Item_Counting_TypeController : ControllerBase
    {
        private Item_Counting_Type_Manager _DB_Helper;

        public Item_Counting_TypeController(IConfiguration configuration)
        {
            _DB_Helper = new Item_Counting_Type_Manager(configuration);
        }


        [Route("api/AddItemCountingType")]
        [HttpPost]
        public async Task<IActionResult> AddItemCountingType([FromBody] Add_Item_Counting_Type_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.AddItemCountingType(input_obj);

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



        [Route("api/EditItemCountingType")]
        [HttpPost]
        public async Task<IActionResult> EditItemCountingType([FromBody] Edit_Item_Counting_Type_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.EditItemCountingType(input_obj);

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



        [Route("api/DeleteItemCountingType")]
        [HttpPost]
        public async Task<IActionResult> DeleteItemCountingType([FromBody] Delete_Item_Counting_Type_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.DeleteItemCountingType(input_obj);

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



        [Route("api/GetItemCountingTypes")]
        [HttpPost]
        public async Task<IActionResult> GetItemCountingTypes([FromBody] Get_Item_Counting_Types_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    List<Item_Counting_Type_Model> result = await _DB_Helper.GetItemCountingTypes(input_obj);

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



        [Route("api/GetItemCountingTypeById")]
        [HttpPost]
        public async Task<IActionResult> GetItemCountingTypeById([FromBody] Get_Item_Counting_Type_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    Item_Counting_Type_Model result = await _DB_Helper.GetItemCountingTypeById(input_obj);

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
