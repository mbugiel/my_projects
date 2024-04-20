using ManagemateAPI.Management.M_Item_Trading_Type.Input_Objects;
using ManagemateAPI.Management.M_Item_Trading_Type.Manager;
using ManagemateAPI.Management.M_Item_Trading_Type.Table_Model;
using Microsoft.AspNetCore.Mvc;

namespace ManagemateAPI.Controllers.Managemate
{

    [ApiController]
    public class Item_Trading_TypeController : ControllerBase
    {

        private Item_Trading_Type_Manager _DB_Helper;

        public Item_Trading_TypeController(IConfiguration configuration)
        {
            _DB_Helper = new Item_Trading_Type_Manager(configuration);
        }

        [Route("api/GetItemTradingTypes")]
        [HttpPost]
        public async Task<IActionResult> GetItemTradingTypes([FromBody] Get_Item_Trading_Types_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    List<Item_Trading_Type_Model> result = await _DB_Helper.GetItemTradingTypes(input_obj);

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
