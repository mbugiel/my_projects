using ManagemateAPI.Database.Context;
using ManagemateAPI.Database.Tables;
using ManagemateAPI.Encryption;
using ManagemateAPI.Encryption.Input_Objects;
using ManagemateAPI.Information;
using ManagemateAPI.Management.M_Cities_List.Input_Objects;
using ManagemateAPI.Management.M_Cities_List.Table_Model;
using ManagemateAPI.Management.M_Session.Manager;

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

        public async Task<string> Add_Cities_List(Add_Cities_List_Data obj)
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
                                duplication_checker = true;
                            }
                        }
                    }

                    if (duplication_checker != false)
                    {
                        throw new Exception("18"); // Already exist
                    }
                    else
                    {
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

        public async Task<string> Edit_Cities_List(Edit_Cities_List_Data obj)
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

                    var edited_record = _context.Cities_List.Where(h => h.id == obj.id).FirstOrDefault();
                    if (edited_record != null)
                    {
                        var city_checker = _context.Cities_List.Where(i => i.city.Equals(obj.city)).FirstOrDefault();

                        if (city_checker != null)
                        {
                            throw new Exception("18"); // Already in use
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

        public async Task<string> Delete_Cities_List(Delete_Cities_List_Data obj)
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

                    var id_exits = _context.Cities_List.Where(i => i.id.Equals(obj.id)).FirstOrDefault();

                    if (id_exits == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }
                    else
                    {
                        _context.Cities_List.Remove(id_exits);
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

        public async Task<Cities_List_Model> Get_Cities_List_By_ID(Get_Cities_List_By_ID obj)
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

                    var selected_record = _context.Cities_List.Where(o => o.id.Equals(obj.id_to_get)).FirstOrDefault();

                    if (selected_record == null)
                    {
                        throw new Exception("19"); //Albo błąd w linijce z selected_record
                    }
                    else
                    {
                        List<Encrypted_Object> encrypted_fields = [
                            new Encrypted_Object { id = 0, encryptedValue = selected_record.city },
                        ];

                        List<Decrypted_Object> decrypted_fields = await Crypto.DecryptList(obj.session, encrypted_fields);

                        Cities_List_Model return_obj = new Cities_List_Model
                        {
                            id = selected_record.id,
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

        public async Task<List<Cities_List_Model_List>> Get_All_Cities_List(Get_All_Cities_List_Data obj)
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

                    List<Cities_List> records_list = _context.Cities_List.ToList();

                    if (records_list == null)
                    {
                        throw new Exception("19");
                    }
                    else
                    {

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

                        List<Decrypted_Object> cities_list_decrypted = await Crypto.DecryptList(obj.session, cities_list);

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
                }
                else
                {
                    throw new Exception("1");
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
