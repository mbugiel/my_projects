using ManagemateAPI.Database.Context;
using ManagemateAPI.Encryption;
using ManagemateAPI.Database.Tables;
using ManagemateAPI.Encryption.Input_Objects;
using ManagemateAPI.Management.M_Session.Manager;
using ManagemateAPI.Information;
using ManagemateAPI.Management.M_Invoice.Input_Objects;
using ManagemateAPI.Management.M_Invoice.Table_Model;
using Microsoft.EntityFrameworkCore;
using ManagemateAPI.Management.M_Item_On_Receipt.Table_Model;
using System.Globalization;
using System.Text.Json;
using ManagemateAPI.Helper.InputObjects.Client;
using ManagemateAPI.Management.M_Client.Manager;
using ManagemateAPI.Management.M_Company.Input_Objects;
using ManagemateAPI.Management.M_Company.Manager;
using Humanizer;
using ManagemateAPI.Management.M_Session.Input_Objects;
using DinkToPdf;
using System;
using DinkToPdf.Contracts;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.InteropServices.JavaScript;
using ManagemateAPI.Management.M_Invoice.Invoice_Issuer.Models;
using ManagemateAPI.Management.M_Invoice.Invoice_Issuer.Language;
using ManagemateAPI.Management.M_Invoice.Invoice_Issuer.Manager;

/*
 * This is the Invoice_Manager with methods dedicated to the Invoice table.
 * 
 * It contains methods to:
 * add records,
 * edit records,
 * delete records,
 * get record by id,
 */
namespace ManagemateAPI.Management.M_Invoice.Manager
{
    public class Invoice_Manager
    {

        private DB_Context _context;
        private readonly IConfiguration _configuration;
        private IConverter _converter;

        public Invoice_Manager(IConfiguration configuration, IConverter converter)
        {
            _configuration = configuration;
            _converter = converter;
        }


        /* 
         * Add_Invoice method
         * This method is used to add new records to the Invoice table.
         * 
         * It accepts Add_Invoice_Data object as input.
         * It then adds new record with values based on the data given in the input object.
         */
        public async Task<string> Add_Invoice(Add_Invoice_Data obj)
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

                        var invoice_exits = _context.Invoice.Where(i => i.order_id_FK.Equals(order) && i.sale_lease.Equals(obj.sale_lease) && i.month.Equals(obj.month) && i.year.Equals(obj.year)).FirstOrDefault();

                        if (invoice_exits != null)
                        {
                            throw new Exception("18 kurwa");// that invoice already exists
                        }
                        else
                        {

                            Invoice new_record = new Invoice
                            {
                                prefix = obj.prefix,
                                sale_lease = obj.sale_lease,
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
                                new Decrypted_Object {id = 2, decryptedValue = obj.tax_worth.ToString()},
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




        /* 
         * Edit_Invoice method
         * This method is used to edit a record in the Invoice table.
         * 
         * It accepts Edit_Invoice_Data object as input.
         * It then changes values of a record with those given in the input object only if its ID matches the one in the input object.
         */
        public async Task<string> Edit_Invoice(Edit_Invoice_Data obj)
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

                        var invoice_exits = _context.Invoice.Where(i => i.order_id_FK.Equals(edited_record.order_id_FK) && i.sale_lease.Equals(obj.sale_lease) && i.month.Equals(obj.month) && i.year.Equals(obj.year)).FirstOrDefault();

                        if (invoice_exits != null)
                        {
                            throw new Exception("18");// that invoice already exists
                        }
                        else
                        {



                            edited_record.prefix = obj.prefix;
                            edited_record.sale_lease = obj.sale_lease;
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
                                new Decrypted_Object {id = 2, decryptedValue = obj.tax_worth.ToString()},
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




        /*
         * Delete_Invoice method
         * This method is used to a record from the Invoice table.
         *  
         * It accepts Delete_Invoice_Data object as input.
         * Then it deletes a record if its ID matches the one given in the input object.
         */
        public async Task<string> Delete_Invoice(Delete_Invoice_Data obj)
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




        /*
         * Get_Invoice_By_ID method
         * This method gets a record from the Invoice table by its ID and returns it.
         * 
         * It accepts Get_Invoice_By_ID_Data object as input.
         * Then it gets a records that has the same ID as the ID given in the input object
         */
        public async Task<Invoice_Model> Get_Invoice_By_ID(Get_Invoice_By_ID_Data obj)
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

                    var selected_record = _context.Invoice.Where(i => i.id.Equals(obj.id_to_get)).Include(i => i.order_id_FK).FirstOrDefault();

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
                            new Encrypted_Object { id = 5, encryptedValue = selected_record.comment_2 },
                            new Encrypted_Object { id = 6, encryptedValue = selected_record.order_id_FK.order_name }
                        ];



                        Invoice_Model return_obj = new Invoice_Model
                        {
                            id = selected_record.id,
                            prefix = selected_record.prefix,
                            sale_lease = selected_record.sale_lease,
                            year = selected_record.year,
                            month = selected_record.month,
                            number = selected_record.number,
                            issue_date = selected_record.issue_date,
                            sale_date = selected_record.sale_date,
                            payment_date = selected_record.payment_date,
                            discount = selected_record.discount
                        };

                        if (selected_record.comment != null)
                        {
                            encrypted_fields.Add(new Encrypted_Object { id = 4, encryptedValue = selected_record.comment });
                        }
                        else
                        {
                            return_obj.comment = "";
                        }

                        List<Decrypted_Object> decrypted_fields = await Crypto.DecryptList(obj.session, encrypted_fields);

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




        /*
         * Get_Invoice_Available_List method
         * This method gets month and year values from the Receipts assigned to given Order ID and returns it in a list.
         * 
         * It accepts Get_Invoice_Available_List_Data object as input.
         * Then it puts unique month-and-year sets in a list, to see which month invoices can be issued from.
         */
        public async Task<List<Invoice_Available>> Get_Invoice_Available_List(Get_Invoice_Available_List_Data obj)
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

                    var selected_order = _context.Order.Where(o => o.id.Equals(obj.order_id)).Include(o => o.receipts_FK).ThenInclude(r => r.items_on_receipt_FK).ThenInclude(i => i.item_id_FK).ThenInclude(it => it.item_trading_type_id_FK).FirstOrDefault();

                    if (selected_order != null)
                    {

                        List<Receipt> order_receipts = selected_order.receipts_FK;

                        List<Invoice_Available> return_list = new List<Invoice_Available>();

                        if(order_receipts.Count > 0)
                        {


                            foreach(Receipt receipt in order_receipts)
                            {

                                if(receipt.items_on_receipt_FK.Count > 0)
                                {
                                    bool found_lease_item = false;
                                    bool found_sale_item = false;

                                    foreach(Item_On_Receipt item in receipt.items_on_receipt_FK)
                                    {

                                        if (found_lease_item && found_sale_item) 
                                        {

                                            break;

                                        }
                                        else
                                        {

                                            if (!found_lease_item)
                                            {

                                                if (item.item_id_FK.item_trading_type_id_FK.id == 1) // if item trading type is set to "lease"
                                                {

                                                    found_lease_item = true;

                                                }

                                            }


                                            if (!found_sale_item)
                                            {

                                                if (item.item_id_FK.item_trading_type_id_FK.id == 2 || item.item_id_FK.item_trading_type_id_FK.id == 3) // if item trading type is set to "sale" or "service"
                                                {

                                                    found_sale_item = true;

                                                }

                                            }


                                        }

                                        

                                    }


                                    if (found_lease_item)
                                    {

                                        if (return_list.Where(i => i.month.Equals(receipt.date.Month) && i.year.Equals(receipt.date.Year) && i.sale_lease.Equals(true)).FirstOrDefault() == null)
                                        {

                                            return_list.Add(new Invoice_Available
                                            {
                                                month = receipt.date.Month,
                                                year = receipt.date.Year,
                                                sale_lease = true

                                            });

                                        }

                                    }

                                    if (found_sale_item)
                                    {

                                        if (return_list.Where(i => i.month.Equals(receipt.date.Month) && i.year.Equals(receipt.date.Year) && i.sale_lease.Equals(false)).FirstOrDefault() == null)
                                        {

                                            return_list.Add(new Invoice_Available
                                            {
                                                month = receipt.date.Month,
                                                year = receipt.date.Year,
                                                sale_lease = false

                                            });

                                        }

                                    }


                                    





                                }
                                else
                                {
                                    continue;
                                }



                            }

                        }

                        return return_list;

                    }
                    else
                    {
                        throw new Exception("19");// order not found in DB
                    }

                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }
            }

        }




