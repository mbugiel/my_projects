using ManagemateAPI.Database.Context;
using ManagemateAPI.Database.Tables;
using ManagemateAPI.Encryption;
using ManagemateAPI.Encryption.Input_Objects;
using ManagemateAPI.Helper.InputObjects.Client;
using ManagemateAPI.Information;
using ManagemateAPI.Management.M_Client.Input_Objects;
using ManagemateAPI.Management.M_Client.Table_Model;
using ManagemateAPI.Management.M_Session.Manager;
using Microsoft.EntityFrameworkCore;

/*
 * This is the Client_Manager with methods dedicated to the Client table.
 * 
 * It contains methods to:
 * add records,
 * edit records,
 * delete records,
 * get record by id,
 * get all the records.
 */

namespace ManagemateAPI.Management.M_Client.Manager
{
    public class Client_Manager
    {
        private DB_Context _context;
        private readonly IConfiguration _configuration;

        public Client_Manager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /* 
         * Add_Client method
         * This method is used to add new records to the Client table.
         * 
         * It accepts Add_Client_Data object as input.
         * It then adds new record with values based on the data given in the input object.
         */
        public async Task<string> Add_Client(Add_Client_Data obj)
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

                    List<Client> records_list = _context.Client.ToList();
                    List<Encrypted_Object> encrypted_column1 = new List<Encrypted_Object>();
                    List<Encrypted_Object> encrypted_column2 = new List<Encrypted_Object>();
                    bool duplication_checker1 = false;
                    bool duplication_checker2 = false;

                    if (records_list.Count == 0 || records_list == null)
                    {
                        duplication_checker1 = false;
                        duplication_checker2 = false;
                    }
                    else
                    {
                        foreach (var field in records_list)
                        {
                            encrypted_column1.Add(new Encrypted_Object { id = field.id, encryptedValue = field.phone_number });
                            encrypted_column2.Add(new Encrypted_Object { id = field.id, encryptedValue = field.email });
                        }

                        List<Decrypted_Object> decrypted_column1 = await Crypto.DecryptList(obj.session, encrypted_column1);
                        List<Decrypted_Object> decrypted_column2 = await Crypto.DecryptList(obj.session, encrypted_column2);

                        foreach (var field in decrypted_column1)
                        {
                            if (field.decryptedValue == obj.address)
                            {
                                duplication_checker1 = true;
                            }
                        }
                        foreach (var field in decrypted_column2)
                        {
                            if (field.decryptedValue == obj.postal_code)
                            {
                                duplication_checker2 = true;
                            }
                        }
                    }

