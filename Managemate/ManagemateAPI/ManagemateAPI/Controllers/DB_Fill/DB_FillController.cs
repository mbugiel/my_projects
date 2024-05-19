using Microsoft.AspNetCore.Mvc;
using ManagemateAPI.Management.M_Session.Input_Objects;
using ManagemateAPI.Database.Context;
using ManagemateAPI.Encryption.Input_Objects;
using ManagemateAPI.Encryption;
using ManagemateAPI.Database.Tables;

/*
 * This a controller is only for testing purposes used to fill the database of the specified user.
 * 
 * It accepts inputClass object.
 * 
 * That object contains session token (which contains user id) and integer that tells the function how much records it needs to add.
 */

namespace ManagemateAPI.Controllers.Managemate
{
    [ApiController]
    public class DB_FillController : ControllerBase
    {
        private DB_Context _context;
        private readonly IConfiguration _configuration;
        public DB_FillController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("api/FillDB")]
        [HttpPost]
        public async Task<IActionResult> FillDB([FromBody] inputClass obj)
        {
            if (obj == null)
            {
                return BadRequest(Response_Handler.GetExceptionResponse(new Exception("14")));//_14_NULL_ERROR
            }
            else
            {
                _context = new DB_Context(obj.session.userId, _configuration);

                List<Decrypted_Object> uncrypted_stringus = new List<Decrypted_Object>();

                for (int i = 0; i < obj.amount; i++)
                {
                    uncrypted_stringus.Add(new Decrypted_Object { id = i, decryptedValue = "stringus" + i });
                }

                List<Encrypted_Object> encrypted_stringus = await Crypto.EncryptList(obj.session, uncrypted_stringus);

                int iye = 0;

                foreach (var stringus in encrypted_stringus)
                {
                    _context.Item_On_Receipt.Add(new Item_On_Receipt
                    {
                        receipt_id_FK = new Receipt
                        {
                            in_out = false,
                            date = DateTime.UtcNow,
                            order_id_FK = new Order
                            {
                                order_name = stringus.encryptedValue,
                                client_id_FK = new Client
                                {
                                    name = stringus.encryptedValue,
                                    surname = stringus.encryptedValue,
                                    company_name = stringus.encryptedValue,
                                    NIP = stringus.encryptedValue,
                                    phone_number = stringus.encryptedValue,
                                    email = stringus.encryptedValue,
                                    address = stringus.encryptedValue,
                                    city_id_FK = new Cities_List { city = stringus.encryptedValue},
                                    postal_code = stringus.encryptedValue,
                                    comment = stringus.encryptedValue,
                                },
                                construction_site_id_FK = new Construction_Site
                                {
                                    construction_site_name = stringus.encryptedValue,
                                    address = stringus.encryptedValue,
                                    cities_list_id_FK = new Cities_List { city = stringus.encryptedValue },
                                    postal_code = stringus.encryptedValue,
                                    comment = stringus.encryptedValue,
                                },
                                status = (int)stringus.id,
                                creation_date = DateTime.UtcNow,
                                comment = stringus.encryptedValue
                            },
                            element = stringus.encryptedValue,
                            transport = stringus.encryptedValue,
                            summary_weight = stringus.id,
                            comment = stringus.encryptedValue
                        },
                        item_id_FK = new Item
                        {
                            catalog_number = "stringus" + stringus.id,
                            product_name = stringus.encryptedValue,
                            item_type_id_FK = new Item_Type { item_type = stringus.encryptedValue},
                            weight_kg = stringus.id,
                            count = stringus.id,
                            blocked_count = stringus.id,
                            price = stringus.encryptedValue,
                            tax_pct = stringus.id,
                            item_counting_type_id_FK = new Item_Counting_Type { counting_type = "stringus" + stringus.id},
                            item_trading_type_id_FK = _context.Item_Trading_Type.Where(y => y.id.Equals(1)).FirstOrDefault(),
                            comment = stringus.encryptedValue,
                        },
                        count = stringus.id,
                        summary_weight = stringus.id,
                        annotation = stringus.encryptedValue,
                    });

                    _context.Invoice.Add(new Invoice
                    {

                        prefix = "stringus" + stringus.id,
                        year = iye,
                        month = iye,
                        number = stringus.id,
                        order_id_FK = new Order
                        {
                            order_name = stringus.encryptedValue,
                            client_id_FK = new Client
                                {
                                    name = stringus.encryptedValue,
                                    surname = stringus.encryptedValue,
                                    company_name = stringus.encryptedValue,
                                    NIP = stringus.encryptedValue,
                                    phone_number = stringus.encryptedValue,
                                    email = stringus.encryptedValue,
                                    address = stringus.encryptedValue,
                                    city_id_FK = new Cities_List { city = stringus.encryptedValue},
                                    postal_code = stringus.encryptedValue,
                                    comment = stringus.encryptedValue,
                                },
                            construction_site_id_FK = new Construction_Site
                                {
                                    construction_site_name = stringus.encryptedValue,
                                    address = stringus.encryptedValue,
                                    cities_list_id_FK = new Cities_List { city = stringus.encryptedValue },
                                    postal_code = stringus.encryptedValue,
                                    comment = stringus.encryptedValue,
                                },
                            status = (int)stringus.id,
                            creation_date = DateTime.UtcNow,
                            comment = stringus.encryptedValue
                        },
                        issue_date = DateTime.UtcNow,
                        sale_date = DateTime.UtcNow,
                        payment_date = DateTime.UtcNow,
                        payment_method = stringus.encryptedValue,
                        discount = stringus.id,
                        net_worth = stringus.encryptedValue,
                        tax_worth = stringus.encryptedValue,
                        gross_worth = stringus.encryptedValue,
                        comment = stringus.encryptedValue,
                        comment_2 = stringus.encryptedValue,
                    });

                    _context.Authorized_Worker.Add(new Authorized_Worker
                    {
                        client_id_FK = new Client
                        {
                            name = stringus.encryptedValue,
                            surname = stringus.encryptedValue,
                            company_name = stringus.encryptedValue,
                            NIP = stringus.encryptedValue,
                            phone_number = stringus.encryptedValue,
                            email = stringus.encryptedValue,
                            address = stringus.encryptedValue,
                            city_id_FK = new Cities_List { city = stringus.encryptedValue },
                            postal_code = stringus.encryptedValue,
                            comment = stringus.encryptedValue,
                        },
                        name = stringus.encryptedValue,
                        surname = stringus.encryptedValue,
                        phone_number = stringus.encryptedValue,
                        email = stringus.encryptedValue,
                        contact = false,
                        collection = false,
                        comment = stringus.encryptedValue
                    });

                    iye++;
                }

                _context.SaveChanges();

                string result = "whatever";

                if (result == null)
                {
                    throw new Exception("14");//_14_NULL_ERROR
                }

                ResponseType responseType = ResponseType.Success;

                return Ok(Response_Handler.GetAppResponse(responseType, result));
            }
        }
    }
    public class inputClass
    {
        public Session_Data session { get; set; }
        public int amount { get; set; }
    }
}
