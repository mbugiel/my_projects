using ManagemateAPI.Database.Context;
using ManagemateAPI.Database.Tables;
using ManagemateAPI.Encryption;
using ManagemateAPI.Encryption.Input_Objects;
using ManagemateAPI.Information;
using ManagemateAPI.Management.M_Authorized_Worker.Input_Objects;
using ManagemateAPI.Management.M_Authorized_Worker.Table_Model;
using ManagemateAPI.Management.M_Client.Table_Model;
using ManagemateAPI.Management.M_Session.Manager;
using Microsoft.EntityFrameworkCore;

/*
 * This is the Authorized_Worker_Manager with methods dedicated to the Authorized_Worker table.
 * 
 * It contains methods to:
 * add records,
 * edit records,
 * delete records,
 * get record by id,
 * get all the records.
 */
namespace ManagemateAPI.Management.M_Authorized_Worker.Manager
{
    public class Authorized_Worker_Manager
    {
        private DB_Context _context;
        private readonly IConfiguration _configuration;

        public Authorized_Worker_Manager(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        /* 
         * Add_Authorized_Worker method
         * This method is used to add new records to the Authorized_Worker table.
         * 
         * It accepts Add_Authorized_Worker_Data object as input.
         * It then adds new record with values based on the data given in the input object.
         */
        public async Task<string> Add_Authorized_Worker(Add_Authorized_Worker_Data obj)
        {
            //Checking if object is empty
            if (obj == null)
            {
                throw new Exception("14");//_14_NULL_ERROR
            }
            else
            {
                //Checking if session token is valid and active
                if (await Session_Checker.ActiveSession(obj.session))
                {
                    _context = new DB_Context(obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();
                   
                    //Checking if the are any duplications in phone_number and email fields
                    List<Authorized_Worker> records_list = _context.Authorized_Worker.ToList();
                    List<Encrypted_Object> encrypted_phoneNumbers = new List<Encrypted_Object>();
                    List<Encrypted_Object> encrypted_emails = new List<Encrypted_Object>();
                    bool phone_number_checker = false;
                    bool email_checker = false;

                    if (records_list.Count == 0 || records_list == null)
                    {
                        //If the Authorized_Worker table is empty it proceds further without triggering duplication checkers
                        phone_number_checker = false;
                        email_checker = false;
                    }
                    else
                    {
                        //In case the Authorized_Worker table isn't it checks for duplications

                        foreach (var field in records_list)
                        {
                            encrypted_emails.Add(new Encrypted_Object { id = field.id, encryptedValue = field.email });
                            encrypted_phoneNumbers.Add(new Encrypted_Object { id = field.id, encryptedValue = field.phone_number });
                        }

                        List<Decrypted_Object> decrypted_emails = await Crypto.DecryptList(obj.session, encrypted_emails);
                        List<Decrypted_Object> decrypted_phoneNumbers = await Crypto.DecryptList(obj.session, encrypted_phoneNumbers);

                        foreach (var field in decrypted_emails)
                        {
                            if (field.decryptedValue == obj.email)
                            {
                                email_checker = true;
                            }
                        }

                        foreach (var field in decrypted_phoneNumbers)
                        {
                            if (field.decryptedValue == obj.phone_number)
                            {
                                email_checker = true;
                            }
                        }
                    }

                    if (phone_number_checker == false && email_checker == false)
                    {
                        //If there aren't any duplications it proceds


                        var client = _context.Client.Where(c => c.id.Equals(obj.client_id_FK)).FirstOrDefault();

                        if (client == null)
                        {
                            //Throws an error if the client ID given doesn't correspond to any in the Client table.
                            throw new Exception("19");//_19_OBJECT_NOT_FOUND
                        }
                        else
                        {
                            //Creating new Authorized_Worker object with data given in the input object.
                            Authorized_Worker new_record = new Authorized_Worker
                            {
                                client_id_FK = client,
                                name = await Crypto.Encrypt(obj.session, obj.name),
                                surname = await Crypto.Encrypt(obj.session, obj.surname),
                                phone_number = await Crypto.Encrypt(obj.session, obj.phone_number),
                                email = await Crypto.Encrypt(obj.session, obj.email),
                                contact = obj.contact,
                                collection = obj.collection,
                                comment = await Crypto.Encrypt(obj.session, obj.comment),
                            };

                            //New object is added to the Authorized_Worker table as a new record.
                            _context.Authorized_Worker.Add(new_record);
                            _context.SaveChanges();

                            return Info.SUCCESSFULLY_ADDED;
                        }
                    }
                    else
                    {
                        //Otherwise it throws duplication error
                        throw new Exception("18"); //_18_DUPLICATE_ERROR
                    }
                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }
            }
        }

        /* 
         * Edit_Authorized_Worker method
         * This method is used to edit a record in the Authorized_Worker table.
         * 
         * It accepts Edit_Authorized_Worker_Data object as input.
         * It then changes values of a record with those given in the input object only if its ID matches the one in the input object.
         */
        public async Task<string> Edit_Authorized_Worker(Edit_Authorized_Worker_Data obj)
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
                    var edited_record = _context.Authorized_Worker.Where(a => a.id.Equals(obj.id)).Include(a => a.client_id_FK).FirstOrDefault();
                    if (edited_record != null)
                    {
                        //Checking if there are any duplications in fields

                        List<Authorized_Worker> records_list = _context.Authorized_Worker.Where(a => a.client_id_FK.Equals(edited_record.client_id_FK)).ToList();

                        List<Encrypted_Object> encrypted_phoneNumbers = new List<Encrypted_Object>();
                        List<Encrypted_Object> encrypted_emails = new List<Encrypted_Object>();
                        List<Encrypted_Object> encrypted_names = new List<Encrypted_Object>();
                        List<Encrypted_Object> encrypted_surnames = new List<Encrypted_Object>();

                        bool phone_number_checker = false;
                        bool email_checker = false;
                        bool name_checker = false;
                        bool surname_checker = false;

                        if (records_list.Count == 0 || records_list == null)
                        {
                            throw new Exception("19"); // no authorized workers found in DB
                        }
                        else
                        {
                            foreach (Authorized_Worker record in records_list)
                            {
                                encrypted_emails.Add(new Encrypted_Object { id = record.id, encryptedValue = record.email });
                                encrypted_phoneNumbers.Add(new Encrypted_Object { id = record.id, encryptedValue = record.phone_number });
                                encrypted_names.Add(new Encrypted_Object { id = record.id, encryptedValue = record.name });
                                encrypted_surnames.Add(new Encrypted_Object { id = record.id, encryptedValue = record.surname });
                            }

                            List<Decrypted_Object> decrypted_emails = await Crypto.DecryptList(obj.session, encrypted_emails);
                            List<Decrypted_Object> decrypted_phoneNumbers = await Crypto.DecryptList(obj.session, encrypted_phoneNumbers);
                            List<Decrypted_Object> decrypted_names = await Crypto.DecryptList(obj.session, encrypted_names);
                            List<Decrypted_Object> decrypted_surnames = await Crypto.DecryptList(obj.session, encrypted_surnames);


                            foreach(Authorized_Worker record in records_list)
                            {

                                if (record.id.Equals(edited_record.id))
                                {
                                    continue;
                                }

                                var email = decrypted_emails.Where(e => e.id.Equals(record.id)).FirstOrDefault();
                                if (email != null)
                                {
                                    if (email.decryptedValue.Equals(obj.email))
                                    {
                                        email_checker = true;
                                    }
                                }
                                else
                                {
                                    throw new Exception("3");//error while decrypting data 
                                }



                                var phone = decrypted_phoneNumbers.Where(e => e.id.Equals(record.id)).FirstOrDefault();
                                if (phone != null)
                                {
                                    if (phone.decryptedValue.Equals(obj.phone_number))
                                    {
                                        phone_number_checker = true;
                                    }
                                }
                                else
                                {
                                    throw new Exception("3");//error while decrypting data 
                                }





                                var name = decrypted_names.Where(e => e.id.Equals(record.id)).FirstOrDefault();
                                if (name != null)
                                {
                                    if (name.decryptedValue.Equals(obj.name))
                                    {
                                        name_checker = true;
                                    }
                                }
                                else
                                {
                                    throw new Exception("3");//error while decrypting data 
                                }




                                var surname = decrypted_surnames.Where(e => e.id.Equals(record.id)).FirstOrDefault();
                                if (surname != null)
                                {
                                    if (surname.decryptedValue.Equals(obj.surname))
                                    {
                                        surname_checker = true;
                                    }
                                }
                                else
                                {
                                    throw new Exception("3");//error while decrypting data 
                                }




                            }


                        }

                        if (phone_number_checker && email_checker && name_checker && surname_checker)
                        {
                            //If all of the checkers where triggered it throws an exception
                            throw new Exception("18"); //_18_DUPLICATE_ERROR
                        }
                        else
                        {
                            //If there aren't any duplications it proceds further

                            //Changing values of the record with new ones from the input object
                            edited_record.id = obj.id;
                            edited_record.name = await Crypto.Encrypt(obj.session, obj.name);
                            edited_record.surname = await Crypto.Encrypt(obj.session, obj.surname);
                            edited_record.phone_number = await Crypto.Encrypt(obj.session, obj.phone_number);
                            edited_record.email = await Crypto.Encrypt(obj.session, obj.email);
                            edited_record.contact = obj.contact;
                            edited_record.collection = obj.collection;
                            edited_record.comment = await Crypto.Encrypt(obj.session, obj.comment);

                            //Saving changes
                            _context.SaveChanges();

                            return Info.SUCCESSFULLY_CHANGED;

                        }
                    }
                    else
                    {
                        throw new Exception("19");//_19_OBJECT_NOT_FOUND
                    }
                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }
            }
        }

