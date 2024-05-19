using ManagemateAPI.Management.M_Item_Trading_Type.Input_Objects;
using ManagemateAPI.Management.M_Item_Trading_Type.Manager;
using ManagemateAPI.Management.M_Item_Trading_Type.Table_Model;
using Microsoft.AspNetCore.Mvc;

/*
 * This is an endpoint controller dedicated to the Item_Trading_Type table.
 * 
 * It contains methods for endpoints
 * - Get all 
 */
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

        /*
         * Get_All_Item_Trading_Type endpoint
         * This endpoint is used to to all the records from the Item_Trading_Type table.
         * 
         * It accepts Get_All_Item_Trading_Type_Data object.
         * The given object is handed over to the Get_All_Item_Trading_Type method in the Item_Trading_Type_Manager.
         */
        [Route("api/Get_All_Item_Trading_Types")]
        [HttpPost]
        public async Task<IActionResult> Get_All_Item_Trading_Types([FromBody] Get_All_Item_Trading_Types_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    List<Item_Trading_Type_Model> result = await _DB_Helper.Get_All_Item_Trading_Types(input_obj);

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
