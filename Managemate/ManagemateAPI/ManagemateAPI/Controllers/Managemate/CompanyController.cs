using Microsoft.AspNetCore.Mvc;
using ManagemateAPI.Management.M_Company.Input_Objects;
using ManagemateAPI.Management.M_Company.Manager;
using ManagemateAPI.Management.M_Company.Table_Model;

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


        [Route("api/AddCompanyData")]
        [HttpPost]
        public async Task<IActionResult> AddCompanyData([FromBody] Add_Company_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.AddCompanyData(input_obj);

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



        [Route("api/EditCompanyData")]
        [HttpPost]
        public async Task<IActionResult> EditCompanyData([FromBody] Edit_Company_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.EditCompanyData(input_obj);

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



        [Route("api/GetCompanyData")]
        [HttpPost]
        public async Task<IActionResult> GetCompanyData([FromBody] Get_Company_Data input_obj)
        {

            if (input_obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    Company_Model result = await _DB_Helper.GetCompanyData(input_obj);

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
