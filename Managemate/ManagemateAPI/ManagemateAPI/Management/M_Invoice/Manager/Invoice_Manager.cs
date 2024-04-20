using ManagemateAPI.Database.Context;
using ManagemateAPI.Encryption;
using ManagemateAPI.Database.Tables;
using ManagemateAPI.Encryption.Input_Objects;
using ManagemateAPI.Management.M_Session.Manager;
using ManagemateAPI.Information;
using ManagemateAPI.Management.M_Invoice.Input_Objects;
using ManagemateAPI.Management.M_Invoice.Table_Model;
using Microsoft.EntityFrameworkCore;

namespace ManagemateAPI.Management.M_Invoice.Manager
{
    public class Invoice_Manager
    {

        private DB_Context _context;
        private readonly IConfiguration _configuration;


        public Invoice_Manager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //Add
        public async Task<string> AddInvoice(Add_Invoice_Data obj)
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

                    var order = _context.Order.Where(o => o.id.Equals(obj.order_id_FK)).FirstOrDefault();


                    if (order != null)
                    {

                        var invoice_exits = _context.Invoice.Where(i => i.order_id_FK.Equals(order) && i.issue_date.Equals(obj.issue_date)).FirstOrDefault();

                        if (invoice_exits != null)
                        {
                            throw new Exception("18");// that invoice already exists
                        }
                        else
                        {

                            Invoice new_record = new Invoice
                            {
                                prefix = obj.prefix,
                                year = obj.year,
                                month = obj.month,
                                number = obj.number,
                                order_id_FK = order,
                                issue_date = obj.issue_date,
                                sale_date = obj.sale_date,
                                payment_date = obj.payment_date,
                                discount = obj.discount
                            };

                            List<Decrypted_Object> decrypted_fields = new List<Decrypted_Object>
                            {
                                new Decrypted_Object {id = 0, decryptedValue = obj.payment_method },
                                new Decrypted_Object { id = 1, decryptedValue = obj.net_worth.ToString() },
                                new Decrypted_Object {id = 2, decryptedValue=obj.tax_worth.ToString()},
                                new Decrypted_Object {id = 3, decryptedValue = obj.gross_worth.ToString()},
                                new Decrypted_Object {id = 4, decryptedValue = obj.comment},
                                new Decrypted_Object {id = 5, decryptedValue = obj.comment_2}
                            };

                            List<Encrypted_Object> encrypted_fields = await Crypto.EncryptList(obj.session, decrypted_fields);

                            if (encrypted_fields == null)
                            {
                                throw new Exception("2");//encryption error
                            }
                            else
                            {
                                foreach (var field in encrypted_fields)
                                {
                                    if (field.encryptedValue == null)
                                    {
                                        throw new Exception("2");//encryption error
                                    }
                                    else
                                    {

                                        switch (field.id)
                                        {
                                            case 0:
                                                new_record.payment_method = field.encryptedValue; break;

                                            case 1:
                                                new_record.net_worth = field.encryptedValue; break;

                                            case 2:
                                                new_record.tax_worth = field.encryptedValue; break;

                                            case 3:
                                                new_record.gross_worth = field.encryptedValue; break;

                                            case 4:
                                                new_record.comment = field.encryptedValue; break;

                                            case 5:
                                                new_record.comment_2 = field.encryptedValue; break;

                                            default:
                                                throw new Exception("2");//encryption error

                                        }

                                    }

                                }



                            }


                            _context.Invoice.Add(new_record);
                            _context.SaveChanges();

                            return Info.SUCCESSFULLY_ADDED;

                        }
                    }
                    else
                    {
                        throw new Exception("19");// Order not found
                    }


                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }

            }

        }

        //Edit
        public async Task<string> EditInvoice(Edit_Invoice_Data obj)
        {
            if (obj == null)
            {
                throw new Exception("14");
            }
            else
            {
                if (await Session_Checker.ActiveSession(obj.session))
                {
                    _context = new DB_Context(obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();


                    var edited_record = _context.Invoice.Where(o => o.id.Equals(obj.id)).FirstOrDefault();


                    if (edited_record != null)
                    {

                        var invoice_exits = _context.Invoice.Where(i => i.order_id_FK.Equals(edited_record.order_id_FK) && i.issue_date.Equals(obj.issue_date)).FirstOrDefault();

                        if (invoice_exits != null)
                        {
                            throw new Exception("18");// that invoice already exists
                        }
                        else
                        {



                            edited_record.prefix = obj.prefix;
                            edited_record.year = obj.year;
                            edited_record.month = obj.month;
                            edited_record.number = obj.number;
                            edited_record.issue_date = obj.issue_date;
                            edited_record.sale_date = obj.sale_date;
                            edited_record.payment_date = obj.payment_date;
                            edited_record.discount = obj.discount;


                            List<Decrypted_Object> decrypted_fields = new List<Decrypted_Object>
                            {
                                new Decrypted_Object {id = 0, decryptedValue = obj.payment_method },
                                new Decrypted_Object { id = 1, decryptedValue = obj.net_worth.ToString() },
                                new Decrypted_Object {id = 2, decryptedValue=obj.tax_worth.ToString()},
                                new Decrypted_Object {id = 3, decryptedValue = obj.gross_worth.ToString()},
                                new Decrypted_Object {id = 4, decryptedValue = obj.comment},
                                new Decrypted_Object {id = 5, decryptedValue = obj.comment_2}
                            };

                            List<Encrypted_Object> encrypted_fields = await Crypto.EncryptList(obj.session, decrypted_fields);

                            if (encrypted_fields == null)
                            {
                                throw new Exception("2");//encryption error
                            }
                            else
                            {
                                foreach (var field in encrypted_fields)
                                {
                                    if (field.encryptedValue == null)
                                    {
                                        throw new Exception("2");//encryption error
                                    }
                                    else
                                    {

                                        switch (field.id)
                                        {
                                            case 0:
                                                edited_record.payment_method = field.encryptedValue; break;

                                            case 1:
                                                edited_record.net_worth = field.encryptedValue; break;

                                            case 2:
                                                edited_record.tax_worth = field.encryptedValue; break;

                                            case 3:
                                                edited_record.gross_worth = field.encryptedValue; break;

                                            case 4:
                                                edited_record.comment = field.encryptedValue; break;

                                            case 5:
                                                edited_record.comment_2 = field.encryptedValue; break;

                                            default:
                                                throw new Exception("2");//encryption error

                                        }

                                    }

                                }



                            }

                            _context.SaveChanges();

                            return Info.SUCCESSFULLY_CHANGED;

                        }
                    }
                    else
                    {
                        throw new Exception("19");// edited invoice not found
                    }

                }
                else
                {
                    throw new Exception("1");// session not found
                }
            }
        }

        //Delete
        public async Task<string> DeleteInvoice(Delete_Invoice_Data obj)
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

                    var id_exits = _context.Invoice.Where(i => i.id.Equals(obj.invoice_id)).FirstOrDefault();

                    if (id_exits == null)
                    {
                        throw new Exception("19");// invoice not found in db
                    }
                    else
                    {

                        _context.Invoice.Remove(id_exits);
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

        //Get by ID
        public async Task<Invoice_Model> GetInvoiceById(Get_Invoice_By_Id_Data obj)
        {
            if (obj == null)
            {
                throw new Exception("14");
            }
            else
            {
                if (await Session_Checker.ActiveSession(obj.session))
                {
                    _context = new DB_Context(obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    var selected_record = _context.Invoice.Include(i => i.order_id_FK.order_name).Where(i => i.id.Equals(obj.id_to_get)).FirstOrDefault();

                    if (selected_record == null)
                    {
                        throw new Exception("19"); //Invoice not found in DB
                    }
                    else
                    {
                        List<Encrypted_Object> encrypted_fields = [
                            new Encrypted_Object { id = 0, encryptedValue = selected_record.payment_method },
                            new Encrypted_Object { id = 1, encryptedValue = selected_record.net_worth },
                            new Encrypted_Object { id = 2, encryptedValue = selected_record.tax_worth },
                            new Encrypted_Object { id = 3, encryptedValue = selected_record.gross_worth },
                            new Encrypted_Object { id = 4, encryptedValue = selected_record.comment },
                            new Encrypted_Object { id = 5, encryptedValue = selected_record.comment_2 },
                            new Encrypted_Object { id = 6, encryptedValue = selected_record.order_id_FK.order_name }
                        ];

                        List<Decrypted_Object> decrypted_fields = await Crypto.DecryptList(obj.session, encrypted_fields);

                        Invoice_Model return_obj = new Invoice_Model
                        {
                            id = selected_record.id,
                            prefix = selected_record.prefix,
                            year = selected_record.year,
                            month = selected_record.month,
                            number = selected_record.number,
                            issue_date = selected_record.issue_date,
                            sale_date = selected_record.sale_date,
                            payment_date = selected_record.payment_date,
                            discount = selected_record.discount
                        };

                        foreach (var field in decrypted_fields)
                        {
                            if (field.decryptedValue == null)
                            {
                                throw new Exception("3");//decryption error
                            }
                            else
                            {
                                switch (field.id)
                                {
                                    case 0:
                                        return_obj.payment_method = field.decryptedValue; break;
                                    case 1:
                                        return_obj.net_worth = Convert.ToDouble(field.decryptedValue); break;
                                    case 2:
                                        return_obj.tax_worth = Convert.ToDouble(field.decryptedValue); break;
                                    case 3:
                                        return_obj.gross_worth = Convert.ToDouble(field.decryptedValue); break;
                                    case 4:
                                        return_obj.comment = field.decryptedValue; break;
                                    case 5:
                                        return_obj.comment_2 = field.decryptedValue; break;
                                    case 6:
                                        return_obj.order_name_FK = field.decryptedValue; break;
                                    default:
                                        throw new Exception("3");
                                }
                            }
                        }
                        return return_obj;
                    }
                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }
            }
        }

        //Get by Page
        public async Task<List<Invoice_Model_List>> GetInvoices(Get_Invoice_By_Page_Data obj)
        {
            if (obj == null)
            {
                throw new Exception("14");
            }
            else
            {
                if (await Session_Checker.ActiveSession(obj.session))
                {
                    _context = new DB_Context(obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    List<Invoice> records_list = _context.Invoice.Include(i => i.order_id_FK.order_name).ToList();

                    if (records_list == null)
                    {
                        throw new Exception("19");// no invoices found
                    }
                    else
                    {
                        int page_lenght = obj.page_Size;
                        int start_position = obj.page_ID * page_lenght;

                        if (records_list.Count() > start_position)
                        {
                            if (records_list.Count() >= start_position + page_lenght)
                            {
                                records_list = records_list.Slice(start_position, page_lenght);
                            }
                            else
                            {
                                int valid_lenght = records_list.Count() - start_position;

                                records_list = records_list.Slice(start_position, valid_lenght);
                            }

                            List<Encrypted_Object> order_name_list = new List<Encrypted_Object>();
                            List<Encrypted_Object> net_worth_list = new List<Encrypted_Object>();
                            List<Encrypted_Object> gross_worth_list = new List<Encrypted_Object>();
                            List<Encrypted_Object> comment_list = new List<Encrypted_Object>();

                            List<Invoice_Model_List> records_decrypted = new List<Invoice_Model_List>();

                            foreach (var field in records_list)
                            {
                                records_decrypted.Add(new Invoice_Model_List
                                {
                                    id = field.id,
                                    prefix = field.prefix,
                                    year = field.year,
                                    month = field.month,
                                    number = field.number,
                                    issue_date = field.issue_date,
                                    sale_date = field.sale_date
                                });

                                order_name_list.Add(new Encrypted_Object { id = field.id, encryptedValue = field.order_id_FK.order_name });

                                net_worth_list.Add(new Encrypted_Object { id = field.id, encryptedValue = field.net_worth });

                                gross_worth_list.Add(new Encrypted_Object { id = field.id, encryptedValue = field.gross_worth });

                                comment_list.Add(new Encrypted_Object { id = field.id, encryptedValue = field.comment });

                            }

                            List<Decrypted_Object> order_name_list_decrypted = await Crypto.DecryptList(obj.session, order_name_list);
                            List<Decrypted_Object> net_worth_list_decrypted = await Crypto.DecryptList(obj.session, net_worth_list);
                            List<Decrypted_Object> gross_worth_list_decrypted = await Crypto.DecryptList(obj.session, gross_worth_list);
                            List<Decrypted_Object> comment_list_decrypted = await Crypto.DecryptList(obj.session, comment_list);


                            foreach (var item in records_decrypted)
                            {
                                var order_name = order_name_list_decrypted.Where(s => s.id.Equals(item.id)).FirstOrDefault();
                                if (order_name == null)
                                {
                                    throw new Exception("3");//decryption error
                                }
                                else
                                {
                                    item.order_name_FK = order_name.decryptedValue;
                                }

                                var net_worth = net_worth_list_decrypted.Where(s => s.id.Equals(item.id)).FirstOrDefault();
                                if (net_worth == null)
                                {
                                    throw new Exception("3");//decryption error
                                }
                                else
                                {
                                    item.net_worth = Convert.ToDouble(net_worth.decryptedValue);
                                }

                                var gross_worth = gross_worth_list_decrypted.Where(s => s.id.Equals(item.id)).FirstOrDefault();
                                if (gross_worth == null)
                                {
                                    throw new Exception("3");//decryption error
                                }
                                else
                                {
                                    item.gross_worth = Convert.ToDouble(gross_worth.decryptedValue);
                                }

                                var comment = comment_list_decrypted.Where(s => s.id.Equals(item.id)).FirstOrDefault();
                                if (comment == null)
                                {
                                    throw new Exception("3");//decryption error
                                }
                                else
                                {
                                    item.comment = comment.decryptedValue;
                                }


                            }
                            return records_decrypted;
                        }
                        else
                        {
                            throw new Exception("19");// no more invoices
                        }
                    }
                }
                else
                {
                    throw new Exception("1");
                }
            }
        }

    }
}
