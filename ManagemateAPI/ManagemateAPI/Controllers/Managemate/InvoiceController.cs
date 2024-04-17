using Microsoft.AspNetCore.Mvc;
using ManagemateAPI.Management.M_Invoice.Manager;
using ManagemateAPI.Management.M_Invoice.Table_Model;
using ManagemateAPI.Management.M_Invoice.Input_Objects;

namespace ManagemateAPI.Controllers.Managemate
{

    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private Invoice_Manager _DB_Helper;

        public InvoiceController(IConfiguration configuration)
        {
            _DB_Helper = new Invoice_Manager(configuration);
        }


        [Route("api/AddInvoice")]
        [HttpPost]
        public async Task<IActionResult> AddInvoice([FromBody] Add_Invoice_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.AddInvoice(obj);

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



        [Route("api/EditInvoice")]
        [HttpPost]
        public async Task<IActionResult> EditInvoice([FromBody] Edit_Invoice_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.EditInvoice(obj);

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



        [Route("api/DeleteInvoice")]
        [HttpPost]
        public async Task<IActionResult> DeleteInvoice([FromBody] Delete_Invoice_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.DeleteInvoice(obj);

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



        [Route("api/GetInvoices")]
        [HttpPost]
        public async Task<IActionResult> GetInvoices([FromBody] Get_Invoice_By_Page_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    List<Invoice_Model_List> result = await _DB_Helper.GetInvoices(obj);

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



        [Route("api/GetInvoiceById")]
        [HttpPost]
        public async Task<IActionResult> GetInvoiceById([FromBody] Get_Invoice_By_Id_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    Invoice_Model result = await _DB_Helper.GetInvoiceById(obj);

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
