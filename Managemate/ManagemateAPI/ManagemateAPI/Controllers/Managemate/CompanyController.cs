using Microsoft.AspNetCore.Mvc;
using ManagemateAPI.Management.M_Company.Input_Objects;
using ManagemateAPI.Management.M_Company.Manager;
using ManagemateAPI.Management.M_Company.Table_Model;

/*
 * This is an endpoint controller dedicated to the Company table.
 * 
 * It contains methods for endpoints
 * - Add 
 * - Edit
 * - Get
 */
namespace ManagemateAPI.Controllers.Managemate
{

    [ApiController]
    public class CompanyController : ControllerBase
    {
        private Company_Manager _DB_Helper;

        public CompanyController(IConfiguration configuration)
        {
            _DB_Helper = new Company_Manager(configuration);
        }

        /*
         * Add_Company endpoint
         * This endpoint is used to add a record to the Company table.
         * 
         * It accepts Add_Company_Data object.
         * The given object is handed over to the Add_Company_Data method in the Company_Manager.
         */
        [Route("api/Add_Company")]
        [HttpPost]
        public async Task<IActionResult> Add_Company([FromBody] Add_Company_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.Add_Company(input_obj);

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
         * Edit_Company endpoint
         * This endpoint is used to edit a record from the Company table.
         * 
         * It accepts Edit_Company_Data object.
         * The given object is handed over to the Edit_Company_Data method in the Company_Manager.
         */
        [Route("api/Edit_Company")]
        [HttpPost]
        public async Task<IActionResult> Edit_Company([FromBody] Edit_Company_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.Edit_Company(input_obj);

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
         * Get_Company_By_ID endpoint
         * This endpoint is used to get a record from to the Company table by its ID.
         * 
         * It accepts Get_Company_Data object.
         * The given object is handed over to the Get_Company_Data method in the Company_Manager.
         */
        [Route("api/Get_Company")]
        [HttpPost]
        public async Task<IActionResult> Get_Company([FromBody] Get_Company_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    Company_Model result = await _DB_Helper.Get_Company(input_obj);

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




        [Route("api/DEV_Delete_Company")]
        [HttpPost]
        public async Task<IActionResult> DEV_Delete_Company([FromBody] DEV_Delete_Company_Data obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));
            }
            else
            {
                try
                {
                    string result = await _DB_Helper.DEV_Delete_Company(obj);

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
