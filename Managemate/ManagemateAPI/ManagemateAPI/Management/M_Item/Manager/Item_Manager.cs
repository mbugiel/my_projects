using ManagemateAPI.Database.Context;
using ManagemateAPI.Encryption;
using ManagemateAPI.Database.Tables;
using Microsoft.EntityFrameworkCore;
using ManagemateAPI.Encryption.Input_Objects;
using ManagemateAPI.Management.M_Item.Input_Objects;
using ManagemateAPI.Management.M_Item.Table_Model;
using ManagemateAPI.Management.M_Session.Manager;
using ManagemateAPI.Information;
using ManagemateAPI.Management.M_Item_Trading_Type.Table_Model;
using ManagemateAPI.Management.M_Item_Counting_Type.Table_Model;

/*
 * This is the Item_Manager with methods dedicated to the Item table.
 * 
 * It contains methods to:
 * add records,
 * edit records,
 * delete records,
 * get record by id,
 * get all the records.
 */
namespace ManagemateAPI.Management.M_Item.Manager
{
    public class Item_Manager
    {

        private DB_Context _context;
        private readonly IConfiguration _configuration;


        public Item_Manager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /* 
         * Add_Item method
         * This method is used to add new records to the Item table.
         * 
         * It accepts Add_Item_Data object as input.
         * It then adds new record with values based on the data given in the input object.
         */
        public async Task<string> Add_Item(Add_Item_Data item)
        {

            if (item == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (await Session_Checker.ActiveSession(item.session))
                {
                    _context = new DB_Context(item.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    var cat_num_exits = _context.Item.Where(i => i.catalog_number.Equals(item.catalog_number)).FirstOrDefault();

                    if (cat_num_exits != null)
                    {
                        throw new Exception("18");//Catolog number already in use
                    }
                    else
                    {

                        var item_type = _context.Item_Type.Where(i => i.id.Equals(item.item_type_id_FK)).FirstOrDefault();
                        var item_trading_type = _context.Item_Trading_Type.Where(i => i.id.Equals(item.item_trading_type_id_FK)).FirstOrDefault();
                        var item_counting_type = _context.Item_Counting_Type.Where(i => i.id.Equals(item.item_counting_type_id_FK)).FirstOrDefault();

                        if (item_type == null || item_trading_type == null || item_counting_type == null)
                        {
                            throw new Exception("19");// object not found
                        }
                        else
                        {

                            Item newItem = new Item
                            {
                                catalog_number = item.catalog_number,
                                product_name = await Crypto.Encrypt(item.session, item.product_name),
                                item_type_id_FK = item_type,
                                weight_kg = item.weight_kg,
                                count = item.count,
                                blocked_count = item.blocked_count,
                                price = await Crypto.Encrypt(item.session, item.price.ToString()),
                                tax_pct = item.tax_pct,
                                item_trading_type_id_FK = item_trading_type,
                                item_counting_type_id_FK = item_counting_type,
                                comment = await Crypto.Encrypt(item.session, item.comment)
                            };

                            _context.Item.Add(newItem);
                            _context.SaveChanges();

                            return Info.SUCCESSFULLY_ADDED;

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
         * Edit_Item method
         * This method is used to edit a record in the Item table.
         * 
         * It accepts Edit_Item_Data object as input.
         * It then changes values of a record with those given in the input object only if its ID matches the one in the input object.
         */
        public async Task<string> Edit_Item(Edit_Item_Data item)
        {
            if (item == null)
            {
                throw new Exception("14");
            }
            else
            {
                if (await Session_Checker.ActiveSession(item.session))
                {
                    _context = new DB_Context(item.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    var edited_record = _context.Item.Where(o => o.id == item.id).FirstOrDefault();

                    if (edited_record != null)
                    {
                        var cat_num_exists = _context.Item.Where(i => i.catalog_number.Equals(item.catalog_number) && i.id != item.id).FirstOrDefault();

                        if (cat_num_exists != null)
                        {
                            throw new Exception("18");
                        }
                        else
                        {

                            var item_type = _context.Item_Type.Where(i => i.id.Equals(item.item_type_id_FK)).FirstOrDefault();
                            var item_counting_type = _context.Item_Counting_Type.Where(i => i.id.Equals(item.item_counting_type_id_FK)).FirstOrDefault();

                            if (item_type == null || item_counting_type == null)
                            {
                                throw new Exception("19");// object not found
                            }
                            else
                            {

                                edited_record.id = item.id;
                                edited_record.catalog_number = item.catalog_number;
                                edited_record.product_name = await Crypto.Encrypt(item.session, item.product_name);
                                edited_record.item_type_id_FK = item_type;
                                edited_record.weight_kg = item.weight_kg;
                                edited_record.count = item.count;
                                edited_record.blocked_count = item.blocked_count;
                                edited_record.price = await Crypto.Encrypt(item.session, item.price.ToString());
                                edited_record.item_counting_type_id_FK = item_counting_type;
                                edited_record.comment = await Crypto.Encrypt(item.session, item.comment);

                                _context.SaveChanges();

                                return Info.SUCCESSFULLY_CHANGED;

                            }


                        }
                    }
                    else
                    {
                        throw new Exception("19");
                    }
                }
                else
                {
                    throw new Exception("1");
                }
            }
        }

        /*
         * Delete_Item method
         * This method is used to a record from the Item table.
         *  
         * It accepts Delete_Item_Data object as input.
         * Then it deletes a record if its ID matches the one given in the input object.
         */
        public async Task<string> Delete_Item(Delete_Item_Data item)
        {
            if (item == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                if (await Session_Checker.ActiveSession(item.session))
                {
                    _context = new DB_Context(item.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    var id_exits = _context.Item.Where(i => i.id.Equals(item.id)).FirstOrDefault();

                    if (id_exits == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }
                    else
                    {

                        _context.Item.Remove(id_exits);
                        _context.SaveChanges();

                        return Info.SUCCESSFULLY_DELETED; //To do: Zmienić tą wiadomość na deleted jak zostanie zaaktualizowana lista odpowiedzi
                    }
                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }
            }
        }

        /*
         * Get_Item_By_ID method
         * This method gets a record from the Item table by its ID and returns it.
         * 
         * It accepts Get_Item_By_ID_Data object as input.
         * Then it gets a records that has the same ID as the ID given in the input object
         */
        public async Task<Item_Model> Get_Item_by_ID(Get_Item_By_ID_Data obj)
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

                    var selected_record = _context.Item.Include(o => o.item_type_id_FK).Include(c => c.item_trading_type_id_FK).Include(d => d.item_counting_type_id_FK).Where(o => o.id.Equals(obj.id_to_get)).FirstOrDefault();

                    if (selected_record == null)
                    {
                        throw new Exception("19"); //Albo błąd w linijce z selected_record
                    }
                    else
                    {
                        List<Encrypted_Object> encrypted_fields = [
                            new Encrypted_Object { id = 0, encryptedValue = selected_record.product_name },
                            new Encrypted_Object { id = 1, encryptedValue = selected_record.price },
                            new Encrypted_Object { id = 2, encryptedValue = selected_record.comment },
                        ];

                        if(selected_record.item_trading_type_id_FK.id != 3)
                        {
                            encrypted_fields.Add(new Encrypted_Object { id = 3, encryptedValue = selected_record.item_type_id_FK.item_type });
                        }

                        List<Decrypted_Object> decrypted_fields = await Crypto.DecryptList(obj.session, encrypted_fields);

                        Item_Model return_obj = new Item_Model
                        {
                            id = selected_record.id,
                            catalog_number = selected_record.catalog_number,
                            weight_kg = selected_record.weight_kg,
                            count = selected_record.count,
                            blocked_count = selected_record.blocked_count,
                            tax_pct = selected_record.tax_pct,
                            item_type = "service", //if that record isn't service, then it will be overwritten in foreach loop
                            item_trading_type_id_FK = new Item_Trading_Type_Model { id = selected_record.item_trading_type_id_FK.id, trading_type_en = selected_record.item_trading_type_id_FK.trading_type_en, trading_type_pl = selected_record.item_trading_type_id_FK.trading_type_pl },
                            item_counting_type_id_FK = new Item_Counting_Type_Model { id = selected_record.item_counting_type_id_FK.id, counting_type = selected_record.item_counting_type_id_FK.counting_type },
                        };

                        foreach (var field in decrypted_fields)
                        {
                            if (field == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                switch (field.id)
                                {
                                    case 0:
                                        return_obj.product_name = field.decryptedValue; break;
                                    case 1:
                                        return_obj.price = field.decryptedValue; break;
                                    case 2:
                                        return_obj.comment = field.decryptedValue; break;
                                    case 3:
                                        return_obj.item_type = field.decryptedValue; break;
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
         * Get_All_Item_Or_Service method
         * This method gets all of the records in the Item table that aren't services and returns them in a list.
         * 
         * It accepts Get_All_Items_Data object as input.
         */
        public async Task<List<Item_Model_List>> Get_All_Item_Or_Service(Get_All_Item_Data input)
        {
            if (input == null)
            {
                throw new Exception("14");
            }
            else
            {
                if (await Session_Checker.ActiveSession(input.session))
                {
                    _context = new DB_Context(input.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    List<Item> records_list = _context.Item.Include(s => s.item_type_id_FK).Include(e => e.item_trading_type_id_FK).Include(r => r.item_counting_type_id_FK).ToList();

                    if (records_list == null)
                    {
                        throw new Exception("19");
                    }
                    else
                    {

                        List<Encrypted_Object> product_name_list = new List<Encrypted_Object>();
                        List<Encrypted_Object> price_list = new List<Encrypted_Object>();
                        List<Encrypted_Object> comment_list = new List<Encrypted_Object>();
                        List<Encrypted_Object> item_type_list = new List<Encrypted_Object>();

                        List<Item_Model_List> records_decrypted = new List<Item_Model_List>();

                        foreach (var field in records_list)
                        {
                            if (input.get_item)
                            {

                                if(field.item_trading_type_id_FK.id == 3)
                                {
                                    continue;
                                }

                            }
                            else
                            {

                                if (field.item_trading_type_id_FK.id != 3)
                                {
                                    continue;
                                }

                            }

                            Item_Model_List decrypted_model = new Item_Model_List
                            {
                                id = field.id,
                                catalog_number = field.catalog_number,
                                weight_kg = field.weight_kg,
                                count = field.count,
                                blocked_count = field.blocked_count,
                                tax_pct = field.tax_pct,
                                trading_type_eng = field.item_trading_type_id_FK.trading_type_en,
                                trading_type_pl = field.item_trading_type_id_FK.trading_type_pl,
                                counting_type = field.item_counting_type_id_FK.counting_type
                            };

                            product_name_list.Add(new Encrypted_Object { id = field.id, encryptedValue = field.product_name });
                            price_list.Add(new Encrypted_Object { id = field.id, encryptedValue = field.price });
                            comment_list.Add(new Encrypted_Object { id = field.id, encryptedValue = field.comment });

                            if (input.get_item)
                            {
                                item_type_list.Add(new Encrypted_Object { id = field.id, encryptedValue = field.item_type_id_FK.item_type });
                            }
                            else
                            {
                                decrypted_model.item_type = "service";
                            }

                            records_decrypted.Add(decrypted_model);


                        }

                        List<Decrypted_Object> product_name_list_decrypted = await Crypto.DecryptList(input.session, product_name_list);
                        List<Decrypted_Object> price_list_decrypted = await Crypto.DecryptList(input.session, price_list);
                        List<Decrypted_Object> comment_list_decrypted = await Crypto.DecryptList(input.session, comment_list);
                        List<Decrypted_Object> item_type_list_decrypted = new List<Decrypted_Object>();

                        if (input.get_item)
                        {
                            item_type_list_decrypted = await Crypto.DecryptList(input.session, item_type_list);
                        }



                        foreach (var item in records_decrypted)
                        {
                            var product_name = product_name_list_decrypted.Where(s => s.id.Equals(item.id)).FirstOrDefault();
                            if (product_name == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                item.product_name = product_name.decryptedValue;
                            }

                            var price = price_list_decrypted.Where(s => s.id.Equals(item.id)).FirstOrDefault();
                            if (price == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                item.price = price.decryptedValue;
                            }

                            var comment = comment_list_decrypted.Where(s => s.id.Equals(item.id)).FirstOrDefault();
                            if (comment == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                item.comment = comment.decryptedValue;
                            }

                            if (input.get_item)
                            {
                                var item_type = item_type_list_decrypted.Where(s => s.id.Equals(item.id)).FirstOrDefault();
                                if (item_type == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.item_type = item_type.decryptedValue;
                                }
                            }

                        }
                        return records_decrypted;

                    }
                }
                else
                {
                    throw new Exception("1");
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

        //Check number of records
        public async Task<long> Get_Number_Of_Items(Get_Number_Of_Items_Data input)
        {
            if (input == null)
            {
                throw new Exception("14");
            }
            else
            {
                if (await Session_Checker.ActiveSession(input.session))
                {
                    _context = new DB_Context(input.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    List<Item> records_list = _context.Item.ToList();

                    if (records_list == null)
                    {
                        return 0;
                    }
                    else
                    {
                        return records_list.Count();
                    }
                }
                else
                {
                    throw new Exception("1");
                }
            }
        }
        //Get by Page
        public async Task<List<Item_Model_List>> Get_Item_Page(Get_Item_By_Page_Data input)
        {
            if (input == null)
            {
                throw new Exception("14");
            }
            else
            {
                if (await Session_Checker.ActiveSession(input.session))
                {
                    _context = new DB_Context(input.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    List<Item> records_list = _context.Item.Include(s => s.item_type_id_FK).Include(e => e.item_trading_type_id_FK).Include(r => r.item_counting_type_id_FK).ToList();

                    if (records_list == null)
                    {
                        throw new Exception("19");
                    }
                    else
                    {
                        int page_lenght = input.page_Size;
                        int start_position = input.page_ID * page_lenght;

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

                            List<Encrypted_Object> product_name_list = new List<Encrypted_Object>();
                            List<Encrypted_Object> price_list = new List<Encrypted_Object>();
                            List<Encrypted_Object> comment_list = new List<Encrypted_Object>();
                            List<Encrypted_Object> item_type_list = new List<Encrypted_Object>();

                            List<Item_Model_List> records_decrypted = new List<Item_Model_List>();

                            foreach (var field in records_list)
                            {
                                records_decrypted.Add(new Item_Model_List
                                {
                                    id = field.id,
                                    catalog_number = field.catalog_number,
                                    weight_kg = field.weight_kg,
                                    count = field.count,
                                    blocked_count = field.blocked_count,
                                    tax_pct = field.tax_pct,
                                    trading_type_eng = field.item_trading_type_id_FK.trading_type_en,
                                    trading_type_pl = field.item_trading_type_id_FK.trading_type_pl,
                                    counting_type = field.item_counting_type_id_FK.counting_type
                                });

                                product_name_list.Add(new Encrypted_Object { id = field.id, encryptedValue = field.product_name });
                                price_list.Add(new Encrypted_Object { id = field.id, encryptedValue = field.price });
                                comment_list.Add(new Encrypted_Object { id = field.id, encryptedValue = field.comment });
                                item_type_list.Add(new Encrypted_Object { id = field.id, encryptedValue = field.item_type_id_FK.item_type });
                            }

                            List<Decrypted_Object> product_name_list_decrypted = await Crypto.DecryptList(input.session, product_name_list);
                            List<Decrypted_Object> price_list_decrypted = await Crypto.DecryptList(input.session, price_list);
                            List<Decrypted_Object> comment_list_decrypted = await Crypto.DecryptList(input.session, comment_list);
                            List<Decrypted_Object> item_type_list_decrypted = await Crypto.DecryptList(input.session, item_type_list);

                            foreach (var item in records_decrypted)
                            {
                                var product_name = product_name_list_decrypted.Where(s => s.id.Equals(item.id)).FirstOrDefault();
                                if (product_name == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.product_name = product_name.decryptedValue;
                                }

                                var price = price_list_decrypted.Where(s => s.id.Equals(item.id)).FirstOrDefault();
                                if (price == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.price = price.decryptedValue;
                                }

                                var comment = comment_list_decrypted.Where(s => s.id.Equals(item.id)).FirstOrDefault();
                                if (comment == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.comment = comment.decryptedValue;
                                }

                                var item_type = item_type_list_decrypted.Where(s => s.id.Equals(item.id)).FirstOrDefault();
                                if (item_type == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.item_type = item_type.decryptedValue;
                                }
                            }
                            return records_decrypted;
                        }
                        else
                        {
                            throw new Exception("19");
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
