using Microsoft.AspNetCore.Mvc;
using ManagemateAPI.Management.M_Invoice.Manager;
using ManagemateAPI.Management.M_Invoice.Table_Model;
using ManagemateAPI.Management.M_Invoice.Input_Objects;
using ManagemateAPI.Management.M_Item_On_Receipt.Table_Model;
using DinkToPdf;
using ManagemateAPI.Information;
using ManagemateAPI.Management.M_Session.Input_Objects;
using Microsoft.Extensions.Configuration;
using DinkToPdf.Contracts;

/*
 * This is an endpoint controller dedicated to the Invoice table.
 * 
 * It contains methods for endpoints
 * - Add 
 * - Edit
 * - Delete
 * - Get by ID
 */
namespace ManagemateAPI.Controllers.Managemate
{

    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private Invoice_Manager _DB_Helper;
        public InvoiceController(IConfiguration _configuration, IConverter _converter)
        {
            _DB_Helper = new Invoice_Manager(_configuration, _converter);
        }
        /*
         * Add_Invoice endpoint
         * This endpoint is used to add a record to the Invoice table.
         * 
         * It accepts Add_Invoice_Data object.
         * The given object is handed over to the Add_Invoice method in the Invoice_Manager.
         */
        [Route("api/Add_Invoice")]
        [HttpPost]
        public async Task<IActionResult> Add_Invoice([FromBody] Add_Invoice_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.Add_Invoice(obj);

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
         * Edit_Invoice endpoint
         * This endpoint is used to edit a record from the Invoice table.
         * 
         * It accepts Edit_Invoice_Data object.
         * The given object is handed over to the Edit_Invoice method in the Invoice_Manager.
         */
        [Route("api/Edit_Invoice")]
        [HttpPost]
        public async Task<IActionResult> Edit_Invoice([FromBody] Edit_Invoice_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.Edit_Invoice(obj);

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
         * Delete_Invoice endpoint
         * This endpoint is used to remove record from the Invoice table.
         * 
         * It accepts Delete_Invoice_Data object.
         * The given object is handed over to the Delete_Invoice method in the Invoice_Manager.
         */
        [Route("api/Delete_Invoice")]
        [HttpPost]
        public async Task<IActionResult> Delete_Invoice([FromBody] Delete_Invoice_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    string result = await _DB_Helper.Delete_Invoice(obj);

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
         * Get_Invoice_By_ID endpoint
         * This endpoint is used to get a record from to the Invoice table by its ID.
         * 
         * It accepts Get_Invoice_By_ID object.
         * The given object is handed over to the Get_Invoice_By_ID method in the Invoice_Manager.
         */
        [Route("api/Get_Invoice_By_ID")]
        [HttpPost]
        public async Task<IActionResult> Get_Invoice_By_ID([FromBody] Get_Invoice_By_ID_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    Invoice_Model result = await _DB_Helper.Get_Invoice_By_ID(obj);

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
         * Get_Invoice_Available_List endpoint
         * This endpoint is used to get a list of month-and-year sets, that shows available dates invoices can be issued from.
         * 
         * It accepts Get_Invoice_Available_List_Data object.
         * The given object is handed over to the Get_Invoice_Available_List method in the Invoice_Manager.
         */
        [Route("api/Get_Invoice_Available_List")]
        [HttpPost]
        public async Task<IActionResult> Get_Invoice_Available_List([FromBody] Get_Invoice_Available_List_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    List<Invoice_Available> result = await _DB_Helper.Get_Invoice_Available_List(obj);

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






        [Route("api/Calculate_Lease_Value")]
        [HttpPost]
        public async Task<IActionResult> Calculate_Lease_Value([FromBody] Calculate_Lease_Value_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    List<Calculate_Lease_Value_Model_List> result = await _DB_Helper.Calculate_Lease_Value(obj);

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


        [Route("api/Calculate_Sale_And_Service_Value")]
        [HttpPost]
        public async Task<IActionResult> Calculate_Sale_And_Service_Value([FromBody] Calculate_Lease_Value_Data obj)
        {

            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {

                    List<Calculate_Sale_And_Service_Value_Model_List> result = await _DB_Helper.Calculate_Sale_And_Service_Value(obj);

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
          * Issue_An_Invoice endpoint
          * This endpoint is used to issue an invoice based on the input.
          * 
          * It accepts object.
          * The given object is handed over to the Issue_An_Invoice method in the Invoice_Manager.
          */
        [Route("api/Invoice_Issuer")]
        [HttpPost]
        public async Task<IActionResult> Invoice_Issuer([FromBody] Invoice_Issuer_Data obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                try
                {
                    string Invoice_HTML_String = await _DB_Helper.Invoice_Issuer(obj);

                    if (Invoice_HTML_String == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }

                    return File(System.IO.File.OpenRead(Invoice_HTML_String), "application/pdf", Path.GetFileName(Invoice_HTML_String));
                }
                catch (Exception e)
                {
                    return BadRequest(Response_Handler.GetExceptionResponse(e));
                }
            }
        }
    }
}
