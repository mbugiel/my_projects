using Microsoft.AspNetCore.Mvc;
using ManagemateAPI.Management.M_Construction_Site.Input_Objects;
using ManagemateAPI.Management.M_Construction_Site.Manager;
using ManagemateAPI.Management.M_Construction_Site.Table_Model;

namespace ManagemateAPI.Controllers.Managemate
{

    [ApiController]
    public class Construction_SiteController : ControllerBase
    {
        private Construction_Site_Manager _DB_Helper;
        public Construction_SiteController(IConfiguration configuration)
        {
            _DB_Helper = new Construction_Site_Manager(configuration);
        }

        [Route("api/Add_ConstructionSite")]
        [HttpPost]
        public async Task<IActionResult> Add_ConstructionSite([FromBody] Add_Construction_Site_Data obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {
                    string result = await _DB_Helper.Add_ConstructionSite(obj);

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

        [Route("api/Edit_ConstructionSite")]
        [HttpPost]
        public async Task<IActionResult> Edit_ConstructionSite([FromBody] Edit_Construction_Site_Data obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {
                    string result = await _DB_Helper.Edit_ConstructionSite(obj);

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

        [Route("api/Delete_ConstructionSite")]
        [HttpPost]
        public async Task<IActionResult> Delete_ConstructionSite([FromBody] Delete_Construction_Site_Data obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));
            }
            else
            {
                try
                {
                    string result = await _DB_Helper.Delete_ConstructionSite(obj);

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

        [Route("api/Get_Construction_Site_By_ID")]
        [HttpPost]
        public async Task<IActionResult> Get_Construction_Site_By_ID([FromBody] Get_Construction_Site_By_ID obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));
            }
            else
            {
                try
                {
                    Construction_Site_Model result = await _DB_Helper.Get_Construction_Site_By_ID(obj);

                    if (result == null)
                    {
                        throw new Exception("14");
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

        [Route("api/Get_Construction_Sites")]
        [HttpPost]
        public async Task<IActionResult> Get_Construction_Sites([FromBody] Get_Construction_Sites obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));
            }
            else
            {
                try
                {
                    List<Construction_Site_Model_List> result = await _DB_Helper.Get_Construction_Sites(obj);

                    if (result == null)
                    {
                        throw new Exception("14");
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
