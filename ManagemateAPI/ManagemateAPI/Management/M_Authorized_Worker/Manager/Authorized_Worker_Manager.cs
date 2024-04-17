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

        public async Task<string> Add_Authorized_Worker(Add_Authorized_Worker_Data obj)
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

                    List<Authorized_Worker> records_list = _context.Authorized_Worker.ToList();
                    List<Encrypted_Object> encrypted_phoneNumbers = new List<Encrypted_Object>();
                    List<Encrypted_Object> encrypted_emails = new List<Encrypted_Object>();
                    bool phone_number_checker = false;
                    bool email_checker = false;

                    if (records_list.Count == 0 || records_list == null)
                    {
                        phone_number_checker = false;
                        email_checker = false;
                    }
                    else
                    {
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

                    //var phone_number_checker = _context.Authorized_Worker.Where(i => i.phone_number.Equals(Crypto.Encrypt(obj.session, obj.phone_number))).FirstOrDefault();
                    //var email_checker = _context.Authorized_Worker.Where(o => o.email.Equals(Crypto.Encrypt(obj.session, obj.email))).FirstOrDefault();

                    if (phone_number_checker == false && email_checker == false)
                    {
                        var client = _context.Client.Where(c => c.id.Equals(obj.client_id_FK)).FirstOrDefault();

                        if (client == null)
                        {
                            throw new Exception("19");// client not found
                        }
                        else
                        {
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
                            _context.Authorized_Worker.Add(new_record);
                            _context.SaveChanges();

                            return Info.SUCCESSFULLY_ADDED;
                        }
                    }
                    else
                    {
                        throw new Exception("18"); // Already exist
                    }
                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }

            }

        }

        //EDIT
        public async Task<string> Edit_Authorized_Worker(Edit_Authorized_Worker_Data obj)
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

                    var edited_record = _context.Authorized_Worker.Where(h => h.id == obj.id).FirstOrDefault();
                    if (edited_record != null)
                    {
                        List<Authorized_Worker> records_list = _context.Authorized_Worker.ToList();
                        List<Encrypted_Object> encrypted_phoneNumbers = new List<Encrypted_Object>();
                        List<Encrypted_Object> encrypted_emails = new List<Encrypted_Object>();
                        bool phone_number_checker = false;
                        bool email_checker = false;

                        if (records_list.Count == 0 || records_list == null)
                        {
                            phone_number_checker = false;
                            email_checker = false;
                        }
                        else
                        {
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

                        if (phone_number_checker != false && email_checker != false)
                        {
                            throw new Exception("18"); // Already in use
                        }
                        else
                        {

                            var client = _context.Client.Where(c => c.id.Equals(obj.client_id_FK)).FirstOrDefault();

                            if (client == null)
                            {
                                throw new Exception("19");// client not found
                            }
                            else
                            {

                                edited_record.id = obj.id;
                                edited_record.client_id_FK = client;
                                edited_record.name = await Crypto.Encrypt(obj.session, obj.name);
                                edited_record.surname = await Crypto.Encrypt(obj.session, obj.surname);
                                edited_record.phone_number = await Crypto.Encrypt(obj.session, obj.phone_number);
                                edited_record.email = await Crypto.Encrypt(obj.session, obj.email);
                                edited_record.contact = obj.contact;
                                edited_record.collection = obj.collection;
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

        //Delete
        public async Task<string> Delete_Authorized_Worker(Delete_Authorized_Worker_Data obj)
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

                    var id_exits = _context.Authorized_Worker.Where(i => i.id.Equals(obj.id)).FirstOrDefault();

                    if (id_exits == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }
                    else
                    {
                        _context.Authorized_Worker.Remove(id_exits);
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
        public async Task<Authorized_Worker_Model> Get_Authorized_Worker_By_ID(Get_Authorized_Worker_By_ID_Data obj)
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

                    var selected_record = _context.Authorized_Worker.Include(j => j.client_id_FK).Where(o => o.id.Equals(obj.id_to_get)).FirstOrDefault();

                    if (selected_record == null)
                    {
                        throw new Exception("19"); //Albo błąd w linijce z selected_record
                    }
                    else
                    {
                        List<Encrypted_Object> encrypted_items = [
                            //Z tabeli Authorized_Worker
                            new Encrypted_Object { id = 0, encryptedValue = selected_record.name },
                            new Encrypted_Object { id = 1, encryptedValue = selected_record.surname },
                            new Encrypted_Object { id = 2, encryptedValue = selected_record.phone_number },
                            new Encrypted_Object { id = 3, encryptedValue = selected_record.email },
                            new Encrypted_Object { id = 4, encryptedValue = selected_record.comment },
                            //Z tabeli Client
                            new Encrypted_Object { id = 5, encryptedValue = selected_record.client_id_FK.surname },
                            new Encrypted_Object { id = 6, encryptedValue = selected_record.client_id_FK.name },
                            new Encrypted_Object { id = 7, encryptedValue = selected_record.client_id_FK.company_name },
                            new Encrypted_Object { id = 8, encryptedValue = selected_record.client_id_FK.NIP },
                            new Encrypted_Object { id = 9, encryptedValue = selected_record.client_id_FK.phone_number },
                            new Encrypted_Object { id = 10, encryptedValue = selected_record.client_id_FK.email },
                            new Encrypted_Object { id = 11, encryptedValue = selected_record.client_id_FK.address },
                            new Encrypted_Object { id = 12, encryptedValue = selected_record.client_id_FK.city_id_FK.city },
                            new Encrypted_Object { id = 13, encryptedValue = selected_record.client_id_FK.postal_code },
                            new Encrypted_Object { id = 14, encryptedValue = selected_record.client_id_FK.comment },
                        ];

                        List<Decrypted_Object> decrypted_items = await Crypto.DecryptList(obj.session, encrypted_items);

                        Authorized_Worker_Model return_obj = new Authorized_Worker_Model
                        {
                            id = selected_record.id,
                            contact = selected_record.contact,
                            collection = selected_record.collection,

                            client_id_FK = new Client_Model { id = selected_record.client_id_FK.id },
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
                                        return_obj.name = item.decryptedValue; break;
                                    case 1:
                                        return_obj.surname = item.decryptedValue; break;
                                    case 2:
                                        return_obj.phone_number = item.decryptedValue; break;
                                    case 3:
                                        return_obj.email = item.decryptedValue; break;
                                    case 4:
                                        return_obj.comment = item.decryptedValue; break;
                                    case 5:
                                        return_obj.client_id_FK.surname = item.decryptedValue; break;

                                    case 6:
                                        return_obj.client_id_FK.name = item.decryptedValue; break;

                                    case 7:
                                        return_obj.client_id_FK.company_name = item.decryptedValue; break;

                                    case 8:
                                        return_obj.client_id_FK.NIP = item.decryptedValue; break;

                                    case 9:
                                        return_obj.client_id_FK.phone_number = item.decryptedValue; break;

                                    case 10:
                                        return_obj.client_id_FK.email = item.decryptedValue; break;

                                    case 11:
                                        return_obj.client_id_FK.address = item.decryptedValue; break;

                                    case 12:
                                        return_obj.client_id_FK.city_id_FK = item.decryptedValue; break;

                                    case 13:
                                        return_obj.client_id_FK.postal_code = item.decryptedValue; break;

                                    case 14:
                                        return_obj.client_id_FK.comment = item.decryptedValue; break;
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

        //Get all
        public async Task<List<Authorized_Worker_Model_List>> Get_All_Authorized_Worker(Get_All_Authorized_Worker_Data obj)
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

                    List<Authorized_Worker> records_list = _context.Authorized_Worker.Include(j => j.client_id_FK).ToList();

                    if (records_list == null)
                    {
                        throw new Exception("19");
                    }
                    else
                    {
                        List<Encrypted_Object> name_encrypted = new List<Encrypted_Object>();
                        List<Encrypted_Object> surname_encrypted = new List<Encrypted_Object>();
                        List<Encrypted_Object> phone_number_encrypted = new List<Encrypted_Object>();
                        List<Encrypted_Object> email_encrypted = new List<Encrypted_Object>();
                        List<Encrypted_Object> comment_encrypted = new List<Encrypted_Object>();
                        List<Encrypted_Object> client_name_encrypted = new List<Encrypted_Object>();

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
                            client_name_encrypted.Add(new Encrypted_Object { id = item.id, encryptedValue = item.client_id_FK.name });
                        }

                        List<Decrypted_Object> name_decrypted = await Crypto.DecryptList(obj.session, name_encrypted);
                        List<Decrypted_Object> surname_decrypted = await Crypto.DecryptList(obj.session, surname_encrypted);
                        List<Decrypted_Object> phone_number_decrypted = await Crypto.DecryptList(obj.session, phone_number_encrypted);
                        List<Decrypted_Object> email_decrypted = await Crypto.DecryptList(obj.session, email_encrypted);
                        List<Decrypted_Object> comment_decrypted = await Crypto.DecryptList(obj.session, comment_encrypted);
                        List<Decrypted_Object> client_name_decrypted = await Crypto.DecryptList(obj.session, client_name_encrypted);

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

                            var client_name = client_name_decrypted.Where(o => o.id.Equals(item.id)).FirstOrDefault();
                            if (client_name == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                item.client_name = client_name.decryptedValue;
                            }
                        }
                        return decrypted_records;
                    }
                }
                else
                {
                    throw new Exception("14");
                }
            }
        }

        //Get by page | NOT_IN_USE
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

                    List<Authorized_Worker> records_list = _context.Authorized_Worker.Include(j => j.client_id_FK).ToList();

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
                            List<Encrypted_Object> client_name_encrypted = new List<Encrypted_Object>();

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
                                client_name_encrypted.Add(new Encrypted_Object { id = item.id, encryptedValue = item.client_id_FK.name });
                            }

                            List<Decrypted_Object> name_decrypted = await Crypto.DecryptList(obj.session, name_encrypted);
                            List<Decrypted_Object> surname_decrypted = await Crypto.DecryptList(obj.session, surname_encrypted);
                            List<Decrypted_Object> phone_number_decrypted = await Crypto.DecryptList(obj.session, phone_number_encrypted);
                            List<Decrypted_Object> email_decrypted = await Crypto.DecryptList(obj.session, email_encrypted);
                            List<Decrypted_Object> comment_decrypted = await Crypto.DecryptList(obj.session, comment_encrypted);
                            List<Decrypted_Object> client_name_decrypted = await Crypto.DecryptList(obj.session, client_name_encrypted);

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

                                var client_name = client_name_decrypted.Where(o => o.id.Equals(item.id)).FirstOrDefault();
                                if (client_name == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.client_name = client_name.decryptedValue;
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
