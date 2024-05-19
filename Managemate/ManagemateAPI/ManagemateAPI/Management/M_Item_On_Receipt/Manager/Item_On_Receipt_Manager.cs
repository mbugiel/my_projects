using ManagemateAPI.Database.Context;
using ManagemateAPI.Database.Tables;
using ManagemateAPI.Encryption;
using ManagemateAPI.Encryption.Input_Objects;
using ManagemateAPI.Information;
using ManagemateAPI.Management.M_Item_On_Receipt.Input_Objects;
using ManagemateAPI.Management.M_Item_On_Receipt.Table_Model;
using ManagemateAPI.Management.M_Session.Manager;
using Microsoft.EntityFrameworkCore;
using System.IO;

/*
 * This is the Item_On_Receipt_Manager with methods dedicated to the Item_On_Receipt table.
 * 
 * It contains methods to:
 * add records,
 * edit records,
 * delete records,
 * get record by id,
 */
namespace ManagemateAPI.Management.M_Item_On_Receipt.Manager
{
    public class Item_On_Receipt_Manager
    {

        private DB_Context _context;
        private readonly IConfiguration _configuration;


        public Item_On_Receipt_Manager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /* 
         * Add_Item_On_Receipt method
         * This method is used to add new records to the Item_On_Receipt table.
         * 
         * It accepts Add_Item_On_Receipt_Data object as input.
         * It then adds new record with values based on the data given in the input object.
         */     
        public async Task<Item_On_Receipt_ID_Model> Add_Item_On_Receipt(Add_Item_On_Receipt_Data input_obj)
        {

            if (input_obj == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (await Session_Checker.ActiveSession(input_obj.session))
                {


                    _context = new DB_Context(input_obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    var receipt = _context.Receipt.Where(r => r.id.Equals(input_obj.receipt_id_FK)).Include(r => r.order_id_FK).FirstOrDefault();
                    var item = _context.Item.Where(i => i.id.Equals(input_obj.item_id_FK)).Include(i => i.item_trading_type_id_FK).FirstOrDefault();

                    if (receipt != null && item != null)
                    {


                        if (receipt.in_out)
                        {
                            if(item.item_trading_type_id_FK.id != 3)
                            {

                                if (receipt.reservation)
                                {

                                    if (input_obj.count <= item.count - item.blocked_count)
                                    {
                                        item.blocked_count += input_obj.count;
                                    }
                                    else
                                    {
                                        throw new Exception("19"); //Cannot block more items than amount that is in stock
                                    }


                                }
                                else
                                {

                                    if (input_obj.count <= item.count - item.blocked_count)
                                    {

                                        item.count -= input_obj.count;

                                    }
                                    else
                                    {
                                        throw new Exception("19"); //Cannot add more items than amount that is in stock
                                    }


                                }

                            }
                            

                        }
                        else
                        {


                            if(item.item_trading_type_id_FK.id != 1) // if item isn't for lease
                            {
                                throw new Exception("25");
                            }


                            Order order = receipt.order_id_FK;

                            var receipts_to_check = _context.Receipt.Where(r => r.order_id_FK.Equals(order)).Include(r => r.items_on_receipt_FK).ThenInclude(i => i.item_id_FK).ToList();

                            double possible_return_count = 0;


                            foreach(var record in receipts_to_check)
                            {

                                foreach(var receipt_item in record.items_on_receipt_FK)
                                {
                                    if (receipt_item.item_id_FK.Equals(item))
                                    {
                                        if (record.in_out)
                                        {
                                            possible_return_count += receipt_item.count;
                                        }
                                        else
                                        {
                                            possible_return_count -= receipt_item.count;
                                        }


                                    }

                                }

                            }


                            if (input_obj.count <= possible_return_count)
                            {

                                item.count += input_obj.count;

                            }
                            else
                            {

                                throw new Exception("24" + possible_return_count); // too many items wanted to be returned

                            }




                        }



                        var existing_item_on_receipt = _context.Item_On_Receipt.Where(i => i.receipt_id_FK.Equals(receipt) && i.item_id_FK.Equals(item)).FirstOrDefault();
                        Item_On_Receipt new_item_on_receipt = new Item_On_Receipt();


                        if (existing_item_on_receipt != null)
                        {

                            existing_item_on_receipt.count += input_obj.count;
                            existing_item_on_receipt.summary_weight += input_obj.count * item.weight_kg;

                        }
                        else
                        {

                            new_item_on_receipt.receipt_id_FK = receipt;
                            new_item_on_receipt.item_id_FK = item;
                            new_item_on_receipt.count = input_obj.count;
                            new_item_on_receipt.summary_weight = input_obj.count * item.weight_kg;
                            new_item_on_receipt.annotation = await Crypto.Encrypt(input_obj.session, input_obj.annotation);

                        }




                        if (existing_item_on_receipt == null)
                        {

                            receipt.summary_weight += new_item_on_receipt.summary_weight;                            
                            _context.Item_On_Receipt.Add(new_item_on_receipt);

                            _context.SaveChanges();

                            return new Item_On_Receipt_ID_Model { id = new_item_on_receipt.id };

                        }
                        else
                        {
                            receipt.summary_weight += item.weight_kg * input_obj.count;

                            _context.SaveChanges();

                            return new Item_On_Receipt_ID_Model { id = existing_item_on_receipt.id };
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
         * Edit_Item_On_Receipt method
         * This method is used to edit a record in the Item_On_Receipt table.
         * 
         * It accepts Edit_Item_On_Receipt_Data object as input.
         * It then changes values of a record with those given in the input object only if its ID matches the one in the input object.
         */
        public async Task<string> Edit_Item_On_Receipt(Edit_Item_On_Receipt_Data input_obj)
        {

            if (input_obj == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (await Session_Checker.ActiveSession(input_obj.session))
                {
                    _context = new DB_Context(input_obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    var editing_item_on_receipt = _context.Item_On_Receipt.Where(i => i.id.Equals(input_obj.id)).Include(i => i.item_id_FK).Include(i => i.receipt_id_FK).ThenInclude(r => r.order_id_FK).FirstOrDefault();

                    if (editing_item_on_receipt == null)
                    {
                        throw new Exception("19");//item on receipt not found
                    }
                    else
                    {



                        if (editing_item_on_receipt.receipt_id_FK.in_out)
                        {

                            if (editing_item_on_receipt.receipt_id_FK.reservation)
                            {

                                if (input_obj.count <= editing_item_on_receipt.item_id_FK.count - editing_item_on_receipt.item_id_FK.blocked_count + editing_item_on_receipt.count)
                                {
                                    editing_item_on_receipt.item_id_FK.blocked_count -= editing_item_on_receipt.count;

                                    editing_item_on_receipt.item_id_FK.blocked_count += input_obj.count;
                                }
                                else
                                {
                                    throw new Exception("19"); //Cannot block more items than amount that is in stock
                                }


                            }
                            else
                            {

                                if (input_obj.count <= editing_item_on_receipt.item_id_FK.count - editing_item_on_receipt.item_id_FK.blocked_count + editing_item_on_receipt.count)
                                {

                                    editing_item_on_receipt.item_id_FK.count += editing_item_on_receipt.count;

                                    editing_item_on_receipt.item_id_FK.count -= input_obj.count;

                                }
                                else
                                {
                                    throw new Exception("19"); //Cannot add more items than amount that is in stock
                                }


                            }

                        }
                        else
                        {

                            //work in progress

                            Order order = editing_item_on_receipt.receipt_id_FK.order_id_FK;

                            var receipts_to_check = _context.Receipt.Where(r => r.order_id_FK.Equals(order)).Include(r => r.items_on_receipt_FK).ThenInclude(i => i.item_id_FK).ToList();

                            double possible_return_count = editing_item_on_receipt.count;


                            foreach (var record in receipts_to_check)
                            {

                                foreach (var receipt_item in record.items_on_receipt_FK)
                                {
                                    if (receipt_item.item_id_FK.Equals(editing_item_on_receipt.item_id_FK))
                                    {
                                        if (record.in_out)
                                        {
                                            possible_return_count += receipt_item.count;
                                        }
                                        else
                                        {
                                            possible_return_count -= receipt_item.count;
                                        }
                                        
                                    }

                                }

                            }


                            if (input_obj.count <= possible_return_count)
                            {

                                editing_item_on_receipt.item_id_FK.count -= editing_item_on_receipt.count;

                                editing_item_on_receipt.item_id_FK.count += input_obj.count;

                            }
                            else
                            {

                                throw new Exception("24"); // too many items wanted to be returned

                            }




                        }


                        editing_item_on_receipt.receipt_id_FK.summary_weight -= editing_item_on_receipt.summary_weight;

                        editing_item_on_receipt.count = input_obj.count;
                        editing_item_on_receipt.summary_weight = input_obj.count * editing_item_on_receipt.item_id_FK.weight_kg;
                        editing_item_on_receipt.annotation = await Crypto.Encrypt(input_obj.session, input_obj.annotation);

                        editing_item_on_receipt.receipt_id_FK.summary_weight += editing_item_on_receipt.summary_weight;



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
         * Delete_Item_On_Receipt method
         * This method is used to a record from the Item_On_Receipt table.
         *  
         * It accepts Delete_Item_On_Receipt_Data object as input.
         * Then it deletes a record if its ID matches the one given in the input object.
         */
        public async Task<string> Delete_Item_On_Receipt(Delete_Item_On_Receipt_Data input_obj)
        {

            if (input_obj == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (await Session_Checker.ActiveSession(input_obj.session))
                {
                    _context = new DB_Context(input_obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    var existing_item_on_receipt = _context.Item_On_Receipt.Where(i => i.id.Equals(input_obj.item_on_receipt_id)).Include(i => i.item_id_FK).Include(i => i.receipt_id_FK).FirstOrDefault();

                    if (existing_item_on_receipt == null)
                    {
                        throw new Exception("19");//item not found
                    }
                    else
                    {


                        if (existing_item_on_receipt.receipt_id_FK.in_out)
                        {
                            
                            if (existing_item_on_receipt.receipt_id_FK.reservation)
                            {

                                existing_item_on_receipt.item_id_FK.blocked_count -= existing_item_on_receipt.count;

                            }
                            else
                            {

                                existing_item_on_receipt.item_id_FK.count += existing_item_on_receipt.count;

                            }

                        }
                        else
                        {

                            existing_item_on_receipt.item_id_FK.count -= existing_item_on_receipt.count;

                        }


                        existing_item_on_receipt.receipt_id_FK.summary_weight -= existing_item_on_receipt.summary_weight;

                        _context.Item_On_Receipt.Remove(existing_item_on_receipt);
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
         * Get_Available_Items_To_Return method
         * This method produces list of available items to be returned to the storage and returns it.
         * 
         * It accepts Get_Available_Items_To_Return_Data object as input.
         * Then it collects information about previousely released and returned items and returns available items in the list.
         */
        public async Task<List<Item_On_Receipt_Return_Available_Model>> Get_Available_Items_To_Return(Get_Available_Items_To_Return_Data input_obj)
        {

            if (input_obj == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (await Session_Checker.ActiveSession(input_obj.session))
                {
                    _context = new DB_Context(input_obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    var order = _context.Order.Where(o => o.id.Equals(input_obj.order_id))
                        .Include(o => o.receipts_FK).ThenInclude(r => r.items_on_receipt_FK).ThenInclude(i => i.item_id_FK).ThenInclude(it => it.item_counting_type_id_FK)
                        .Include(o => o.receipts_FK).ThenInclude(r => r.items_on_receipt_FK).ThenInclude(i => i.item_id_FK).ThenInclude(it => it.item_trading_type_id_FK)
                        .FirstOrDefault();

                    if (order == null)
                    {
                        throw new Exception("19");// receipt not found
                    }
                    else
                    {


                        List<Item_On_Receipt_Return_Available_Model> items_on_receipt_model = new List<Item_On_Receipt_Return_Available_Model>();

                        List<Encrypted_Object> items_on_receipt_annotations = new List<Encrypted_Object>();
                        List<Encrypted_Object> items_on_receipt_names = new List<Encrypted_Object>();


                        foreach(Receipt receipt in order.receipts_FK)
                        {
                            foreach (Item_On_Receipt item_on_receipt in receipt.items_on_receipt_FK)
                            {

                                if(item_on_receipt.item_id_FK.item_trading_type_id_FK.id != 1)
                                {
                                    continue;
                                }

                                Encrypted_Object data = new Encrypted_Object { id = item_on_receipt.item_id_FK.id, encryptedValue = item_on_receipt.item_id_FK.product_name };

                                if (items_on_receipt_names.Contains(data))
                                {
                                    continue;
                                }
                                else
                                {
                                    items_on_receipt_names.Add(data);
                                }


                            }
                        }

                        List<Decrypted_Object> decrypted_items_on_receipt_names = await Crypto.DecryptList(input_obj.session, items_on_receipt_names);


                        foreach (Receipt receipt in order.receipts_FK)
                        {

                            foreach(Item_On_Receipt item_on_receipt in receipt.items_on_receipt_FK)
                            {

                                if (item_on_receipt.item_id_FK.item_trading_type_id_FK.id != 1)
                                {
                                    continue;
                                }

                                var item_check = items_on_receipt_model.Where(i => i.item_id.Equals(item_on_receipt.item_id_FK.id)).FirstOrDefault();

                                if (item_check == null)
                                {

                                    Item_On_Receipt_Return_Available_Model new_item = new Item_On_Receipt_Return_Available_Model
                                    {
                                        available_count = 0,
                                        catalog_number = item_on_receipt.item_id_FK.catalog_number,
                                        counting_type = item_on_receipt.item_id_FK.item_counting_type_id_FK.counting_type,
                                        item_id = item_on_receipt.item_id_FK.id,
                                        weight = item_on_receipt.item_id_FK.weight_kg

                                    };

                                    if (receipt.in_out)
                                    {
                                        new_item.available_count += item_on_receipt.count;
                                    }
                                    else
                                    {
                                        new_item.available_count -= item_on_receipt.count;
                                    }

                                    items_on_receipt_model.Add(new_item);

                                }
                                else
                                {

                                    if (receipt.in_out)
                                    {
                                        item_check.available_count += item_on_receipt.count;
                                    }
                                    else
                                    {
                                        item_check.available_count -= item_on_receipt.count;
                                    }


                                }


                            }

                        }




                        foreach (Item_On_Receipt_Return_Available_Model item_decrypted in items_on_receipt_model)
                        {

                            var product_name = decrypted_items_on_receipt_names.Where(d => d.id.Equals(item_decrypted.item_id)).FirstOrDefault();

                            if (product_name != null)
                            {
                                item_decrypted.product_name = product_name.decryptedValue;
                            }
                            else
                            {
                                throw new Exception("3");//error while decrypting data 
                            }

                        }

                        return items_on_receipt_model;


                    }



                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }

            }

        }






        /*
         * Get_Items_From_Receipt method
         * This method gets all of records from the Item_On_Receipt table and returns list of theirs models.
         * 
         * It accepts Get_Items_From_Receipt_Data object as input.
         * Then it gets records from DB and makes models of them, after that it returns list of that models.
         */
        public async Task<List<Item_On_Receipt_Model>> Get_Items_From_Receipt(Get_Items_From_Receipt_Data input_obj)
        {

            if (input_obj == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (await Session_Checker.ActiveSession(input_obj.session))
                {
                    _context = new DB_Context(input_obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    var current_receipt = _context.Receipt.Where(r => r.id.Equals(input_obj.receipt_id)).FirstOrDefault();

                    if (current_receipt == null)
                    {
                        throw new Exception("19");// receipt not found
                    }
                    else
                    {

                        List<Item_On_Receipt> items_on_receipt = _context.Item_On_Receipt.Where(i => i.receipt_id_FK.Equals(current_receipt)).Include(i => i.item_id_FK).ThenInclude(it => it.item_counting_type_id_FK).ToList();

                        if (items_on_receipt == null)
                        {
                            throw new Exception("13");// no items on receipt found
                        }
                        else
                        {
                            List<Item_On_Receipt_Model> items_on_receipt_model = new List<Item_On_Receipt_Model>();

                            List<Encrypted_Object> items_on_receipt_annotations = new List<Encrypted_Object>();
                            List<Encrypted_Object> items_on_receipt_names = new List<Encrypted_Object>();

                            foreach (var item in items_on_receipt)
                            {
                                items_on_receipt_model.Add(new Item_On_Receipt_Model
                                {
                                    id = item.id,
                                    catalog_number = item.item_id_FK.catalog_number,
                                    counting_type = item.item_id_FK.item_counting_type_id_FK.counting_type,
                                    item_id = item.item_id_FK.id,
                                    count = item.count,
                                    weight = item.item_id_FK.weight_kg,
                                    summary_weight = item.summary_weight

                                });

                                items_on_receipt_annotations.Add(new Encrypted_Object { id = item.id, encryptedValue = item.annotation });
                                items_on_receipt_names.Add(new Encrypted_Object { id = item.id, encryptedValue = item.item_id_FK.product_name });
                            }


                            List<Decrypted_Object> decrypted_items_on_receipt_annotations = await Crypto.DecryptList(input_obj.session, items_on_receipt_annotations);
                            List<Decrypted_Object> decrypted_items_on_receipt_names = await Crypto.DecryptList(input_obj.session, items_on_receipt_names);

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

                            return items_on_receipt_model;


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
