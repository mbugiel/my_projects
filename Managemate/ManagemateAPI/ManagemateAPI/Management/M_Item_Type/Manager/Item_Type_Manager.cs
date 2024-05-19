using ManagemateAPI.Database.Context;
using ManagemateAPI.Database.Tables;
using ManagemateAPI.Encryption;
using ManagemateAPI.Encryption.Input_Objects;
using ManagemateAPI.Information;
using ManagemateAPI.Management.M_Item_Type.Input_Objects;
using ManagemateAPI.Management.M_Item_Type.Table_Model;
using ManagemateAPI.Management.M_Session.Manager;

/*
 * This is the Item_Type_Manager with methods dedicated to the Item_Type table.
 * 
 * It contains methods to:
 * add records,
 * edit records,
 * delete records,
 * get record by id,
 * get all the records.
 */
namespace ManagemateAPI.Management.M_Item_Type.Manager
{
    public class Item_Type_Manager
    {

        private DB_Context _context;
        private readonly IConfiguration _configuration;


        public Item_Type_Manager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /* 
         * Add_Item_Type method
         * This method is used to add new records to the Item_Type table.
         * 
         * It accepts Add_Item_Type_Data object as input.
         * It then adds new record with values based on the data given in the input object.
         */
        public async Task<string> Add_Item_Type(Add_Item_Type_Data obj)
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

                    var encrypted_type = await Crypto.Encrypt(obj.session, obj.item_type);
                    var encrypted_rate = await Crypto.Encrypt(obj.session, obj.rate.ToString());

                    var item_type_checker = _context.Item_Type.Where(i => i.item_type.Equals(encrypted_type) && i.id > 0).FirstOrDefault();

                    if (item_type_checker != null)
                    {
                        throw new Exception("18"); // Already exist
                    }
                    else
                    {
                        Item_Type new_record = new Item_Type
                        {
                            item_type = encrypted_type,
                            rate = encrypted_rate
                        };
                        _context.Item_Type.Add(new_record);
                        _context.SaveChanges();

                        return Info.SUCCESSFULLY_ADDED;
                    }
                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }

            }

        }

        /* 
         * Edit_Item_Type method
         * This method is used to edit a record in the Item_Type table.
         * 
         * It accepts Edit_Item_Type_Data object as input.
         * It then changes values of a record with those given in the input object only if its ID matches the one in the input object.
         */
        public async Task<string> Edit_Item_Type(Edit_Item_Type_Data obj)
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

                    var edited_record = _context.Item_Type.Where(c => c.id.Equals(obj.item_type_id)).FirstOrDefault();

                    if (edited_record != null)
                    {

                        var encrypted_type = await Crypto.Encrypt(obj.session, obj.item_type);
                        var encrypted_rate = await Crypto.Encrypt(obj.session, obj.rate.ToString());

                        var item_type_checker = _context.Item_Type.Where(i => i.item_type.Equals(encrypted_type) && i.rate.Equals(encrypted_rate) && i.id != edited_record.id && i.id > 0).FirstOrDefault();

                        if (item_type_checker != null)
                        {
                            throw new Exception("18"); // Already in use
                        }
                        else
                        {

                            edited_record.item_type = encrypted_type;
                            edited_record.rate = encrypted_rate;

                            _context.SaveChanges();

                            return Info.SUCCESSFULLY_CHANGED;
                        }
                    }
                    else
                    {
                        throw new Exception("19"); // edited record not found in db
                    }
                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }
            }
        }

        /*
         * Delete_Item_Type method
         * This method is used to a record from the Item_Type table.
         *  
         * It accepts Delete_Item_Type_Data object as input.
         * Then it deletes a record if its ID matches the one given in the input object.
         */
        public async Task<string> Delete_Item_Type(Delete_Item_Type_Data obj)
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

                    var id_exits = _context.Item_Type.Where(i => i.id.Equals(obj.item_type_id) && i.id > 0).FirstOrDefault();

                    if (id_exits == null)
                    {
                        throw new Exception("19");// item type not found
                    }
                    else
                    {
                        _context.Item_Type.Remove(id_exits);
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
         * Get_Item_Type_By_ID method
         * This method gets a record from the Item_Type table by its ID and returns it.
         * 
         * It accepts Get_Item_Type_By_ID_Data object as input.
         * Then it gets a records that has the same ID as the ID given in the input object
         */
        public async Task<Item_Type_Model> Get_Item_Type_By_ID(Get_Item_Type_By_ID_Data obj)
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

                    var item_type = _context.Item_Type.Where(c => c.id.Equals(obj.item_type_id)).FirstOrDefault();

                    if (item_type == null)
                    {
                        throw new Exception("19");// item type not found
                    }
                    else
                    {
                        Item_Type_Model item_type_model = new Item_Type_Model {
                            
                            id = item_type.id,
                            item_type = await Crypto.Decrypt(obj.session, item_type.item_type),
                            rate = Convert.ToDouble(await Crypto.Decrypt(obj.session, item_type.item_type))
                        };

                        return item_type_model;
                    }


                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }

            }

        }

        /*
         * Get_All_Item_Type method
         * This method gets all of the records in the Item_Type table and returns them in a list.
         * 
         * It accepts Get_All_Item_Type_Data object as input.
         */
        public async Task<List<Item_Type_Model>> Get_All_Item_Type(Get_All_Item_Type_Data obj)
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

                    var item_types = _context.Item_Type.Where(i => i.id > 0).ToList();

                    if (item_types == null)
                    {
                        throw new Exception("19");// item types not found
                    }
                    else
                    {
                        List<Item_Type_Model> item_types_model = new List<Item_Type_Model>();

                        List<Encrypted_Object> encrypted_types = new List<Encrypted_Object>();
                        List<Encrypted_Object> encrypted_rates = new List<Encrypted_Object>();

                        foreach (var type in item_types)
                        {

                            item_types_model.Add(new Item_Type_Model { id = type.id });

                            encrypted_types.Add(new Encrypted_Object { id = type.id, encryptedValue = type.item_type });
                            encrypted_rates.Add(new Encrypted_Object { id = type.id, encryptedValue = type.rate });
                        }


                        List<Decrypted_Object> decrypted_types = await Crypto.DecryptList(obj.session, encrypted_types);
                        List<Decrypted_Object> decrypted_rates = await Crypto.DecryptList(obj.session, encrypted_rates);

                        if (decrypted_types == null || decrypted_rates == null)
                        {
                            throw new Exception("3"); // decryption error
                        }
                        else
                        {
                            foreach (var type in item_types_model)
                            {

                                var type_name = decrypted_types.Where(t => t.id.Equals(type.id)).FirstOrDefault();
                                if (type_name == null)
                                {
                                    throw new Exception("3"); // decryption error
                                }
                                else
                                {
                                    type.item_type = type_name.decryptedValue;
                                }


                                var rate = decrypted_rates.Where(r => r.id.Equals(type.id)).FirstOrDefault();
                                if (rate == null)
                                {
                                    throw new Exception("3"); // decryption error
                                }
                                else
                                {
                                    type.rate = Convert.ToDouble(rate.decryptedValue);
                                }


                            }


                        }




                        return item_types_model;

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