                    if (duplication_checker1 != false && duplication_checker2 != false)
                    {
                        throw new Exception("18"); // Already in use
                    }
                    else
                    {
                        var city = _context.Cities_List.Where(c => c.id.Equals(obj.city_id_fk)).FirstOrDefault();

                        if (city == null)
                        {
                            throw new Exception("19"); //city not found
                        }
                        else
                        {

                            Client new_record = new Client
                            {
                                surname = await Crypto.Encrypt(obj.session, obj.surname),
                                name = await Crypto.Encrypt(obj.session, obj.name),
                                company_name = await Crypto.Encrypt(obj.session, obj.company_name),
                                NIP = await Crypto.Encrypt(obj.session, obj.nip),
                                phone_number = await Crypto.Encrypt(obj.session, obj.phone_number),
                                email = await Crypto.Encrypt(obj.session, obj.email),
                                address = await Crypto.Encrypt(obj.session, obj.address),
                                city_id_FK = city,
                                postal_code = await Crypto.Encrypt(obj.session, obj.postal_code),
                                comment = await Crypto.Encrypt(obj.session, obj.comment),
                            };
                            _context.Client.Add(new_record);
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
         * Edit_Client method
         * This method is used to edit a record in the Client table.
         * 
         * It accepts Edit_Client_Data object as input.
         * It then changes values of a record with those given in the input object only if its ID matches the one in the input object.
         */
        public async Task<string> Edit_Client(Edit_Client_Data obj)
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

                    var edited_record = _context.Client.Where(h => h.id == obj.id).FirstOrDefault();
                    if (edited_record != null)
                    {
                        var phone_number_checker = _context.Client.Where(i => i.phone_number.Equals(obj.phone_number)).FirstOrDefault();
                        var email_checker = _context.Client.Where(o => o.email.Equals(obj.email)).FirstOrDefault();

                        if (phone_number_checker != null || email_checker != null)
                        {
                            throw new Exception("18"); // Already in use
                        }
                        else
                        {

                            var city = _context.Cities_List.Where(c => c.id.Equals(obj.city_id_fk)).FirstOrDefault();

                            if (city == null)
                            {
                                throw new Exception("19"); //city not found
                            }
                            else
                            {

                                edited_record.id = obj.id;
                                edited_record.surname = await Crypto.Encrypt(obj.session, obj.surname);
                                edited_record.name = await Crypto.Encrypt(obj.session, obj.name);
                                edited_record.company_name = await Crypto.Encrypt(obj.session, obj.company_name);
                                edited_record.NIP = await Crypto.Encrypt(obj.session, obj.nip);
                                edited_record.phone_number = await Crypto.Encrypt(obj.session, obj.phone_number);
                                edited_record.email = await Crypto.Encrypt(obj.session, obj.email);
                                edited_record.address = await Crypto.Encrypt(obj.session, obj.address);
                                edited_record.city_id_FK = city;
                                edited_record.postal_code = await Crypto.Encrypt(obj.session, obj.postal_code);
                                edited_record.comment = await Crypto.Encrypt(obj.session, obj.comment);

                                _context.SaveChanges();

                                return Info.SUCCESSFULLY_ADDED;

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
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }
            }
        }

        /*
         * Delete_Client method
         * This method is used to a record from the Client table.
         *  
         * It accepts Delete_Client_Data object as input.
         * Then it deletes a record if its ID matches the one given in the input object.
         */
        public async Task<string> Delete_Client(Delete_Client_Data obj)
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

                    var id_exits = _context.Client.Where(i => i.id.Equals(obj.id)).FirstOrDefault();

                    if (id_exits == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }
                    else
                    {
                        _context.Client.Remove(id_exits);
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
         * Get_Client_By_ID method
         * This method gets a record from the Client table by its ID and returns it.
         * 
         * It accepts Get_Client_By_ID_Data object as input.
         * Then it gets a records that has the same ID as the ID given in the input object
         */
        public async Task<Client_Model> Get_Client_by_ID(Get_Client_By_ID obj)
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

                    var selected_record = _context.Client.Include(k => k.city_id_FK).Where(o => o.id.Equals(obj.id_to_get)).FirstOrDefault();

                    if (selected_record == null)
                    {
                        throw new Exception("19"); //Albo błąd w linijce z selected_record
                    }
                    else
                    {
                        List<Encrypted_Object> encrypted_items = [
                            new Encrypted_Object { id = 0, encryptedValue = selected_record.surname },
                            new Encrypted_Object { id = 1, encryptedValue = selected_record.name },
                            new Encrypted_Object { id = 2, encryptedValue = selected_record.phone_number },
                            new Encrypted_Object { id = 3, encryptedValue = selected_record.email },
                            new Encrypted_Object { id = 4, encryptedValue = selected_record.address },
                            new Encrypted_Object { id = 5, encryptedValue = selected_record.city_id_FK.city },
                            new Encrypted_Object { id = 6, encryptedValue = selected_record.postal_code },
                            new Encrypted_Object { id = 7, encryptedValue = selected_record.comment },
                            new Encrypted_Object { id = 8, encryptedValue = selected_record.company_name },
                            new Encrypted_Object { id = 9, encryptedValue = selected_record.NIP },
                        ];
                        List<Decrypted_Object> decrypted_items = await Crypto.DecryptList(obj.session, encrypted_items);

                        Client_Model return_obj = new Client_Model
                        {
                            id = selected_record.id
                        };

                        foreach (var item in decrypted_items)
                        {
                            if (item == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                switch (item.id)
                                {
                                    case 0:
                                        return_obj.surname = item.decryptedValue; break;
                                    case 1:
                                        return_obj.name = item.decryptedValue; break;
                                    case 2:
                                        return_obj.phone_number = item.decryptedValue; break;
                                    case 3:
                                        return_obj.email = item.decryptedValue; break;
                                    case 4:
                                        return_obj.address = item.decryptedValue; break;
                                    case 5:
                                        return_obj.city_id_FK = item.decryptedValue; break;
                                    case 6:
                                        return_obj.postal_code = item.decryptedValue; break;
                                    case 7:
                                        return_obj.comment = item.decryptedValue; break;
                                    case 8:
                                        return_obj.company_name = item.decryptedValue; break;
                                    case 9:
                                        return_obj.NIP = item.decryptedValue; break;
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
         * Get_All_Client method
         * This method gets all of the records in the Client table and returns them in a list.
         * 
         * It accepts Get_All_Client_Data object as input.
         */
        public async Task<List<Client_Model_List>> Get_All_Clients(Get_All_Clients_Data obj)
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

                    List<Client> records_list = _context.Client.Include(e => e.city_id_FK).ToList();

                    if (records_list == null)
                    {
                        throw new Exception("19");
                    }
                    else
                    {

                        List<Encrypted_Object> surname_list = new List<Encrypted_Object>();
                        List<Encrypted_Object> name_list = new List<Encrypted_Object>();
                        List<Encrypted_Object> company_name_list = new List<Encrypted_Object>();
                        List<Encrypted_Object> NIP_list = new List<Encrypted_Object>();
                        List<Encrypted_Object> phone_number_list = new List<Encrypted_Object>();
                        List<Encrypted_Object> email_list = new List<Encrypted_Object>();
                        List<Encrypted_Object> address_list = new List<Encrypted_Object>();
                        List<Encrypted_Object> city_list = new List<Encrypted_Object>();
                        List<Encrypted_Object> postal_code_list = new List<Encrypted_Object>();
                        List<Encrypted_Object> comment_list = new List<Encrypted_Object>();

                        List<Client_Model_List> decrypted_records = new List<Client_Model_List>();

                        foreach (var item in records_list)
                        {
                            decrypted_records.Add(new Client_Model_List
                            {
                                id = item.id,
                            });

                            surname_list.Add(new Encrypted_Object { id = item.id, encryptedValue = item.surname });
                            name_list.Add(new Encrypted_Object { id = item.id, encryptedValue = item.name });
                            company_name_list.Add(new Encrypted_Object { id = item.id, encryptedValue = item.company_name });
                            NIP_list.Add(new Encrypted_Object { id = item.id, encryptedValue = item.NIP });
                            phone_number_list.Add(new Encrypted_Object { id = item.id, encryptedValue = item.phone_number });
                            email_list.Add(new Encrypted_Object { id = item.id, encryptedValue = item.email });
                            address_list.Add(new Encrypted_Object { id = item.id, encryptedValue = item.address });
                            city_list.Add(new Encrypted_Object { id = item.id, encryptedValue = item.city_id_FK.city });
                            postal_code_list.Add(new Encrypted_Object { id = item.id, encryptedValue = item.postal_code });
                            comment_list.Add(new Encrypted_Object { id = item.id, encryptedValue = item.comment });
                        }

                        List<Decrypted_Object> surname_list_decrypted = await Crypto.DecryptList(obj.session, surname_list);
                        List<Decrypted_Object> name_list_decrypted = await Crypto.DecryptList(obj.session, name_list);
                        List<Decrypted_Object> company_name_list_decrypted = await Crypto.DecryptList(obj.session, company_name_list);
                        List<Decrypted_Object> NIP_list_decrypted = await Crypto.DecryptList(obj.session, NIP_list);
                        List<Decrypted_Object> phone_number_list_decrypted = await Crypto.DecryptList(obj.session, phone_number_list);
                        List<Decrypted_Object> email_list_decrypted = await Crypto.DecryptList(obj.session, email_list);
                        List<Decrypted_Object> address_list_decrypted = await Crypto.DecryptList(obj.session, address_list);
                        List<Decrypted_Object> city_list_decrypted = await Crypto.DecryptList(obj.session, city_list);
                        List<Decrypted_Object> postal_code_list_decrypted = await Crypto.DecryptList(obj.session, postal_code_list);
                        List<Decrypted_Object> comment_list_decrypted = await Crypto.DecryptList(obj.session, comment_list);

                        foreach (var item in decrypted_records)
                        {
                            var surname = surname_list_decrypted.Where(k => k.id.Equals(item.id)).FirstOrDefault();
                            if (surname == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                item.surname = surname.decryptedValue;
                            }

                            var name = name_list_decrypted.Where(k => k.id.Equals(item.id)).FirstOrDefault();
                            if (name == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                item.name = name.decryptedValue;
                            }

                            var company_name = company_name_list_decrypted.Where(k => k.id.Equals(item.id)).FirstOrDefault();
                            if (company_name == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                item.company_name = company_name.decryptedValue;
                            }

                            var NIP = NIP_list_decrypted.Where(k => k.id.Equals(item.id)).FirstOrDefault();
                            if (NIP == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                item.NIP = NIP.decryptedValue;
                            }

                            var phone_number = phone_number_list_decrypted.Where(k => k.id.Equals(item.id)).FirstOrDefault();
                            if (NIP == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                item.phone_number = phone_number.decryptedValue;
                            }

                            var email = email_list_decrypted.Where(k => k.id.Equals(item.id)).FirstOrDefault();
                            if (email == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                item.email = email.decryptedValue;
                            }

                            var address = address_list_decrypted.Where(k => k.id.Equals(item.id)).FirstOrDefault();
                            if (address == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                item.address = address.decryptedValue;
                            }

                            var city_id_FK = city_list_decrypted.Where(k => k.id.Equals(item.id)).FirstOrDefault();
                            if (city_id_FK == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                item.city_id_FK = city_id_FK.decryptedValue;
                            }

                            var postal_code = postal_code_list_decrypted.Where(k => k.id.Equals(item.id)).FirstOrDefault();
                            if (postal_code == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                item.postal_code = postal_code.decryptedValue;
                            }

                            var comment = comment_list_decrypted.Where(k => k.id.Equals(item.id)).FirstOrDefault();
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
        //Get by page | Nieużywane
        public async Task<List<Client_Model_List>> Get_Client_Page(Get_Client_Page obj)
        {
            if (obj == null)
            {
                throw new Exception("14");
            }
            else
            {
                if(await Session_Checker.ActiveSession(obj.session))
                {
                    _context = new DB_Context(obj.session.userId, _configuration);
                    _context.Database.EnsureCreated();

                    List<Client> records_list = _context.Client.Include(e => e.city_id_FK).ToList();

                    if(records_list == null)
                    {
                        throw new Exception("19");
                    }
                    else
                    {
                        int page_lenght = obj.page_Size;
                        int start_position = obj.page_ID * page_lenght;

                        if(records_list.Count() > start_position)
                        {
                            if(records_list.Count() >= start_position + page_lenght)
                            {
                                records_list = records_list.Slice(start_position, page_lenght);
                            }
                            else
                            {
                                int valid_lenght = records_list.Count() - start_position;
                                records_list = records_list.Slice(start_position, valid_lenght);
                            }

                            List<Encrypted_Object> surname_list = new List<Encrypted_Object>();
                            List<Encrypted_Object> name_list = new List<Encrypted_Object>();
                            List<Encrypted_Object> company_name_list = new List<Encrypted_Object>();
                            List<Encrypted_Object> NIP_list = new List<Encrypted_Object>();
                            List<Encrypted_Object> phone_number_list = new List<Encrypted_Object>();
                            List<Encrypted_Object> email_list = new List<Encrypted_Object>();
                            List<Encrypted_Object> address_list = new List<Encrypted_Object>();
                            List<Encrypted_Object> city_list = new List<Encrypted_Object>();
                            List<Encrypted_Object> postal_code_list = new List<Encrypted_Object>();
                            List<Encrypted_Object> comment_list = new List<Encrypted_Object>();

                            List<Client_Model_List> decrypted_records = new List<Client_Model_List>();

                            foreach(var item in records_list)
                            {
                                decrypted_records.Add(new Client_Model_List
                                {
                                    id = item.id,
                                });

                                surname_list.Add(new Encrypted_Object { id = item.id, encryptedValue = item.surname});
                                name_list.Add(new Encrypted_Object { id = item.id, encryptedValue = item.name });
                                company_name_list.Add(new Encrypted_Object { id = item.id, encryptedValue = item.company_name});
                                NIP_list.Add(new Encrypted_Object { id = item.id, encryptedValue = item.NIP });
                                phone_number_list.Add(new Encrypted_Object { id = item.id, encryptedValue = item.phone_number});
                                email_list.Add(new Encrypted_Object { id = item.id, encryptedValue = item.email });
                                address_list.Add(new Encrypted_Object { id = item.id, encryptedValue = item.address });
                                city_list.Add(new Encrypted_Object { id = item.id, encryptedValue = item.city_id_FK.city });
                                postal_code_list.Add(new Encrypted_Object { id = item.id, encryptedValue = item.postal_code});
                                comment_list.Add(new Encrypted_Object { id = item.id, encryptedValue = item.comment });
                            }

                            List<Decrypted_Object> surname_list_decrypted = await Crypto.DecryptList(obj.session, surname_list);
                            List<Decrypted_Object> name_list_decrypted = await Crypto.DecryptList(obj.session, name_list);
                            List<Decrypted_Object> company_name_list_decrypted = await Crypto.DecryptList(obj.session, company_name_list);
                            List<Decrypted_Object> NIP_list_decrypted = await Crypto.DecryptList(obj.session, NIP_list);
                            List<Decrypted_Object> phone_number_list_decrypted = await Crypto.DecryptList(obj.session, phone_number_list);
                            List<Decrypted_Object> email_list_decrypted = await Crypto.DecryptList(obj.session, email_list);
                            List<Decrypted_Object> address_list_decrypted = await Crypto.DecryptList(obj.session, address_list);
                            List<Decrypted_Object> city_list_decrypted = await Crypto.DecryptList(obj.session, city_list);
                            List<Decrypted_Object> postal_code_list_decrypted = await Crypto.DecryptList(obj.session, postal_code_list);
                            List<Decrypted_Object> comment_list_decrypted = await Crypto.DecryptList(obj.session, comment_list);

                            foreach (var item in decrypted_records)
                            {
                                var surname = surname_list_decrypted.Where(k => k.id.Equals(item.id)).FirstOrDefault();
                                if ( surname == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.surname = surname.decryptedValue;
                                }

                                var name = name_list_decrypted.Where(k => k.id.Equals(item.id)).FirstOrDefault();
                                if (name == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.name = name.decryptedValue;
                                }

                                var company_name = company_name_list_decrypted.Where(k => k.id.Equals(item.id)).FirstOrDefault();
                                if (company_name == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.company_name = company_name.decryptedValue;
                                }

                                var NIP = NIP_list_decrypted.Where(k => k.id.Equals(item.id)).FirstOrDefault();
                                if (NIP == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.NIP = NIP.decryptedValue;
                                }

                                var phone_number = NIP_list_decrypted.Where(k => k.id.Equals(item.id)).FirstOrDefault();
                                if (NIP == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.phone_number = phone_number.decryptedValue;
                                }

                                var email = email_list_decrypted.Where(k => k.id.Equals(item.id)).FirstOrDefault();
                                if (email == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.email = email.decryptedValue;
                                }

                                var address = address_list_decrypted.Where(k => k.id.Equals(item.id)).FirstOrDefault();
                                if (address == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.address = address.decryptedValue;
                                }

                                var city_id_FK = city_list_decrypted.Where(k => k.id.Equals(item.id)).FirstOrDefault();
                                if (city_id_FK == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.city_id_FK = city_id_FK.decryptedValue;
                                }

                                var postal_code = postal_code_list_decrypted.Where(k => k.id.Equals(item.id)).FirstOrDefault();
                                if (postal_code == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.postal_code = postal_code.decryptedValue;
                                }

                                var comment = comment_list_decrypted.Where(k => k.id.Equals(item.id)).FirstOrDefault();
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
                    throw new Exception("1");
                }
            }
        }
    }
}
