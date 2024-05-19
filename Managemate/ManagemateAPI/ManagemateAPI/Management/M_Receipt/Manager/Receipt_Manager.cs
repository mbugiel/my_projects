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
using Microsoft.EntityFrameworkCore;


/*
 * This is the Receipt_Manager with methods dedicated to the Receipt table.
 * 
 * It contains methods to:
 * add
 * edit
 * delete
 * get by id
 * 
 */
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

        /* 
         * Add_Receipt method
         * This method is used to add new records to the Receipt table.
         * 
         * It accepts Add_Receipt_Data object as input.
         * It then adds new record with values based on the data given in the input object.
         */
        public async Task<Receipt_Model_Id> Add_Receipt(Add_Receipt_Data obj)
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

                        Receipt newReceipt = new Receipt
                        {
                            in_out = obj.in_out,
                            reservation = obj.reservation,
                            date = obj.creation_date,
                            order_id_FK = order,
                            element = await Crypto.Encrypt(obj.session, obj.element),
                            transport = await Crypto.Encrypt(obj.session, obj.transport),
                            summary_weight = 0,
                            comment = null,

                        };


                        var receipts_check = _context.Receipt.Where(r => r.items_on_receipt_FK.Count.Equals(0)).ToList();

                        if(receipts_check != null)
                        {

                            if(receipts_check.Count > 0)
                            {

                                foreach(var receipt in receipts_check)
                                {

                                    _context.Receipt.Remove(receipt);

                                }

                            }

                        }


                        _context.Receipt.Add(newReceipt);
                        _context.SaveChanges();

                        if(newReceipt.id > 0)
                        {
                            return new Receipt_Model_Id { receipt_id = newReceipt.id };
                        }
                        else
                        {
                            throw new Exception("22");//_22_ADD_RECEIPT_ERROR
                        }
                        

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

        /* 
         * Edit_Receipt method
         * This method is used to edit a record in the Receipt table.
         * 
         * It accepts Edit_Receipt_Data object as input.
         * It then changes values of a record with those given in the input object only if its ID matches the one in the input object.
         */
        public async Task<string> Edit_Receipt(Edit_Receipt_Data obj)
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

                    var editing_receipt = _context.Receipt.Where(c => c.id.Equals(obj.receipt_id)).Include(c => c.items_on_receipt_FK).FirstOrDefault();

                    if (editing_receipt == null)
                    {
                        throw new Exception("19");//Receipt not found
                    }
                    else
                    {
                        List<Item_On_Receipt> items_on_editing_receipt = editing_receipt.items_on_receipt_FK;

                        if (items_on_editing_receipt != null && items_on_editing_receipt.Count > 0)
                        {

                            //work in progress:
                            //changing data in all of items when receipt.data changes

                        }
                        else
                        {
                            throw new Exception("19");//No items on the receipt found
                        }

                        editing_receipt.date = obj.date;
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


        /* 
         * Change_Reservation_State method
         * This method is used to switch between two states of reservation field in Receipt table.
         * 
         * It accepts Change_Reservation_State_Data object as input.
         * It then changes value of a reservation field in the record with those given in the input object, only if its ID matches the one in the input object.
         */
        public async Task<string> Change_Reservation_State(Change_Reservation_State_Data obj)
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

                    var editing_receipt = _context.Receipt.Where(c => c.id.Equals(obj.receipt_id)).Include(c => c.items_on_receipt_FK).FirstOrDefault();

                    if (editing_receipt == null)
                    {
                        throw new Exception("19");//Receipt not found
                    }
                    else
                    {
                        var items_on_editing_receipt = editing_receipt.items_on_receipt_FK;

                        if (items_on_editing_receipt != null && items_on_editing_receipt.Count > 0)
                        {

                            if (editing_receipt.in_out) // Reservation can only be changed on release receipt
                            {


                                foreach (var item in items_on_editing_receipt)
                                {

                                    if (editing_receipt.reservation)
                                    {

                                        if (!obj.reservation)
                                        {

                                            item.item_id_FK.blocked_count -= item.count;

                                            item.item_id_FK.count -= item.count;

                                        }

                                    }
                                    else
                                    {

                                        if (obj.reservation)
                                        {

                                            item.item_id_FK.count += item.count;
                                            item.item_id_FK.blocked_count += item.count;

                                        }

                                    }

                                }

                                editing_receipt.reservation = obj.reservation;


                            }

                        }
                        else
                        {
                            throw new Exception("19");//No items on the receipt found
                        }


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



        /*
         * Delete_Receipt method
         * This method is used to delete a record from the Receipt table.
         *  
         * It accepts Delete_Receipt_Data object as input.
         * Then it deletes a record if its ID matches the one given in the input object.
         */
        public async Task<string> Delete_Receipt(Delete_Receipt_Data obj)
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

        /*
         * Get_Receipt_By_ID method
         * This method gets a record from the Receipt table by its ID and returns it.
         * 
         * It accepts Get_Receipt_By_ID_Data object as input.
         * Then it gets a records that has the same ID as the ID given in the input object
         */
        public async Task<Receipt_Model> Get_Receipt_By_ID(Get_Receipt_By_ID_Data obj)
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

                    var selected_receipt = _context.Receipt.Where(r => r.id.Equals(obj.receipt_id)).Include(r => r.order_id_FK).Include(r => r.items_on_receipt_FK).ThenInclude(i => i.item_id_FK).ThenInclude(it => it.item_counting_type_id_FK).FirstOrDefault();

                    if (selected_receipt == null)
                    {
                        throw new Exception("19");//receipt not found
                    }
                    else
                    {
                        Receipt_Model receipt_model = new Receipt_Model
                        {
                            id = selected_receipt.id,
                            in_out = selected_receipt.in_out,
                            reservation = selected_receipt.reservation,
                            order_id = selected_receipt.order_id_FK.id,
                            date = selected_receipt.date,
                            summary_weight = selected_receipt.summary_weight
                        };



                        List<Encrypted_Object> encrypted_fields =
                        [
                            //encrypted fields in Receipt table
                            new Encrypted_Object { id = 0, encryptedValue = selected_receipt.element },
                            new Encrypted_Object { id = 1, encryptedValue = selected_receipt.transport }
                        ];

                        if(selected_receipt.comment != null)
                        {

                            encrypted_fields.Add(new Encrypted_Object { id = 2, encryptedValue = selected_receipt.comment });

                        }
                        else
                        {

                            receipt_model.comment = "";

                        }

                        List<Decrypted_Object> decrypted_fields = await Crypto.DecryptList(obj.session, encrypted_fields);


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


                        List<Item_On_Receipt> items_on_selected_receipt = selected_receipt.items_on_receipt_FK;

                        if (items_on_selected_receipt == null)
                        {
                            throw new Exception("13");// no items on receipt found
                        }
                        else
                        {
                            List<Item_On_Receipt_Model> items_on_receipt_model = new List<Item_On_Receipt_Model>();

                            List<Encrypted_Object> items_on_receipt_annotations = new List<Encrypted_Object>();
                            List<Encrypted_Object> items_on_receipt_names = new List<Encrypted_Object>();

                            foreach (var item in items_on_selected_receipt)
                            {
                                items_on_receipt_model.Add(new Item_On_Receipt_Model
                                {
                                    id = item.id,
                                    catalog_number = item.item_id_FK.catalog_number,
                                    counting_type = item.item_id_FK.item_counting_type_id_FK.counting_type,
                                    count = item.count,
                                    item_id = item.item_id_FK.id,
                                    weight = item.item_id_FK.weight_kg,
                                    summary_weight = item.summary_weight

                                });

                                items_on_receipt_annotations.Add(new Encrypted_Object { id = item.id, encryptedValue = item.annotation });
                                items_on_receipt_names.Add(new Encrypted_Object { id = item.id, encryptedValue = item.item_id_FK.product_name });
                            }


                            List<Decrypted_Object> decrypted_items_on_receipt_annotations = await Crypto.DecryptList(obj.session, items_on_receipt_annotations);
                            List<Decrypted_Object> decrypted_items_on_receipt_names = await Crypto.DecryptList(obj.session, items_on_receipt_names);

                            foreach (var item_decrypted in items_on_receipt_model)
                            {

                                var annotation = decrypted_items_on_receipt_annotations.Where(d => d.id.Equals(item_decrypted.id)).FirstOrDefault();

                                if (annotation != null)
                                {
                                    item_decrypted.annotation = annotation.decryptedValue;
                                }
                                else
                                {
                                    throw new Exception("3");//error while decrypting data 
                                }

                                var product_name = decrypted_items_on_receipt_names.Where(d => d.id.Equals(item_decrypted.id)).FirstOrDefault();

                                if (product_name != null)
                                {
                                    item_decrypted.product_name = product_name.decryptedValue;
                                }
                                else
                                {
                                    throw new Exception("3");//error while decrypting data 
                                }

                            }

                            receipt_model.items_on_receipt = items_on_receipt_model;


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


        public async Task<List<Receipt_Model_List>> Get_In_Out_Receipt(Get_In_Out_Receipt_Data obj)
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
                        List<Receipt> receipts = _context.Receipt.Where(r => r.order_id_FK.Equals(order) && r.in_out.Equals(obj.in_out) && r.items_on_receipt_FK.Count > 0).ToList();

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
                                var receipt_decrypted = new Receipt_Model_List
                                {
                                    id = item.id,
                                    in_out = item.in_out,
                                    date = item.date,
                                    summary_weight = item.summary_weight

                                };

                                receipt_elements.Add(new Encrypted_Object { id = item.id, encryptedValue = item.element });
                                receipt_transports.Add(new Encrypted_Object { id = item.id, encryptedValue = item.transport });

                                if(item.comment != null)
                                {
                                    receipt_comments.Add(new Encrypted_Object { id = item.id, encryptedValue = item.comment });
                                }
                                else
                                {
                                    receipt_decrypted.comment = "";
                                }

                                receipts_decrypted.Add(receipt_decrypted);


                            }

                            List<Decrypted_Object> receipt_elements_decrypted = await Crypto.DecryptList(obj.session, receipt_elements);
                            List<Decrypted_Object> receipt_transports_decrypted = await Crypto.DecryptList(obj.session, receipt_transports);

                            List<Decrypted_Object> receipt_comments_decrypted = new List<Decrypted_Object>();

                            bool no_comments = true;

                            if (receipt_comments.Count > 0)
                            {
                                no_comments = false;

                                receipt_comments_decrypted = await Crypto.DecryptList(obj.session, receipt_comments);
                            }



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

                                if (!no_comments)
                                {
                                    if(item.comment == null)
                                    {

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

        /*
         * złom
         * złom
         * złom
         * złom
         * złom
         * złom
         * złom
         * złom
         * złom
         * złom
         * złom
         * złom
         * złom
         * złom
         * złom
         * złom
         * złom
         * złom
         * złom
         * złom
         * złom
         * złom
         */
        public async Task<List<Receipt_Model_List>> Get_all_by_page_Receipts(Get_Receipts_Data obj)
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
    }
}
