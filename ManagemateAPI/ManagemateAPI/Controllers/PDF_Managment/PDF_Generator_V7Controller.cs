using DinkToPdf;
using DinkToPdf.Contracts;
using ManagemateAPI.Management.M_Session.Manager;
using Microsoft.AspNetCore.Mvc;

namespace ManagemateAPI.Controllers.PDF_Managment
{
    [ApiController]
    public class PDF_Generator_V7Controller : ControllerBase
    {

        private IConverter _converter;

        public PDF_Generator_V7Controller(IConverter converter)
        {
            _converter = converter;
        }



        [Route("api/Print_stringus")]
        [HttpPost]
        public async Task<IActionResult> Print_stringus([FromBody] PDF_Generator_input obj)
        {
            return BadRequest(html_stringus.html);
        }

        [Route("api/PDF_Generator")]
        [HttpPost]
        public async Task<IActionResult> PDF_Generator_V7([FromBody] PDF_Generator_input obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));
            }
            else
            {
                if (await Session_Checker.ActiveSession(obj.session))
                {
                    try
                    {
                        string outputLocalization = "/home/pdf_management/";
                        string outputFile = "/home/pdf_management/test_V6.pdf";
                        string htmlOrigin = "/home/pdf_management/funny_html.html";
                        //string htmlString1 = html_stringus.html;
                        string htmlString2 = "<b>Skill Issue</b>";

                        Random rnd = new Random();
                        string outputFileRandom = outputLocalization + rnd.Next() + "_test_V6.pdf";



                        //MemoryStream memoryStream = new MemoryStream();
                        //memoryStream.Flush();
                        //memoryStream.Position = 0;
                        //return new FileStreamResult(memoryStream, "application/pdf");

                        var options = new HtmlToPdfDocument()
                        {
                            GlobalSettings =
                            {
                                ColorMode = ColorMode.Color,
                                Orientation = Orientation.Portrait,
                                PaperSize = PaperKind.A4,
                                Out = outputFile
                            },
                            Objects =
                            {
                                new ObjectSettings
                                {
                                    PagesCount = true,
                                    HtmlContent = obj.html,
                                    WebSettings =
                                    {
                                        DefaultEncoding = "UTF-8"
                                    },
                                    //HeaderSettings =
                                    //{
                                    //    FontSize = 10,
                                    //    Line = true,
                                    //    Spacing = 3
                                    //}
                                }
                            }
                        };

                        _converter.Convert(options);
                        
                        return File(System.IO.File.OpenRead(outputFile), "application/octet-stream", Path.GetFileName(outputFile));
                        
                        /*
                        var globalSettings = new GlobalSettings
                        {
                            ColorMode = ColorMode.Color,
                            Orientation = Orientation.Portrait,
                            PaperSize = PaperKind.A4,
                        };
                        var objectSettings = new ObjectSettings
                        {
                            PagesCount = true,
                            HtmlContent = htmlString1,
                        };
                        var pdf = new HtmlToPdfDocument()
                        {
                            GlobalSettings = globalSettings,
                            Objects = { objectSettings }
                        };
                        var converter = new BasicConverter(new PdfTools());
                        byte[] pdfBytes = converter.Convert(pdf);
                        System.IO.File.WriteAllBytes(outputFileRandom, pdfBytes);
                        */
                    }
                    catch (Exception e)
                    {
                        return BadRequest(Response_Handler.GetExceptionResponse(e));
                    }
                }
                else
                {
                    return BadRequest(Response_Handler.GetExceptionResponse(new Exception("1")));
                }
            }
        }
    }
}