        /*
         * Calculate_Lease_Value method
         * This method calculates net worth of lease in specified month in specified year.
         * 
         * It accepts Calculate_Lease_Value_Data object as input.
         * Then it performs calculation and returns list which contains objects with net worth, tax pct, tax worth, gross worth (each object has unique tax pct value).
         */
        public async Task<List<Calculate_Lease_Value_Model_List>> Calculate_Lease_Value(Calculate_Lease_Value_Data obj)
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

                    var selected_order = _context.Order.Where(o => o.id.Equals(obj.order_id))
                        .Include(o => o.receipts_FK)
                            .ThenInclude(r => r.items_on_receipt_FK)
                                .ThenInclude(i => i.item_id_FK).ThenInclude(it => it.item_trading_type_id_FK)
                        .FirstOrDefault();

                    if (selected_order != null)
                    {

                        int month_to_calculate = obj.month;
                        int year_to_calculate = obj.year;

                        DateTime first_day_of_month = new DateTime(year_to_calculate, month_to_calculate, 1);


                        List<Receipt> order_receipts = selected_order.receipts_FK;

                        List<Calculate_Lease_Value_Model_List> return_list = new List<Calculate_Lease_Value_Model_List>();

                        List<Calculation_Item_On_Receipt_Model> from_month_begining_model = new List<Calculation_Item_On_Receipt_Model>();

                        List<Item_On_Receipt_Model> item_buffer = new List<Item_On_Receipt_Model>();

                        if (order_receipts.Count > 0)
                        {


                            foreach (Receipt receipt in order_receipts)
                            {

                                DateTime receipt_date = new DateTime(receipt.date.Year, receipt.date.Month, receipt.date.Day);


                                if (receipt_date < first_day_of_month)
                                {


                                    foreach (Item_On_Receipt item_on_receipt in receipt.items_on_receipt_FK)
                                    {


                                        var exist_in_buffer = item_buffer.Where(i => i.item_id.Equals(item_on_receipt.item_id_FK.id)).FirstOrDefault();

                                        if (exist_in_buffer != null)
                                        {

                                            if (receipt.in_out) // release receipt
                                            {

                                                exist_in_buffer.count += item_on_receipt.count;

                                            }
                                            else // return receipt
                                            {

                                                exist_in_buffer.count -= item_on_receipt.count;

                                            }

                                        }
                                        else
                                        {
                                            if (item_on_receipt.item_id_FK.item_trading_type_id_FK.id != 1)
                                            {
                                                continue;
                                            }


                                            Item_On_Receipt_Model model = new Item_On_Receipt_Model
                                            {
                                                id = item_on_receipt.id,
                                                count = 0,
                                                item_id = item_on_receipt.item_id_FK.id,

                                            };

                                            if (receipt.in_out) // release receipt
                                            {

                                                model.count += item_on_receipt.count;

                                            }
                                            else // return receipt
                                            {

                                                model.count -= item_on_receipt.count;

                                            }

                                            item_buffer.Add(model);


                                        }


                                    }


                                }


                            }





                            foreach (Item_On_Receipt_Model temp_model in item_buffer)
                            {

                                from_month_begining_model.Add(new Calculation_Item_On_Receipt_Model
                                {
                                    id = temp_model.id,
                                    count= temp_model.count,
                                    receipt_date = first_day_of_month,
                                    item_id = temp_model.item_id,
                                    overwritten = false,
                                    days_on_construction_site = 0
                                });

                            }



                            foreach (DateTime day in AllDaysInMonth(year_to_calculate, month_to_calculate))
                            {


                                foreach (Receipt receipt in order_receipts)
                                {

                                    DateTime receipt_date = new DateTime(receipt.date.Year, receipt.date.Month, receipt.date.Day);


                                    if (receipt_date.Equals(day))
                                    {


                                        foreach (Item_On_Receipt item_on_receipt in receipt.items_on_receipt_FK)
                                        {

                                            if (item_on_receipt.item_id_FK.item_trading_type_id_FK.id != 1)
                                            {
                                                continue;
                                            }


                                            Calculation_Item_On_Receipt_Model calc_model = new Calculation_Item_On_Receipt_Model
                                            {
                                                id = item_on_receipt.id,
                                                count = 0,
                                                item_id = item_on_receipt.item_id_FK.id,
                                                overwritten = false,
                                                days_on_construction_site = 0,
                                                receipt_date = day

                                            };

                                            var previous_check = from_month_begining_model.Where(f => f.item_id.Equals(calc_model.item_id)).ToList().MaxBy(f => f.receipt_date);


                                            if (receipt.in_out) // release receipt
                                            {

                                                if (previous_check != null)
                                                {

                                                    calc_model.count = previous_check.count + item_on_receipt.count;

                                                    previous_check.overwritten = true;

                                                }
                                                else
                                                {

                                                    calc_model.count = item_on_receipt.count;

                                                }


                                            }
                                            else // return receipt
                                            {

                                                if (previous_check != null)
                                                {

                                                    calc_model.count = previous_check.count - item_on_receipt.count;

                                                    previous_check.overwritten = true;

                                                }
                                                else
                                                {

                                                    throw new Exception("19"); // previous release not found

                                                }

                                            }

                                            from_month_begining_model.Add(calc_model);




                                        }


                                    }


                                }



                                foreach (Calculation_Item_On_Receipt_Model calc_model in from_month_begining_model)
                                {

                                    if (!calc_model.overwritten)
                                    {
                                        calc_model.days_on_construction_site++;
                                    }

                                }


                            }


                            List<Calculation_Item_On_Receipt_Model> empty_construction_site_check = from_month_begining_model.Where(f => f.overwritten.Equals(false) && f.count == 0).ToList();

                            if (empty_construction_site_check.Count > 0)
                            {

                                foreach (Calculation_Item_On_Receipt_Model model in from_month_begining_model)
                                {

                                    if (model.overwritten.Equals(false) && model.count == 0)
                                    {

                                        var previous = from_month_begining_model.Where(f => f.item_id.Equals(model.item_id) && f.overwritten.Equals(true)).ToList().MaxBy(f => f.receipt_date);

                                        if (previous != null)
                                        {
                                            model.count = previous.count;
                                            model.days_on_construction_site = 1;

                                        }
                                        else
                                        {
                                            throw new Exception("19");//previous item from receipt not found
                                        }

                                    }

                                }


                            }

                            List<Encrypted_Object> encrypted_rate = new List<Encrypted_Object>();
                            List<Encrypted_Object> encrypted_price = new List<Encrypted_Object>();
                            
                            List<Decrypted_Object> decrypted_tax_pct = new List<Decrypted_Object>();


                            foreach (Calculation_Item_On_Receipt_Model decrypt_model in from_month_begining_model)
                            {

                                var item_info = _context.Item.Where(i => i.id.Equals(decrypt_model.item_id)).Include(it => it.item_type_id_FK).FirstOrDefault();


                                if (item_info != null)
                                {
                                    var value_exist_1 = encrypted_rate.Where(e => e.id.Equals(decrypt_model.item_id)).FirstOrDefault();

                                    var value_exist_2 = encrypted_price.Where(e => e.id.Equals(decrypt_model.item_id)).FirstOrDefault();

                                    var value_exist_3 = decrypted_tax_pct.Where(d => d.id.Equals(decrypt_model.item_id)).FirstOrDefault();


                                    if (value_exist_1 == null)
                                    {
                                    
                                        encrypted_rate.Add(new Encrypted_Object { id = decrypt_model.item_id, encryptedValue = item_info.item_type_id_FK.rate });

                                    }


                                    if (value_exist_2 == null)
                                    {

                                        encrypted_price.Add(new Encrypted_Object { id = decrypt_model.item_id, encryptedValue = item_info.price });

                                    }


                                    if (value_exist_3 == null)
                                    {

                                        decrypted_tax_pct.Add(new Decrypted_Object { id = decrypt_model.item_id, decryptedValue = item_info.tax_pct.ToString() });

                                    }

                                }
                                else
                                {

                                    throw new Exception("19");

                                }

                            }

                            if(encrypted_rate.Count > 0 && encrypted_price.Count > 0 && decrypted_tax_pct.Count > 0)
                            {

                                List<Decrypted_Object> decrypted_rate = await Crypto.DecryptList(obj.session, encrypted_rate);
                                List<Decrypted_Object> decrypted_price = await Crypto.DecryptList(obj.session, encrypted_price);

                                if(decrypted_rate.Count == encrypted_rate.Count && decrypted_price.Count == encrypted_price.Count)
                                {

                                    foreach (Calculation_Item_On_Receipt_Model final_model in from_month_begining_model)
                                    {
                                        
                                        double rate = Convert.ToDouble(decrypted_rate.Where(d => d.id.Equals(final_model.item_id)).First().decryptedValue);

                                        double net_price = Convert.ToDouble(decrypted_price.Where(d => d.id.Equals(final_model.item_id)).First().decryptedValue);
                                        
                                        double tax_pct = Convert.ToDouble(decrypted_tax_pct.Where(d => d.id.Equals(final_model.item_id)).First().decryptedValue);


                                        double net_worth = final_model.count * net_price * rate * final_model.days_on_construction_site;
                                        double tax_worth = net_worth * (tax_pct/100);

                                        
                                        var exist_on_checkout = return_list.Where(r => r.tax_pct.Equals(tax_pct)).FirstOrDefault();

                                        if (exist_on_checkout != null)
                                        {

                                            exist_on_checkout.net_worth += net_worth;
                                            exist_on_checkout.tax_worth += tax_worth;
                                            exist_on_checkout.gross_worth += net_worth + tax_worth;

                                        }
                                        else
                                        {

                                            return_list.Add(new Calculate_Lease_Value_Model_List
                                            {
                                                tax_pct = tax_pct,
                                                net_worth = net_worth,
                                                tax_worth = tax_worth,
                                                gross_worth = net_worth + tax_worth
                                            });

                                        }


                                    }


                                }


                            }


                        }


                        return return_list;

                    }
                    else
                    {
                        throw new Exception("19");// order not found in DB
                    }

                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }
            }

        }




        /*
         * AllDaysInMonth method
         * This method returns list of all days in month specified in parameter
         */
        public List<DateTime> AllDaysInMonth(int year, int month)
        {

            int days = DateTime.DaysInMonth(year, month);

            List<DateTime> result = new List<DateTime>();

            for (int day = 1; day <= days; day++)
            {
                result.Add(new DateTime(year, month, day));
            }

            return result;

        }





        /*
         * Calculate_Sale_And_Service_Value method
         * This method calculates on-invoice values of sold items and services in specified month in specified year.
         * 
         * It accepts Calculate_Lease_Value_Data object as input.
         * Then it performs calculation and returns list which contains objects with name, count, net worth, tax pct, tax worth, gross worth (each object for each sold item / service).
         */
        public async Task<List<Calculate_Sale_And_Service_Value_Model_List>> Calculate_Sale_And_Service_Value(Calculate_Lease_Value_Data obj)
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

                    var selected_order = _context.Order.Where(o => o.id.Equals(obj.order_id))
                        .Include(o => o.receipts_FK)
                            .ThenInclude(r => r.items_on_receipt_FK)
                                .ThenInclude(i => i.item_id_FK).ThenInclude(it => it.item_trading_type_id_FK)
                        .FirstOrDefault();

                    if (selected_order != null)
                    {

                        int month_to_calculate = obj.month;
                        int year_to_calculate = obj.year;


                        List<Receipt> order_receipts = selected_order.receipts_FK;

                        List<Calculate_Sale_And_Service_Value_Model_List> return_list = new List<Calculate_Sale_And_Service_Value_Model_List>();

                        List<Encrypted_Object> item_names = new List<Encrypted_Object>();
                        List<Encrypted_Object> item_prices = new List<Encrypted_Object>();

                        if (order_receipts.Count > 0)
                        {

                            foreach (DateTime day in AllDaysInMonth(year_to_calculate, month_to_calculate))
                            {


                                foreach (Receipt receipt in order_receipts)
                                {

                                    if (receipt.in_out)
                                    {


                                        DateTime receipt_date = new DateTime(receipt.date.Year, receipt.date.Month, receipt.date.Day);


                                        if (receipt_date.Equals(day))
                                        {


                                            foreach (Item_On_Receipt item_on_receipt in receipt.items_on_receipt_FK)
                                            {

                                                if (item_on_receipt.item_id_FK.item_trading_type_id_FK.id == 1)
                                                {
                                                    continue;
                                                }


                                                var already_on_name_list = item_names.Where(r => r.id.Equals(item_on_receipt.item_id_FK.id)).FirstOrDefault();

                                                var already_on_price_list = item_prices.Where(r => r.id.Equals(item_on_receipt.item_id_FK.id)).FirstOrDefault();


                                                if (already_on_name_list == null && already_on_price_list == null)
                                                {

                                                    item_names.Add(new Encrypted_Object { id = item_on_receipt.item_id_FK.id, encryptedValue = item_on_receipt.item_id_FK.product_name });

                                                    item_prices.Add(new Encrypted_Object { id = item_on_receipt.item_id_FK.id, encryptedValue = item_on_receipt.item_id_FK.price });

                                                }

                                            }

                                        }


                                    }

                                }

                            }






                            if (item_names.Count > 0 && item_prices.Count > 0)
                            {

                                List<Decrypted_Object> decrypted_names = await Crypto.DecryptList(obj.session, item_names);
                                List<Decrypted_Object> decrypted_prices = await Crypto.DecryptList(obj.session, item_prices);


                                if (decrypted_names.Count == item_names.Count && decrypted_prices.Count == item_prices.Count)
                                {


                                    foreach (DateTime day in AllDaysInMonth(year_to_calculate, month_to_calculate))
                                    {


                                        foreach (Receipt receipt in order_receipts)
                                        {

                                            if (receipt.in_out)
                                            {


                                                DateTime receipt_date = new DateTime(receipt.date.Year, receipt.date.Month, receipt.date.Day);


                                                if (receipt_date.Equals(day))
                                                {


                                                    foreach (Item_On_Receipt item_on_receipt in receipt.items_on_receipt_FK)
                                                    {

                                                        if (item_on_receipt.item_id_FK.item_trading_type_id_FK.id == 1)
                                                        {
                                                            continue;
                                                        }


                                                        var already_on_list = return_list.Where(r => r.item_id.Equals(item_on_receipt.item_id_FK.id)).FirstOrDefault();


                                                        if (already_on_list == null)
                                                        {

                                                            var name = decrypted_names.Where(d => d.id.Equals(item_on_receipt.item_id_FK.id)).FirstOrDefault();
                                                            var price = decrypted_prices.Where(d => d.id.Equals(item_on_receipt.item_id_FK.id)).FirstOrDefault();

                                                            if (price != null && name != null)
                                                            {
                                                                double converted_net_worth = Convert.ToDouble(price.decryptedValue) * item_on_receipt.count;
                                                                double tax_pct = item_on_receipt.item_id_FK.tax_pct;
                                                                double tax_worth = converted_net_worth * (tax_pct / 100);


                                                                Calculate_Sale_And_Service_Value_Model_List final_model = new Calculate_Sale_And_Service_Value_Model_List
                                                                {
                                                                    item_id = item_on_receipt.item_id_FK.id,
                                                                    name = name.decryptedValue,
                                                                    count = item_on_receipt.count,
                                                                    net_worth = converted_net_worth,
                                                                    tax_pct = tax_pct,
                                                                    tax_worth = tax_worth,
                                                                    gross_worth = converted_net_worth + tax_worth

                                                                };

                                                                return_list.Add(final_model);

                                                            }
                                                            else
                                                            {
                                                                throw new Exception("3"); //Decryption error
                                                            }


                                                        }
                                                        else
                                                        {


                                                            var name = decrypted_names.Where(d => d.id.Equals(item_on_receipt.item_id_FK.id)).FirstOrDefault();
                                                            var price = decrypted_prices.Where(d => d.id.Equals(item_on_receipt.item_id_FK.id)).FirstOrDefault();

                                                            if (price != null && name != null)
                                                            {
                                                                double converted_net_worth = Convert.ToDouble(price.decryptedValue) * item_on_receipt.count;
                                                                double tax_pct = item_on_receipt.item_id_FK.tax_pct;
                                                                double tax_worth = converted_net_worth * (tax_pct / 100);


                                                                already_on_list.count += item_on_receipt.count;
                                                                already_on_list.net_worth += converted_net_worth;
                                                                already_on_list.tax_worth += tax_worth;
                                                                already_on_list.gross_worth += converted_net_worth + tax_worth;


                                                            }
                                                            else
                                                            {
                                                                throw new Exception("3"); //Decryption error
                                                            }


                                                        }



                                                    }



                                                }


                                            }


                                        }


                                    }


                                }



                            }




                        }


                        return return_list;

                    }
                    else
                    {
                        throw new Exception("19");// order not found in DB
                    }

                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }
            }

        }









        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++\\
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++\\
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++\\
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++\\
        /*
         * Invoice_Issuer
         */
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++\\
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++\\
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++\\
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++\\




        public async Task<string> Invoice_Get_Logo_Tag(Session_Data session_Data, string invoice_full_number, byte[]? logo_bytes, string? file_type)
        {
            string logo_return_string = string.Empty;


            if (logo_bytes != null && file_type != null)
            {                

                string logo_folder_path = System_Path.WRITE_DIRECTORY + "/" + _configuration.GetValue<string>("Database:DB") + session_Data.userId;
                string logo_file_path = logo_folder_path + "/logo_" + invoice_full_number + file_type;

                try
                {
                    if (Directory.Exists(logo_folder_path))
                    {

                        File.WriteAllBytes(logo_file_path, logo_bytes);

                        logo_return_string = "<img src='" + logo_file_path + "'>";
                    }
                    else
                    {
                        Directory.CreateDirectory(logo_file_path);

                        File.WriteAllBytes(logo_file_path, logo_bytes);

                        logo_return_string = "<img src='" + logo_file_path + "'>";
                    }
                }
                catch
                {
                    throw new Exception("27");
                }

                return logo_return_string;
            }
            else
            {
                logo_return_string = "";

                return logo_return_string;
            }
        }


        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++\\
        //Invoice number row
        public long Get_Avaible_Invoice_Number(bool invoice_type, long year, long month)
        {
            long Invoice_Number;

            var LIN = _context.Invoice.Where(i => i.month.Equals(year) && i.year.Equals(month) && i.sale_lease.Equals(invoice_type)).ToList().MaxBy(f => f.number);

            if (LIN == null)
            {
                Invoice_Number = 1;

                return Invoice_Number;
            }
            else
            {
                Invoice_Number = LIN.number + 1;

                return Invoice_Number;
            }
        }


        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++\\
        //Full invoice number
        public string Generate_Full_Invoice_Number(bool invoice_type, long year, long month, long invoice_short_number)
        {
            string Full_Invoice_Number = string.Empty;

            if (invoice_type == true)
            {
                //LEASE

                Full_Invoice_Number += "DW" + "/" + year + "/" + month + "/" + invoice_short_number;
            }
            else
            {
                //SALE/SERVICE

                Full_Invoice_Number += "DS" + "/" + year + "/" + month + "/" + invoice_short_number;
            }

            return Full_Invoice_Number;
        }


        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++\\
        public string Generate_Full_Invoice_Number_For_Files(bool invoice_type, long year, long month, long invoice_short_number)
        {
            string Full_Invoice_Number = string.Empty;

            if (invoice_type == true)
            {
                //LEASE

                Full_Invoice_Number += "DW" + "_" + year + "_" + month + "_" + invoice_short_number;
            }
            else
            {
                //SALE/SERVICE

                Full_Invoice_Number += "DS" + "_" + year + "_" + month + "_" + invoice_short_number;
            }

            return Full_Invoice_Number;
        }        
        
        
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++\\
        public async Task<string> Invoice_Issuer(Invoice_Issuer_Data obj)
        {
            if(obj == null)
            {
                throw new Exception("14");
            }
            else
            {
                if (await Session_Checker.ActiveSession(obj.session))
                {
                    _context = new DB_Context(obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();


                    var invoice_exits = _context.Invoice.Where(i => i.order_id_FK.id.Equals(obj.order_id) && i.sale_lease.Equals(obj.invoice_type) && i.month.Equals(obj.month) && i.year.Equals(obj.year)).FirstOrDefault();

                    if (invoice_exits != null)
                    {
                        Invoice_Model existing_invoice = await Get_Invoice_By_ID(new Get_Invoice_By_ID_Data { session = obj.session, id_to_get = invoice_exits.id });

                        if (existing_invoice == null)
                        {
                            throw new Exception("19");
                        }
                        else
                        {
                            obj.invoice_type = existing_invoice.sale_lease;
                            obj.month = existing_invoice.month;
                            obj.year = existing_invoice.year;
                            obj.issue_date = existing_invoice.issue_date;
                            obj.sale_date = existing_invoice.sale_date;
                            obj.payment_date = existing_invoice.payment_date;
                            obj.payment_method = existing_invoice.payment_method;
                            obj.discount = existing_invoice.discount;
                            obj.comment = existing_invoice.comment;
                        }
                    }

                    var company_Manager = new Company_Manager(_configuration);
                    var sellerInfo = await company_Manager.Get_Company(new Get_Company_Data { session = obj.session });


                    var order = _context.Order.Where(i => i.id.Equals(obj.order_id)).Include(x => x.client_id_FK).FirstOrDefault();
                    
                    if(order == null)
                    {
                        throw new Exception("19");
                    }

                    var client_Manager = new Client_Manager(_configuration);
                    var clientInfo = await client_Manager.Get_Client_by_ID(new Get_Client_By_ID { session = obj.session, id_to_get = order.client_id_FK.id });

                    var humanizer_lang = new CultureInfo(obj.language_tag);

                    Invoice_Language_JSON invoice_language;

                    long invoice_number_row;
                    string invoice_full_number;
                    string invoice_full_number_file_friendly;

                    string invoice_PDF_file;

                    double invoice_gross_value = 0.0;
                    double total_vat = 0.0;
                    double total_net = 0.0;

                    string invoice_gross_value_to_word;

                    string invoice_prefix;

                    string invoice_includings = "";

                    string invoice_language_file;
                    string invoice_css;
                    string invoice_invoiceBody;
                    string invoice_header;
                    string invoice_sellerInfo;
                    string invoice_clientInfo;
                    string invoice_mainTable;
                    string invoice_mainTableHead;
                    string mainTableData;
                    string invoice_mainTableFooter;
                    string invoice_summaryTable;
                    string invoice_footer;

                    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++\\
                    try
                    {
                        invoice_language_file = Invoice_Issuer_Manager.GET_INVOICE_LANGUAGE(obj.language_tag);

                        invoice_css = Invoice_Issuer_Manager.GET_INVOICE_CSS();

                        invoice_invoiceBody = Invoice_Issuer_Manager.GET_INVOICE_INVOICEBODY();

                        invoice_header = Invoice_Issuer_Manager.GET_INVOICE_HEADER();

                        invoice_sellerInfo = Invoice_Issuer_Manager.GET_INVOICE_SELLERINFO();

                        invoice_clientInfo = Invoice_Issuer_Manager.GET_INVOICE_CLIENTINFO();

                        invoice_mainTable = Invoice_Issuer_Manager.GET_INVOICE_MAINTABLE();

                        invoice_mainTableHead = Invoice_Issuer_Manager.GET_INVOICE_MAINTABLEHEAD();

                        invoice_mainTableFooter = Invoice_Issuer_Manager.GET_INVOICE_MAINTABLEFOOTER();

                        invoice_summaryTable = Invoice_Issuer_Manager.GET_INVOICE_SUMMARYTABLE();

                        invoice_footer = Invoice_Issuer_Manager.GET_INVOICE_FOOTER();
                    }
                    catch
                    {
                        throw new Exception("26");
                    }

                    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++\\
                    try
                    {
                        invoice_language = JsonSerializer.Deserialize<Invoice_Language_JSON>(invoice_language_file)!;
                    }
                    catch
                    {
                        throw new Exception("28");
                    }

                    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++\\
                    try
                    {
                        if(invoice_exits == null)
                        {
                            invoice_number_row = Get_Avaible_Invoice_Number(obj.invoice_type, obj.year, obj.month);

                            invoice_full_number = Generate_Full_Invoice_Number(obj.invoice_type, obj.year, obj.month, invoice_number_row);

                            invoice_full_number_file_friendly = Generate_Full_Invoice_Number_For_Files(obj.invoice_type, obj.year, obj.month, invoice_number_row);
                        }
                        else
                        {
                            invoice_number_row = invoice_exits.number;

                            invoice_full_number = invoice_exits.prefix + "/" + invoice_exits.year + "/" + invoice_exits.month + "/" + invoice_number_row;

                            invoice_full_number_file_friendly = invoice_exits.prefix + "_" + invoice_exits.year + "_" + invoice_exits.month + "_" + invoice_number_row;
                        }

                        invoice_PDF_file = System_Path.WRITE_DIRECTORY + "/" + _configuration.GetValue<string>("Database:DB") + obj.session.userId + "/" + invoice_full_number_file_friendly + ".pdf";
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.ToString());
                    }

                    invoice_header = string.Format(invoice_header, new object[]
                    {
                        invoice_language.invoice,
                        invoice_full_number,
                        invoice_language.issue_date,
                        invoice_language.sale_date,
                        invoice_language.payment_date,
                        invoice_language.payment_method,
                        obj.issue_date,
                        obj.sale_date,
                        obj.payment_date,
                        obj.payment_method,
                        await Invoice_Get_Logo_Tag
                        (
                            obj.session,
                            invoice_full_number_file_friendly,
                            sellerInfo.company_logo,
                            sellerInfo.file_type
                        )
                    });

                    invoice_sellerInfo = string.Format(invoice_sellerInfo, new object[]
                    {
                        invoice_language.seller,
                        sellerInfo.name,
                        sellerInfo.surname,
                        sellerInfo.company_name,
                        sellerInfo.NIP,
                    });

                    invoice_clientInfo = string.Format(invoice_clientInfo, new object[]
                    {
                        invoice_language.client,
                        clientInfo.name,
                        clientInfo.surname,
                        clientInfo.company_name,
                        clientInfo.NIP,
                    });

                    invoice_mainTableHead = string.Format(invoice_mainTableHead, new object[]
                    {
                        invoice_language.number,
                        invoice_language.table_head_item,
                        invoice_language.quantity,
                        invoice_language.net_price,
                        invoice_language.net_value,
                        invoice_language.vat_percent,
                        invoice_language.vat_value,
                        invoice_language.gross_value
                    });

                    if (obj.invoice_type == true)
                    {
                        // LEASE
                        invoice_prefix = "DW";

                        var leases_calculated = await Calculate_Lease_Value(new Calculate_Lease_Value_Data { session = obj.session, order_id = obj.order_id, year = obj.year, month = obj.month});

                        invoice_prefix = "DS";

                        mainTableData = "";

                        if (leases_calculated != null)
                        {
                            int count = 1;
                            foreach (var item1 in leases_calculated)
                            {

                                mainTableData +=
                                    "<tr>" +
                                        "<td>" + count + "</td>" +
                                        "<td style=\"text-align: left\">" + invoice_language.lease_for_month + " " + " " + "</td>" +
                                        "<td>" + invoice_language.lease_measurement_unit + "</td>" +
                                        "<td>" + "-" + "</td>" +
                                        "<td>" + Math.Round(item1.net_worth, 2) + "</td>" +
                                        "<td>" + item1.tax_pct + "</td>" +
                                        "<td>" + Math.Round(item1.tax_worth, 2) + "</td>" +
                                        "<td>" + Math.Round(item1.gross_worth, 2) + "</td>" +
                                    "</tr>";

                                invoice_gross_value += Math.Round(item1.gross_worth, 2);
                                total_vat += Math.Round(item1.tax_worth, 2);
                                total_net += Math.Round(item1.net_worth, 2);

                                count++;
                            }

                            foreach (var m in leases_calculated)
                            {
                                invoice_includings +=
                                "<tr>" +
                                    "<td colspan = \"3\" style = \"border-bottom: 2px solid transparent; border-left: 2px solid transparent;\"></td>" +
                                    "<th>" + invoice_language.including + "</th>" +
                                    "<td>" + Math.Round(m.net_worth, 2) + "</td>" +
                                    "<td>" + m.tax_pct + "</td>" +
                                    "<td>" + Math.Round(m.tax_worth, 2) + "</td>" +
                                    "<td>" + Math.Round(m.gross_worth, 2) + "</td>" +
                                "</tr>";
                            }

                            invoice_mainTableFooter = string.Format(invoice_mainTableFooter, new object[]
                            {
                                invoice_includings,

                                invoice_language.total,
                                Math.Round(total_net, 2),
                                "-",
                                Math.Round(total_vat, 2),
                                Math.Round(invoice_gross_value, 2)
                            });
                        }
                        else
                        {
                            throw new Exception("14");
                        }
                    }
                    else
                    {
                        // SALE/SERVICE

                        invoice_prefix = "DS";

                        mainTableData = "";

                        var sales_services_calculated = await Calculate_Sale_And_Service_Value(new Calculate_Lease_Value_Data { session = obj.session, order_id = obj.order_id, year = obj.year, month = obj.month});

                        if(sales_services_calculated != null)
                        {

                            List<Item> items = new List<Item>();

                            foreach(var iiii in sales_services_calculated)
                            {
                                var eeee = _context.Item.Where(i => i.id.Equals(iiii.item_id)).Include(x => x.item_counting_type_id_FK).FirstOrDefault();

                                if(eeee != null)
                                {
                                    items.Add(eeee);
                                }
                                else
                                {
                                    throw new Exception("19");
                                }

                                items.Add(eeee);
                            }

                            List<Encrypted_Object> prices = new List<Encrypted_Object>();

                            foreach (var x in items)
                            {
                                prices.Add(new Encrypted_Object { id = x.id, encryptedValue = x.price });
                            }

                            List<Decrypted_Object> prices_decrypted = await Crypto.DecryptList(obj.session, prices);

                            List<invoice_including_row_object> includings = new List<invoice_including_row_object>();

                            int count = 1;
                            foreach (var item1 in sales_services_calculated)
                            {
                                var item_data = _context.Item.Where(i => i.id.Equals(item1.item_id)).Include(x => x.item_counting_type_id_FK).FirstOrDefault();
                                var item_price = prices_decrypted.Where(a => a.id.Equals(item1.item_id)).FirstOrDefault();

                                if (item_data != null && item_price != null)
                                {
                                    mainTableData +=
                                    "<tr>" +
                                        "<td>" + count + "</td>" +
                                        "<td style=\"text-align: left\">" + item1.name + "</td>" +
                                        "<td>" + item1.count + "</td>" +
                                        "<td>" + item_price.decryptedValue + "</td>" +
                                        "<td>" + Math.Round(item1.net_worth, 2) + "</td>" +
                                        "<td>" + item1.tax_pct + "</td>" +
                                        "<td>" + Math.Round(item1.tax_worth, 2) + "</td>" +
                                        "<td>" + Math.Round(item1.gross_worth, 2) + "</td>" +
                                    "</tr>";

                                    var e = includings.Where(i => i.tax_pct.Equals(item1.tax_pct)).FirstOrDefault();

                                    if(e == null)
                                    {
                                        includings.Add(new invoice_including_row_object { tax_pct = item1.tax_pct, net_total = Math.Round(item1.net_worth, 2), gross_total = Math.Round(item1.gross_worth, 2), tax_total = Math.Round(item1.tax_worth, 2) });
                                    }
                                    else
                                    {
                                        e.tax_total += Math.Round(item1.tax_worth, 2);
                                        e.net_total += Math.Round(item1.net_worth, 2);
                                        e.gross_total += Math.Round(item1.gross_worth, 2);
                                    }

                                    invoice_gross_value += Math.Round(item1.gross_worth, 2);
                                    total_vat += Math.Round(item1.tax_worth, 2);
                                    total_net += Math.Round(item1.net_worth, 2);

                                    count++;
                                }
                                else
                                {
                                    throw new Exception("19");
                                }
                            }

                            foreach(var m in includings)
                            {
                                invoice_includings +=
                                "<tr>" +
                                    "<td colspan = \"3\" style = \"border-bottom: 2px solid transparent; border-left: 2px solid transparent;\"></td>" +
                                    "<th>" + invoice_language.including + "</th>" +
                                    "<td>" + Math.Round(m.net_total, 2) + "</td>" +
                                    "<td>" + m.tax_pct + "</td>" +
                                    "<td>" + Math.Round(m.tax_total, 2) + "</td>" +
                                    "<td>" + Math.Round(m.gross_total, 2) + "</td>" +
                                "</tr>";
                            }

                            invoice_mainTableFooter = string.Format(invoice_mainTableFooter, new object[]
                            {
                                invoice_includings,

                                invoice_language.total,
                                Math.Round(total_net, 2),
                                "-",
                                Math.Round(total_vat, 2),
                                Math.Round(invoice_gross_value, 2)
                            });
                        }
                        else
                        {
                            throw new Exception("14");
                        }
                    }

                    invoice_mainTable = string.Format(invoice_mainTable, invoice_mainTableHead, mainTableData, invoice_mainTableFooter);

                    invoice_summaryTable = string.Format(invoice_summaryTable, new object[]
                    {
                        invoice_language.net_price,
                        invoice_language.vat_value,
                        invoice_language.gross_value,
                        Math.Round(total_net, 2) + " " + sellerInfo.money_sign,
                        Math.Round(total_vat, 2) + " " + sellerInfo.money_sign,
                        Math.Round(invoice_gross_value, 2) + " " + sellerInfo.money_sign
                    });

                    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++\\
                    var gross_price_string = invoice_gross_value.ToString().Split('.');
                    long part_1;
                    long part_2;
                    if (gross_price_string.Length == 2)
                    {
                        part_1 = long.Parse(gross_price_string[0]);
                        part_2 = long.Parse(gross_price_string[1]);

                        invoice_gross_value_to_word = part_1.ToWords(humanizer_lang) + " " + sellerInfo.money_sign + " " + part_2.ToWords(humanizer_lang) + " " + sellerInfo.money_sign_decimal;
                    }
                    else
                    {
                        part_1 = long.Parse(gross_price_string[0]);

                        invoice_gross_value_to_word = part_1.ToWords(humanizer_lang) + sellerInfo.money_sign;
                    }

                    invoice_footer = string.Format(invoice_footer, new object[]
                    {
                            invoice_language.comments,
                            obj.comment,
                            invoice_language.to_pay,
                            invoice_gross_value_to_word,

                            invoice_language.name_and_surname_of_invoice_recipient,
                            invoice_language.name_and_surname_of_invoice_issuer,
                            sellerInfo.name,
                            sellerInfo.surname
                    });

                    invoice_invoiceBody = string.Format(invoice_invoiceBody, new object[]
                    {
                            invoice_css,
                            invoice_header,
                            invoice_sellerInfo,
                            invoice_clientInfo,
                            invoice_mainTable,
                            invoice_summaryTable,
                            invoice_footer
                    });

                    try
                    {
                        if (!Directory.Exists(System_Path.WRITE_DIRECTORY + "/" + _configuration.GetValue<string>("Database:DB") + obj.session.userId + "/"))
                        {
                            Directory.CreateDirectory(System_Path.WRITE_DIRECTORY + "/" + _configuration.GetValue<string>("Database:DB") + obj.session.userId + "/");
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("27");
                    }

                    var options = new HtmlToPdfDocument()
                    {
                        GlobalSettings =
                            {
                                ColorMode = ColorMode.Color,
                                Orientation = Orientation.Portrait,
                                PaperSize = PaperKind.A4,
                                Out = invoice_PDF_file
                            },

                        Objects =
                            {
                                new ObjectSettings
                                {
                                    PagesCount = true,
                                    HtmlContent = invoice_invoiceBody,
                                    WebSettings =
                                    {
                                        DefaultEncoding = "UTF-8"
                                    },
                                }
                            }
                    };

                    _converter.Convert(options);


                    if (invoice_exits == null)
                    {
                        var New_Invoice_To_Add = new Add_Invoice_Data
                        {
                            session = obj.session,
                            prefix = invoice_prefix,
                            sale_lease = obj.invoice_type,
                            year = obj.year,
                            month = obj.month,
                            number = invoice_number_row,
                            order_id_FK = obj.order_id,
                            issue_date = obj.issue_date,
                            sale_date = obj.sale_date,
                            payment_date = obj.payment_date,
                            payment_method = obj.payment_method,
                            discount = obj.discount,
                            net_worth = 0,
                            tax_worth = 0,
                            gross_worth = 0,
                            comment = obj.comment,
                            comment_2 = invoice_language.to_pay + invoice_gross_value_to_word
                        };

                        var result = await Add_Invoice(New_Invoice_To_Add);

                        if (result != null)
                        {
                            return invoice_PDF_file;
                        }
                        else
                        {
                            throw new Exception("4");
                        }
                    }
                    else
                    {
                        return invoice_PDF_file;
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
