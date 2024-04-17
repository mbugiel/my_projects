using Microsoft.AspNetCore.Mvc;
using ManagemateAPI.Management.M_Item_Type.Input_Objects;
using ManagemateAPI.Management.M_Item_Type.Manager;
using ManagemateAPI.Management.M_Item_Type.Table_Model;

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


        [Route("api/AddItemType")]
        [HttpPost]
        public async Task<IActionResult> AddItemType([FromBody] Add_Item_Type_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.AddItemType(input_obj);

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



        [Route("api/EditItemType")]
        [HttpPost]
        public async Task<IActionResult> EditItemType([FromBody] Edit_Item_Type_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.EditItemType(input_obj);

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



        [Route("api/DeleteItemType")]
        [HttpPost]
        public async Task<IActionResult> DeleteItemType([FromBody] Delete_Item_Type_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.DeleteItemType(input_obj);

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



        [Route("api/GetItemTypes")]
        [HttpPost]
        public async Task<IActionResult> GetItemTypes([FromBody] Get_Item_Types_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    List<Item_Type_Model> result = await _DB_Helper.GetItemTypes(input_obj);

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



        [Route("api/GetItemTypeById")]
        [HttpPost]
        public async Task<IActionResult> GetItemTypeById([FromBody] Get_Item_Type_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    Item_Type_Model result = await _DB_Helper.GetItemTypeById(input_obj);

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
