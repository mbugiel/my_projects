using ManagemateAPI.Database.Context;
using ManagemateAPI.Database.Tables;
using ManagemateAPI.Encryption;
using ManagemateAPI.Encryption.Input_Objects;
using ManagemateAPI.Information;
using ManagemateAPI.Management.M_Cities_List.Input_Objects;
using ManagemateAPI.Management.M_Cities_List.Table_Model;
using ManagemateAPI.Management.M_Session.Manager;

/*
 * This is the Cities_List_Manager with methods dedicated to the Cities_List table.
 * 
 * It contains methods to:
 * add records,
 * edit records,
 * delete records,
 * get records by id,
 * get all the records.
 */

namespace ManagemateAPI.Management.M_Cities_List.Manager
{
    public class Cities_List_Manager
    {
        private DB_Context _context;
        private readonly IConfiguration _configuration;

        public Cities_List_Manager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /* 
         * Add_Cities_List method
         * This method is used to add new records to the Cities_List table.
         * 
         * It accepts Add_Cities_List_Data object as input.
         * It then adds new record with values based on the data given in the input object.
         */
        public async Task<string> Add_Cities_List(Add_Cities_List_Data obj)
        {
            //Checking if object is empty.
            if (obj == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                //Checking if session token is valid and active.
                if (await Session_Checker.ActiveSession(obj.session))
                {
                    _context = new DB_Context(obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    //Checking for any duplications
                    List<Cities_List> records_list = _context.Cities_List.ToList();
                    List<Encrypted_Object> encrypted_column = new List<Encrypted_Object>();
                    bool duplication_checker = false;

                    if (records_list.Count == 0 || records_list == null)
                    {
                        duplication_checker = false;
                    }
                    else
                    {
                        foreach (var field in records_list)
                        {
                            encrypted_column.Add(new Encrypted_Object { id = field.id, encryptedValue = field.city });
                        }

                        List<Decrypted_Object> decrypted_column = await Crypto.DecryptList(obj.session, encrypted_column);

                        foreach (var field in decrypted_column)
                        {
                            if (field.decryptedValue == obj.city)
                            {
                                //Triggering duplication checker if input object city field matches any that is already in the table.
                                duplication_checker = true;
                            }
                        }
                    }

                    if (duplication_checker != false)
                    {
                        throw new Exception("18"); //_18_DUPLICATE_ERROR
                    }
                    else
                    {
                        //Creating a new Cities_List object and adding it to the Cities_List table.
                        Cities_List new_record = new Cities_List
                        {
                            city = await Crypto.Encrypt(obj.session, obj.city),
                        };
                        _context.Cities_List.Add(new_record);
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
         * Edit_Cities_List method
         * This method is used to edit a record in the Cities_List table.
         * 
         * It accepts Edit_Cities_List_Data object as input.
         * It then changes values of a record with those given in the input object only if its ID matches the one in the input object.
         */
        public async Task<string> Edit_Cities_List(Edit_Cities_List_Data obj)
        {
            //Checking if input object is empty.
            if (obj == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                //Checking if session token is valid and active
                if (await Session_Checker.ActiveSession(obj.session))
                {
                    //Creating new DB_Context object and ensuring that the database if created.
                    _context = new DB_Context(obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    //Checking if the edited record exists
                    var edited_record = _context.Cities_List.Where(h => h.id == obj.id).FirstOrDefault();
                    if (edited_record != null)
                    {
                        var city_checker = _context.Cities_List.Where(i => i.city.Equals(obj.city)).FirstOrDefault();

                        if (city_checker != null)
                        {
                            throw new Exception("18"); //_18_DUPLICATE_ERROR
                        }
                        else
                        {
                            edited_record.id = obj.id;
                            edited_record.city = await Crypto.Encrypt(obj.session, obj.city);

                            _context.SaveChanges();

                            return Info.SUCCESSFULLY_CHANGED;
                        }
                    }
                    else
                    {
                        throw new Exception("19");
                    }
                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }
            }
        }


        /*
         * Delete_Cities_List method
         * This method is used to a record from the Cities_List table.
         *  
         * It accepts Delete_Cities_List_Data object as input.
         * Then it deletes a record if its ID matches the one given in the input object.
         */
        public async Task<string> Delete_Cities_List(Delete_Cities_List_Data obj)
        {
             //Checking if input object is null
            if (obj == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                //Checking if session token is valid and active
                if (await Session_Checker.ActiveSession(obj.session))
                {
                    //Creating new DB_Context object and ensuring that the database if created.
                    _context = new DB_Context(obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    //Checking if ID given in the input object corresponds to ID of any record in the Cities_List table.
                    var id_exits = _context.Cities_List.Where(i => i.id.Equals(obj.id)).FirstOrDefault();
                    if (id_exits == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }
                    else
                    {
                        //Deleting the record
                        _context.Cities_List.Remove(id_exits);
                        //Saving changes to the Cities_List table.
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
         * Get_Cities_List_By_ID method
         * This method gets a record from the Cities_List table by its ID and returns it.
         * 
         * It accepts Get_Cities_List_By_ID_Data object as input.
         * Then it gets a records that has the same ID as the ID given in the input object
         */
        public async Task<Cities_List_Model> Get_Cities_List_By_ID(Get_Cities_List_By_ID obj)
        {
            //Checking if input object is null
            if (obj == null)
            {
                throw new Exception("14");
            }
            else
            {
                //Checking if session token is valid and active
                if (await Session_Checker.ActiveSession(obj.session))
                {
                    //Creating new DB_Context object and ensuring that the database if created.
                    _context = new DB_Context(obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    //Getting the object from the database with matching ID
                    var selected_record = _context.Cities_List.Where(o => o.id.Equals(obj.id_to_get)).FirstOrDefault();
                    if (selected_record == null)
                    {
                        throw new Exception("19"); //_19_OBJECT_NOT_FOUND
                    }
                    else
                    {
                        //Creating a list with encrypted fields form Cities_List table.
                        List<Encrypted_Object> encrypted_fields = [
                            new Encrypted_Object { id = 0, encryptedValue = selected_record.city },
                        ];

                        //Decrypting the list
                        List<Decrypted_Object> decrypted_fields = await Crypto.DecryptList(obj.session, encrypted_fields);
                        
                        //Creating the return object
                        Cities_List_Model return_obj = new Cities_List_Model
                        {
                            id = selected_record.id,
                        };

                        //Adding values to encrypted fields from the decrypted_fields list.
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
                                        return_obj.city = field.decryptedValue; break;
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
         * Get_All_Cities_List endpoint
         * This endpoint is used to to all the records from the Cities_List table.
         * 
         * It accepts Get_All_Cities_List_Data object.
         * The given object is handed over to the Get_All_Cities_List method in the Cities_List_Manager.
         */
        public async Task<List<Cities_List_Model_List>> Get_All_Cities_List(Get_All_Cities_List_Data obj)
        {
            //Checking if input object is null
            if (obj == null)
            {
                throw new Exception("14");
            }
            else
            {
                //Checking if session token is valid and active
                if (await Session_Checker.ActiveSession(obj.session))
                {
                    //Creating new DB_Context object and ensuring that the database if created.
                    _context = new DB_Context(obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    //Creating a list of Cities_List objects.
                    List<Cities_List> records_list = _context.Cities_List.ToList();
                    if (records_list == null)
                    {
                        throw new Exception("19");//_19_OBJECT_NOT_FOUND
                    }
                    else
                    {
                        //Creating Encrypted_Object lists for encrypted fields in the Cities_List table.
                        List<Encrypted_Object> cities_list = new List<Encrypted_Object>();

                        //Creating a list for records with decrypted values
                        List<Cities_List_Model_List> records_decrypted = new List<Cities_List_Model_List>();

                        //Filling the decrypted_records list with records
                        foreach (var field in records_list)
                        {
                            //Creating Cities_List_Model_List object and adding values to unencrypted fields
                            records_decrypted.Add(new Cities_List_Model_List
                            {
                                id = field.id,
                            });

                            //Filling encrypted lists with encrypted values
                            cities_list.Add(new Encrypted_Object { id = field.id, encryptedValue = field.city });
                        }

                        //Decrypting encrypted field lists
                        List<Decrypted_Object> cities_list_decrypted = await Crypto.DecryptList(obj.session, cities_list);

                        //Adding the rest of values to the Cities_List_Model_List in the decrypted_records list
                        foreach (var item in records_decrypted)
                        {
                            var city = cities_list_decrypted.Where(s => s.id.Equals(item.id)).FirstOrDefault();
                            if (city == null)
                            {
                                throw new Exception("3");//_3_DECRYPTION_ERROR
                            }
                            else
                            {
                                item.city = city.decryptedValue;
                            }
                        }
                        return records_decrypted;

                    }
                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }
            }
        }







        //Not in use

        public async Task<List<Cities_List_Model_List>> Get_Cities_List_By_Page(Get_Cities_List_By_Page input)
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

                    List<Cities_List> records_list = _context.Cities_List.ToList();

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

                            List<Encrypted_Object> cities_list = new List<Encrypted_Object>();

                            List<Cities_List_Model_List> records_decrypted = new List<Cities_List_Model_List>();

                            foreach (var field in records_list)
                            {
                                records_decrypted.Add(new Cities_List_Model_List
                                {
                                    id = field.id,
                                });

                                cities_list.Add(new Encrypted_Object { id = field.id, encryptedValue = field.city });
                            }

                            List<Decrypted_Object> cities_list_decrypted = await Crypto.DecryptList(input.session, cities_list);

                            foreach (var item in records_decrypted)
                            {
                                var city = cities_list_decrypted.Where(s => s.id.Equals(item.id)).FirstOrDefault();
                                if (city == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.city = city.decryptedValue;
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