        /*
         * Delete_Authorized_Worker method
         * This method is used to a record from the Authorized_Worker table.
         *  
         * It accepts Delete_Authorized_Worker_Data object as input.
         * Then it deletes a record if its ID matches the one given in the input object.
         */
        public async Task<string> Delete_Authorized_Worker(Delete_Authorized_Worker_Data obj)
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

                    //Checking if ID given in the input object corresponds to ID of any record in the Authorized_Worker table.
                    var id_exits = _context.Authorized_Worker.Where(i => i.id.Equals(obj.id)).FirstOrDefault();
                    if (id_exits == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }
                    else
                    {
                        //Deleting the record
                        _context.Authorized_Worker.Remove(id_exits);
                        //Saving changes
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
         * Get_Authorized_Worker_By_ID method
         * This method gets a record from the Authorized_Worker table by its ID and returns it.
         * 
         * It accepts Get_Authorized_Worker_By_ID_Data object as input.
         * Then it gets a records that has the same ID as the ID given in the input object
         */
        public async Task<Authorized_Worker_Model> Get_Authorized_Worker_By_ID(Get_Authorized_Worker_By_ID_Data obj)
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
                    var selected_record = _context.Authorized_Worker.Where(o => o.id.Equals(obj.id_to_get)).Include(j => j.client_id_FK).ThenInclude(c => c.city_id_FK).FirstOrDefault();

                    //Checking if object is null
                    if (selected_record == null)
                    {
                        throw new Exception("19"); //_19_OBJECT_NOT_FOUND
                    }
                    else
                    {
                        //Creating a list with encrypted fields form Authorized_Worker and Client tables
                        List<Encrypted_Object> encrypted_items = [
                            //Encrypted fields from Authorized_Worker table
                            new Encrypted_Object { id = 0, encryptedValue = selected_record.name },
                            new Encrypted_Object { id = 1, encryptedValue = selected_record.surname },
                            new Encrypted_Object { id = 2, encryptedValue = selected_record.phone_number },
                            new Encrypted_Object { id = 3, encryptedValue = selected_record.email },
                            new Encrypted_Object { id = 4, encryptedValue = selected_record.comment }
                        ];

                        //Decrypting the list
                        List<Decrypted_Object> decrypted_items = await Crypto.DecryptList(obj.session, encrypted_items);

                        //Creating an object to return
                        Authorized_Worker_Model return_obj = new Authorized_Worker_Model
                        {
                            //Adding values to unencrypted fields
                            id = selected_record.id,
                            contact = selected_record.contact,
                            collection = selected_record.collection
                        };

                        //Adding values to encrypted fields from the decrypted_items list.
                        foreach (var item in decrypted_items)
                        {
                            if (item == null)
                            {
                                throw new Exception("3");//_3_DECRYPTION_ERROR
                            }
                            else
                            {
                                switch (item.id)
                                {
                                    case 0:
                                        return_obj.name = item.decryptedValue; break;
                                    case 1:
                                        return_obj.surname = item.decryptedValue; break;
                                    case 2:
                                        return_obj.phone_number = item.decryptedValue; break;
                                    case 3:
                                        return_obj.email = item.decryptedValue; break;
                                    case 4:
                                        return_obj.comment = item.decryptedValue; break;
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
         * Get_All_Authorized_Worker method
         * This method gets all of the records in the Authorized_Worker table assigned to given client_ID and returns them in a list.
         * 
         * It accepts Get_All_Authorized_Worker_Data object as input.
         */
        public async Task<List<Authorized_Worker_Model_List>> Get_All_Authorized_Worker(Get_All_Authorized_Worker_Data obj)
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

                    //Creating a list of Authorized_Worker objects assigned to given Client ID.
                    List<Authorized_Worker> records_list = _context.Authorized_Worker.Where(a => a.client_id_FK.id.Equals(obj.client_id)).ToList();

                    //Checking if list is empty
                    if (records_list == null)
                    {
                        throw new Exception("19");//_19_OBJECT_NOT_FOUND
                    }
                    else
                    {
                        //Creating Encrypted_Object lists for encrypted fields in the Authorized_Worker table.
                        List<Encrypted_Object> name_encrypted = new List<Encrypted_Object>();
                        List<Encrypted_Object> surname_encrypted = new List<Encrypted_Object>();
                        List<Encrypted_Object> phone_number_encrypted = new List<Encrypted_Object>();
                        List<Encrypted_Object> email_encrypted = new List<Encrypted_Object>();
                        List<Encrypted_Object> comment_encrypted = new List<Encrypted_Object>();

                        //Creating a list for records with decrypted values
                        List<Authorized_Worker_Model_List> decrypted_records = new List<Authorized_Worker_Model_List>();

                        //Filling the decrypted_records list with records
                        foreach (var item in records_list)
                        {
                            //Creating Authorized_Worker_Model_List object and adding values to unencrypted fields
                            decrypted_records.Add(new Authorized_Worker_Model_List
                            {
                                id = item.id,
                                contact = item.contact,
                                collection = item.collection,
                            });

                            //Filling encrypted lists with encrypted values
                            name_encrypted.Add(new Encrypted_Object { id = item.id, encryptedValue = item.name });
                            surname_encrypted.Add(new Encrypted_Object { id = item.id, encryptedValue = item.surname });
                            phone_number_encrypted.Add(new Encrypted_Object { id = item.id, encryptedValue = item.phone_number });
                            email_encrypted.Add(new Encrypted_Object { id = item.id, encryptedValue = item.email });
                            comment_encrypted.Add(new Encrypted_Object { id = item.id, encryptedValue = item.comment });
                        }

                        //Decrypting encrypted field lists
                        List<Decrypted_Object> name_decrypted = await Crypto.DecryptList(obj.session, name_encrypted);
                        List<Decrypted_Object> surname_decrypted = await Crypto.DecryptList(obj.session, surname_encrypted);
                        List<Decrypted_Object> phone_number_decrypted = await Crypto.DecryptList(obj.session, phone_number_encrypted);
                        List<Decrypted_Object> email_decrypted = await Crypto.DecryptList(obj.session, email_encrypted);
                        List<Decrypted_Object> comment_decrypted = await Crypto.DecryptList(obj.session, comment_encrypted);

                        //Adding the rest of values to the Authorized_Worker_Model_List in the decrypted_records list
                        foreach (var item in decrypted_records)
                        {
                            var name = name_decrypted.Where(o => o.id.Equals(item.id)).FirstOrDefault();
                            if (name == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                item.name = name.decryptedValue;
                            }

                            var surname = surname_decrypted.Where(o => o.id.Equals(item.id)).FirstOrDefault();
                            if (surname == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                item.surname = surname.decryptedValue;
                            }

                            var phone_number = phone_number_decrypted.Where(o => o.id.Equals(item.id)).FirstOrDefault();
                            if (phone_number == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                item.phone_number = phone_number.decryptedValue;
                            }

                            var email = email_decrypted.Where(o => o.id.Equals(item.id)).FirstOrDefault();
                            if (email == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                item.email = email.decryptedValue;
                            }

                            var comment = comment_decrypted.Where(o => o.id.Equals(item.id)).FirstOrDefault();
                            if (comment == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                item.comment = comment.decryptedValue;
                            }

                        }
                        return decrypted_records;
                    }
                }
                else
                {
                    throw new Exception("14");//_14_NULL_ERROR
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
        public async Task<List<Authorized_Worker_Model_List>> Get_Authorized_Worker_By_Page(Get_Authorized_Worker_By_Page_Data obj)
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

                    List<Authorized_Worker> records_list = _context.Authorized_Worker.ToList();

                    if (records_list == null)
                    {
                        throw new Exception("19");
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

                            List<Encrypted_Object> name_encrypted = new List<Encrypted_Object>();
                            List<Encrypted_Object> surname_encrypted = new List<Encrypted_Object>();
                            List<Encrypted_Object> phone_number_encrypted = new List<Encrypted_Object>();
                            List<Encrypted_Object> email_encrypted = new List<Encrypted_Object>();
                            List<Encrypted_Object> comment_encrypted = new List<Encrypted_Object>();

                            List<Authorized_Worker_Model_List> decrypted_records = new List<Authorized_Worker_Model_List>();

                            foreach (var item in records_list)
                            {
                                decrypted_records.Add(new Authorized_Worker_Model_List
                                {
                                    id = item.id,
                                    contact = item.contact,
                                    collection = item.collection,
                                });

                                name_encrypted.Add(new Encrypted_Object { id = item.id, encryptedValue = item.name });
                                surname_encrypted.Add(new Encrypted_Object { id = item.id, encryptedValue = item.surname });
                                phone_number_encrypted.Add(new Encrypted_Object { id = item.id, encryptedValue = item.phone_number });
                                email_encrypted.Add(new Encrypted_Object { id = item.id, encryptedValue = item.email });
                                comment_encrypted.Add(new Encrypted_Object { id = item.id, encryptedValue = item.comment });
                            }

                            List<Decrypted_Object> name_decrypted = await Crypto.DecryptList(obj.session, name_encrypted);
                            List<Decrypted_Object> surname_decrypted = await Crypto.DecryptList(obj.session, surname_encrypted);
                            List<Decrypted_Object> phone_number_decrypted = await Crypto.DecryptList(obj.session, phone_number_encrypted);
                            List<Decrypted_Object> email_decrypted = await Crypto.DecryptList(obj.session, email_encrypted);
                            List<Decrypted_Object> comment_decrypted = await Crypto.DecryptList(obj.session, comment_encrypted);

                            foreach (var item in decrypted_records)
                            {
                                var name = name_decrypted.Where(o => o.id.Equals(item.id)).FirstOrDefault();
                                if (name == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.name = name.decryptedValue;
                                }

                                var surname = surname_decrypted.Where(o => o.id.Equals(item.id)).FirstOrDefault();
                                if (surname == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.surname = surname.decryptedValue;
                                }

                                var phone_number = phone_number_decrypted.Where(o => o.id.Equals(item.id)).FirstOrDefault();
                                if (phone_number == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.phone_number = phone_number.decryptedValue;
                                }

                                var email = email_decrypted.Where(o => o.id.Equals(item.id)).FirstOrDefault();
                                if (email == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.email = email.decryptedValue;
                                }

                                var comment = comment_decrypted.Where(o => o.id.Equals(item.id)).FirstOrDefault();
                                if (comment == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.comment = comment.decryptedValue;
                                }

                            }
                            return decrypted_records;
                        }
                        else
                        {
                            throw new Exception("19");
                        }
                    }
                }
                else
                {
                    throw new Exception("14");
                }
            }
        }
    }
}
