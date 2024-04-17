using ManagemateAPI.Database.Context;
using ManagemateAPI.Database.Tables;
using ManagemateAPI.Encryption;
using ManagemateAPI.Encryption.Input_Objects;
using ManagemateAPI.Information;
using ManagemateAPI.Management.M_Item_On_Receipt.Input_Objects;
using ManagemateAPI.Management.M_Item_On_Receipt.Manager;
using ManagemateAPI.Management.M_Item_On_Receipt.Table_Model;
using ManagemateAPI.Management.M_Receipt.Input_Objects;
using ManagemateAPI.Management.M_Receipt.Table_Model;
using ManagemateAPI.Management.M_Session.Manager;

namespace ManagemateAPI.Management.M_Receipt.Manager
{
    public class Receipt_Manager
    {

        private DB_Context _context;
        private readonly IConfiguration _configuration;


        public Receipt_Manager(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<string> AddReceipt(Add_Receipt_Data obj)
        {

            if (obj == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (await Session_Checker.ActiveSession(obj.session))
                {
                    _context = new DB_Context(obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    var order = _context.Order.Where(c => c.id.Equals(obj.order_id_FK)).FirstOrDefault();


                    if (order != null)
                    {

                        Receipt newOrder = new Receipt
                        {
                            in_out = obj.in_out,
                            date = DateTime.UtcNow,
                            order_id_FK = order,
                            element = await Crypto.Encrypt(obj.session, obj.element),
                            transport = await Crypto.Encrypt(obj.session, obj.transport),
                            summary_weight = 0,
                            comment = await Crypto.Encrypt(obj.session, obj.comment)

                        };

                        _context.Receipt.Add(newOrder);
                        _context.SaveChanges();

                        return Info.SUCCESSFULLY_ADDED;

                    }
                    else
                    {
                        throw new Exception("19");// objects not found
                    }



                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }

            }

        }


        public async Task<string> EditReceipt(Edit_Receipt_Data obj)
        {

            if (obj == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (await Session_Checker.ActiveSession(obj.session))
                {
                    _context = new DB_Context(obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    var editing_receipt = _context.Receipt.Where(c => c.id.Equals(obj.receipt_id)).FirstOrDefault();

                    if (editing_receipt == null)
                    {
                        throw new Exception("19");//Receipt not found
                    }
                    else
                    {
                        editing_receipt.date = obj.date;
                        editing_receipt.in_out = obj.in_out;
                        editing_receipt.element = await Crypto.Encrypt(obj.session, obj.element);
                        editing_receipt.transport = await Crypto.Encrypt(obj.session, obj.transport);
                        editing_receipt.comment = await Crypto.Encrypt(obj.session, obj.comment);

                        _context.SaveChanges();

                        return Info.SUCCESSFULLY_CHANGED;
                    }

                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }

            }

        }


        public async Task<string> DeleteReceipt(Delete_Receipt_Data obj)
        {

            if (obj == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (await Session_Checker.ActiveSession(obj.session))
                {
                    _context = new DB_Context(obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    var existing_receipt = _context.Receipt.Where(o => o.id.Equals(obj.receipt_id)).FirstOrDefault();

                    if (existing_receipt == null)
                    {
                        throw new Exception("19");//receipt not found
                    }
                    else
                    {
                        _context.Receipt.Remove(existing_receipt);
                        _context.SaveChanges();

                        return Info.SUCCESSFULLY_DELETED;
                    }

                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }

            }

        }


        public async Task<List<Receipt_Model_List>> GetReceipts(Get_Receipts_Data obj)
        {

            if (obj == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (await Session_Checker.ActiveSession(obj.session))
                {
                    _context = new DB_Context(obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    var order = _context.Order.Where(o => o.id.Equals(obj.order_id)).FirstOrDefault();

                    if (order == null)
                    {
                        throw new Exception("19"); // order not found
                    }
                    else
                    {

                        List<Receipt> receipts = _context.Receipt.Where(r => r.order_id_FK.Equals(order)).ToList();


                        if (receipts == null)
                        {
                            throw new Exception("19");//receipts not found
                        }
                        else
                        {

                            int page_length = obj.pageSize;
                            int start_position = obj.pageid * page_length;


                            if (receipts.Count() > start_position)
                            {

                                if (receipts.Count() >= start_position + page_length)
                                {
                                    receipts = receipts.Slice(start_position, page_length);

                                }
                                else
                                {
                                    int valid_length = receipts.Count() - start_position;
                                    receipts = receipts.Slice(start_position, valid_length);
                                }

                                List<Encrypted_Object> receipt_elements = new List<Encrypted_Object>();
                                List<Encrypted_Object> receipt_transports = new List<Encrypted_Object>();
                                List<Encrypted_Object> receipt_comments = new List<Encrypted_Object>();


                                List<Receipt_Model_List> receipts_decrypted = new List<Receipt_Model_List>();

                                foreach (var item in receipts)
                                {
                                    receipts_decrypted.Add(new Receipt_Model_List
                                    {
                                        id = item.id,
                                        in_out = item.in_out,
                                        date = item.date,
                                        summary_weight = item.summary_weight

                                    });

                                    receipt_elements.Add(new Encrypted_Object { id = item.id, encryptedValue = item.element });
                                    receipt_transports.Add(new Encrypted_Object { id = item.id, encryptedValue = item.transport });
                                    receipt_comments.Add(new Encrypted_Object { id = item.id, encryptedValue = item.comment });


                                }

                                List<Decrypted_Object> receipt_elements_decrypted = await Crypto.DecryptList(obj.session, receipt_elements);
                                List<Decrypted_Object> receipt_transports_decrypted = await Crypto.DecryptList(obj.session, receipt_transports);
                                List<Decrypted_Object> receipt_comments_decrypted = await Crypto.DecryptList(obj.session, receipt_comments);

                                foreach (var item in receipts_decrypted)
                                {
                                    var element = receipt_elements_decrypted.Where(o => o.id.Equals(item.id)).FirstOrDefault();

                                    if (element == null)
                                    {
                                        throw new Exception("3");//error while decrypting data 
                                    }
                                    else
                                    {
                                        item.element = element.decryptedValue;
                                    }


                                    var transport = receipt_transports_decrypted.Where(o => o.id.Equals(item.id)).FirstOrDefault();

                                    if (transport == null)
                                    {
                                        throw new Exception("3");//error while decrypting data 
                                    }
                                    else
                                    {
                                        item.transport = transport.decryptedValue;
                                    }


                                    var comment = receipt_comments_decrypted.Where(o => o.id.Equals(item.id)).FirstOrDefault();

                                    if (comment == null)
                                    {
                                        throw new Exception("3");//error while decrypting data 
                                    }
                                    else
                                    {
                                        item.comment = comment.decryptedValue;
                                    }


                                }

                                return receipts_decrypted;



                            }
                            else
                            {
                                throw new Exception("19");//no more orders
                            }



                        }

                    }





                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }

            }

        }



        public async Task<List<Receipt_Model_List>> Get_Receipt_List_In_Out(Get_Receipt_List_Data obj)
        {

            if (obj == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (await Session_Checker.ActiveSession(obj.session))
                {
                    _context = new DB_Context(obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    var order = _context.Order.Where(o => o.id.Equals(obj.order_id)).FirstOrDefault();

                    if (order == null)
                    {
                        throw new Exception("19"); // order not found
                    }
                    else
                    {

                        List<Receipt> receipts = _context.Receipt.Where(r => r.order_id_FK.Equals(order) && r.in_out.Equals(obj.in_out)).ToList();


                        if (receipts == null)
                        {
                            throw new Exception("19");//receipts not found
                        }
                        else
                        {

                            

                            List<Encrypted_Object> receipt_elements = new List<Encrypted_Object>();
                            List<Encrypted_Object> receipt_transports = new List<Encrypted_Object>();
                            List<Encrypted_Object> receipt_comments = new List<Encrypted_Object>();


                            List<Receipt_Model_List> receipts_decrypted = new List<Receipt_Model_List>();

                            foreach (var item in receipts)
                            {
                                receipts_decrypted.Add(new Receipt_Model_List
                                {
                                    id = item.id,
                                    in_out = item.in_out,
                                    date = item.date,
                                    summary_weight = item.summary_weight

                                });

                                receipt_elements.Add(new Encrypted_Object { id = item.id, encryptedValue = item.element });
                                receipt_transports.Add(new Encrypted_Object { id = item.id, encryptedValue = item.transport });
                                receipt_comments.Add(new Encrypted_Object { id = item.id, encryptedValue = item.comment });


                            }

                            List<Decrypted_Object> receipt_elements_decrypted = await Crypto.DecryptList(obj.session, receipt_elements);
                            List<Decrypted_Object> receipt_transports_decrypted = await Crypto.DecryptList(obj.session, receipt_transports);
                            List<Decrypted_Object> receipt_comments_decrypted = await Crypto.DecryptList(obj.session, receipt_comments);

                            foreach (var item in receipts_decrypted)
                            {
                                var element = receipt_elements_decrypted.Where(o => o.id.Equals(item.id)).FirstOrDefault();

                                if (element == null)
                                {
                                    throw new Exception("3");//error while decrypting data 
                                }
                                else
                                {
                                    item.element = element.decryptedValue;
                                }


                                var transport = receipt_transports_decrypted.Where(o => o.id.Equals(item.id)).FirstOrDefault();

                                if (transport == null)
                                {
                                    throw new Exception("3");//error while decrypting data 
                                }
                                else
                                {
                                    item.transport = transport.decryptedValue;
                                }


                                var comment = receipt_comments_decrypted.Where(o => o.id.Equals(item.id)).FirstOrDefault();

                                if (comment == null)
                                {
                                    throw new Exception("3");//error while decrypting data 
                                }
                                else
                                {
                                    item.comment = comment.decryptedValue;
                                }


                            }

                            return receipts_decrypted;



                        }

                    }





                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }

            }

        }


        public async Task<Receipt_Model> GetReceiptById(Get_Receipt_By_Id_Data obj)
        {

            if (obj == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (await Session_Checker.ActiveSession(obj.session))
                {
                    _context = new DB_Context(obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    var selected_receipt = _context.Receipt.Where(r => r.id.Equals(obj.receipt_id)).FirstOrDefault();

                    if (selected_receipt == null)
                    {
                        throw new Exception("19");//receipt not found
                    }
                    else
                    {

                        List<Encrypted_Object> encrypted_fields =
                        [
                            //encrypted fields in Receipt table
                            new Encrypted_Object { id = 0, encryptedValue = selected_receipt.element },
                            new Encrypted_Object { id = 1, encryptedValue = selected_receipt.transport },
                            new Encrypted_Object { id = 2, encryptedValue = selected_receipt.comment }
                        ];

                        List<Decrypted_Object> decrypted_fields = await Crypto.DecryptList(obj.session, encrypted_fields);

                        Receipt_Model receipt_model = new Receipt_Model
                        {
                            id = selected_receipt.id,
                            in_out = selected_receipt.in_out,
                            date = selected_receipt.date,
                            summary_weight = selected_receipt.summary_weight
                        };

                        Item_On_Receipt_Manager Receipt_Helper = new Item_On_Receipt_Manager(_configuration);
                        Get_Items_On_Receipt_Data get_data = new Get_Items_On_Receipt_Data { session = obj.session, receipt_id = selected_receipt.id };

                        List<Item_On_Receipt_Model> items_on_selected_receipt = await Receipt_Helper.GetItemsOnReceipt(get_data);

                        receipt_model.items_on_receipt = items_on_selected_receipt;

                        foreach (var item in decrypted_fields)
                        {
                            if (item == null)
                            {
                                throw new Exception("3");//error while decrypting data 
                            }
                            else
                            {

                                switch (item.id)
                                {
                                    case 0:
                                        receipt_model.element = item.decryptedValue; break;

                                    case 1:
                                        receipt_model.transport = item.decryptedValue; break;

                                    case 2:
                                        receipt_model.comment = item.decryptedValue; break;

                                    default:
                                        throw new Exception("3");//error while decrypting data 

                                }

                            }

                        }



                        return receipt_model;

                    }

                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }

            }

        }

    }
}
