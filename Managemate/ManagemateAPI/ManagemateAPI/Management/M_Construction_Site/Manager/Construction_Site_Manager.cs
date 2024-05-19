using ManagemateAPI.Database.Context;
using ManagemateAPI.Database.Tables;
using ManagemateAPI.Encryption;
using ManagemateAPI.Encryption.Input_Objects;
using ManagemateAPI.Information;
using ManagemateAPI.Management.M_Construction_Site.Input_Objects;
using ManagemateAPI.Management.M_Construction_Site.Table_Model;
using ManagemateAPI.Management.M_Session.Manager;
using Microsoft.EntityFrameworkCore;

/*
 * This is the Construction_Site_Manager with methods dedicated to the Construction_Site table.
 * 
 * It contains methods to:
 * add records,
 * edit records,
 * delete records,
 * get record by id,
 * get all the records.
 */
namespace ManagemateAPI.Management.M_Construction_Site.Manager
{
    public class Construction_Site_Manager
    {
        private DB_Context _context;
        private readonly IConfiguration _configuration;

        public Construction_Site_Manager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /* 
         * Add_Construction_Site method
         * This method is used to add new records to the Construction_Site table.
         * 
         * It accepts Add_Construction_Site_Data object as input.
         * It then adds new record with values based on the data given in the input object.
         */
        public async Task<string> Add_Construction_Site(Add_Construction_Site_Data obj)
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

                    List<Construction_Site> records_list = _context.Construction_Site.ToList();
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
                            encrypted_column1.Add(new Encrypted_Object { id = field.id, encryptedValue = field.address });
                            encrypted_column2.Add(new Encrypted_Object { id = field.id, encryptedValue = field.postal_code });
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
                        throw new Exception("18"); // Already exists
                    }
                    else
                    {

                        var city = _context.Cities_List.Where(c => c.id.Equals(obj.cities_list_id_fk)).FirstOrDefault();

                        if (city == null)
                        {
                            throw new Exception("19"); //city not found
                        }
                        else
                        {

                            Construction_Site new_record = new Construction_Site
                            {
                                construction_site_name = await Crypto.Encrypt(obj.session, obj.construction_site_name),
                                address = await Crypto.Encrypt(obj.session, obj.address),
                                cities_list_id_FK = city,
                                postal_code = await Crypto.Encrypt(obj.session, obj.postal_code),
                                comment = await Crypto.Encrypt(obj.session, obj.comment),
                            };
                            _context.Construction_Site.Add(new_record);
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
         * Edit_Construction_Site method
         * This method is used to edit a record in the Construction_Site table.
         * 
         * It accepts Edit_Construction_Site_Data object as input.
         * It then changes values of a record with those given in the input object only if its ID matches the one in the input object.
         */
        public async Task<string> Edit_Construction_Site(Edit_Construction_Site_Data obj)
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

                    var edited_record = _context.Construction_Site.Where(h => h.id == obj.id).FirstOrDefault();
                    if (edited_record != null)
                    {
                        var address_checker = _context.Construction_Site.Where(i => i.address.Equals(obj.address)).FirstOrDefault();
                        var postal_code_checker = _context.Construction_Site.Where(o => o.postal_code.Equals(obj.postal_code)).FirstOrDefault();

                        if (address_checker != null || postal_code_checker != null)
                        {
                            throw new Exception("18"); // Already in use
                        }
                        else
                        {

                            var city = _context.Cities_List.Where(c => c.id.Equals(obj.cities_list_id_fk)).FirstOrDefault();

                            if (city == null)
                            {
                                throw new Exception("19"); //city not found
                            }
                            else
                            {

                                edited_record.id = obj.id;
                                edited_record.construction_site_name = await Crypto.Encrypt(obj.session, obj.construction_site_name);
                                edited_record.address = await Crypto.Encrypt(obj.session, obj.address);
                                edited_record.cities_list_id_FK = city;
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
         * Delete_Construction_Site method
         * This method is used to a record from the Construction_Site table.
         *  
         * It accepts Delete_Construction_Site_Data object as input.
         * Then it deletes a record if its ID matches the one given in the input object.
         */
        public async Task<string> Delete_Construction_Site(Delete_Construction_Site_Data obj)
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

                    var id_exits = _context.Construction_Site.Where(i => i.id.Equals(obj.id)).FirstOrDefault();

                    if (id_exits == null)
                    {
                        throw new Exception("14");//_14_NULL_ERROR
                    }
                    else
                    {
                        _context.Construction_Site.Remove(id_exits);
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
         * Get_Construction_Site_By_ID method
         * This method gets a record from the Construction_Site table by its ID and returns it.
         * 
         * It accepts Get_Construction_Site_By_ID_Data object as input.
         * Then it gets a records that has the same ID as the ID given in the input object
         */
        public async Task<Construction_Site_Model> Get_Construction_Site_By_ID(Get_Construction_Site_By_ID obj)
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

                    var selected_record = _context.Construction_Site.Include(e => e.cities_list_id_FK).Where(r => r.id.Equals(obj.id_to_get)).FirstOrDefault();

                    if (selected_record == null)
                    {
                        throw new Exception("19"); //Albo błąd w linijce z selected_record
                    }
                    else
                    {
                        List<Encrypted_Object> encrypted_fields = [
                            new Encrypted_Object { id = 0, encryptedValue = selected_record.construction_site_name },
                            new Encrypted_Object { id = 1, encryptedValue = selected_record.address },
                            new Encrypted_Object { id = 2, encryptedValue = selected_record.cities_list_id_FK.city },
                            new Encrypted_Object { id = 3, encryptedValue = selected_record.postal_code },
                            new Encrypted_Object { id = 4, encryptedValue = selected_record.comment }
                        ];

                        List<Decrypted_Object> decrypted_fields = await Crypto.DecryptList(obj.session, encrypted_fields);

                        Construction_Site_Model return_obj = new Construction_Site_Model
                        {
                            id = selected_record.id
                        };

                        foreach (var field in decrypted_fields)
                        {
                            if(field == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                switch(field.id)
                                {
                                    case 0:
                                        return_obj.construction_site_name = field.decryptedValue; break;
                                    case 1:
                                        return_obj.address = field.decryptedValue; break;
                                    case 2:
                                        return_obj.cities_list_id_FK = field.decryptedValue; break;
                                    case 3:
                                        return_obj.postal_code = field.decryptedValue; break;
                                    case 4:
                                        return_obj.comment = field.decryptedValue; break;
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
         * Get_All_Construction_Site method
         * This method gets all of the records in the Construction_Site table and returns them in a list.
         * 
         * It accepts Get_All_Construction_Site_Data object as input.
         */
        public async Task<List<Construction_Site_Model_List>> Get_All_Construction_Site(Get_All_Construction_Site_Data obj)
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

                    List<Construction_Site> record_list = _context.Construction_Site.Include(e => e.cities_list_id_FK).ToList();

                    if (record_list == null)
                    {
                        throw new Exception("19");
                    }
                    else
                    {
                        

                        List<Encrypted_Object> construction_site_name_encrypted = new List<Encrypted_Object>();
                        List<Encrypted_Object> address_encrypted = new List<Encrypted_Object>();
                        List<Encrypted_Object> city_encrypted = new List<Encrypted_Object>();
                        List<Encrypted_Object> postal_code_encrypted = new List<Encrypted_Object>();
                        List<Encrypted_Object> comment_encrypted = new List<Encrypted_Object>();

                        List<Construction_Site_Model_List> decrypted_records = new List<Construction_Site_Model_List>();

                        foreach (var item in record_list)
                        {
                            decrypted_records.Add(new Construction_Site_Model_List
                            {
                                id = item.id,
                            });

                            construction_site_name_encrypted.Add(new Encrypted_Object { id = item.id, encryptedValue = item.construction_site_name });
                            address_encrypted.Add(new Encrypted_Object { id = item.id, encryptedValue = item.address });
                            city_encrypted.Add(new Encrypted_Object { id = item.id, encryptedValue = item.cities_list_id_FK.city });
                            postal_code_encrypted.Add(new Encrypted_Object { id = item.id, encryptedValue = item.postal_code });
                            comment_encrypted.Add(new Encrypted_Object { id = item.id, encryptedValue = item.comment });
                        }

                        List<Decrypted_Object> decrypted_construction_site = await Crypto.DecryptList(obj.session, construction_site_name_encrypted);
                        List<Decrypted_Object> decrypted_address = await Crypto.DecryptList(obj.session, address_encrypted);
                        List<Decrypted_Object> decrypted_city = await Crypto.DecryptList(obj.session, city_encrypted);
                        List<Decrypted_Object> decrypted_postal_code = await Crypto.DecryptList(obj.session, postal_code_encrypted);
                        List<Decrypted_Object> decrypted_comment = await Crypto.DecryptList(obj.session, comment_encrypted);

                        foreach (var item in decrypted_records)
                        {
                            var construction_site_name = decrypted_construction_site.Where(s => s.id.Equals(item.id)).FirstOrDefault();
                            if (construction_site_name == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                item.construction_site_name = construction_site_name.decryptedValue;
                            }

                            var address = decrypted_address.Where(s => s.id.Equals(item.id)).FirstOrDefault();
                            if (address == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                item.address = address.decryptedValue;
                            }

                            var cities_list_id_FK = decrypted_city.Where(s => s.id.Equals(item.id)).FirstOrDefault();
                            if (cities_list_id_FK == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                item.cities_list_id_FK = cities_list_id_FK.decryptedValue;
                            }

                            var postal_code = decrypted_postal_code.Where(s => s.id.Equals(item.id)).FirstOrDefault();
                            if (postal_code == null)
                            {
                                throw new Exception("3");
                            }
                            else
                            {
                                item.postal_code = postal_code.decryptedValue;
                            }

                            var comment = decrypted_comment.Where(s => s.id.Equals(item.id)).FirstOrDefault();
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

        //Get by page
        /*
        public async Task<List<Construction_Site_Model_List>> Get_Construction_Site_By_Page(Get_Construction_Site_By_Page obj)
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

                    List<Construction_Site> record_list = _context.Construction_Site.Include(e => e.cities_list_id_FK).ToList();

                    if (record_list == null)
                    {
                        throw new Exception("19");
                    }
                    else
                    {
                        int page_lenght = obj.page_Size;
                        int start_position = obj.page_ID * page_lenght;

                        if (record_list.Count() > start_position)
                        {
                            if (record_list.Count() >= start_position + page_lenght)
                            {
                                record_list = record_list.Slice(start_position, page_lenght);
                            }
                            else
                            {
                                int valid_lenght = record_list.Count() - start_position;

                                record_list = record_list.Slice(start_position, valid_lenght);
                            }

                            List<Encrypted_Object> construction_site_name_encrypted = new List<Encrypted_Object>();
                            List<Encrypted_Object> address_encrypted = new List<Encrypted_Object>();
                            List<Encrypted_Object> city_encrypted = new List<Encrypted_Object>();
                            List<Encrypted_Object> postal_code_encrypted = new List<Encrypted_Object>();
                            List<Encrypted_Object> comment_encrypted = new List<Encrypted_Object>();

                            List<Construction_Site_Model_List> decrypted_records = new List<Construction_Site_Model_List>();

                            foreach(var item in record_list)
                            {
                                decrypted_records.Add(new Construction_Site_Model_List
                                {
                                    id = item.id,
                                });

                                construction_site_name_encrypted.Add(new Encrypted_Object { id = item.id, encryptedValue = item.construction_site_name});
                                address_encrypted.Add(new Encrypted_Object { id = item.id, encryptedValue = item.address });
                                city_encrypted.Add(new Encrypted_Object { id = item.id, encryptedValue = item.cities_list_id_FK.city});
                                postal_code_encrypted.Add(new Encrypted_Object { id = item.id, encryptedValue = item.postal_code});
                                comment_encrypted.Add(new Encrypted_Object { id = item.id, encryptedValue = item.comment});
                            }

                            List<Decrypted_Object> decrypted_construction_site = await Crypto.DecryptList(obj.session, construction_site_name_encrypted);
                            List<Decrypted_Object> decrypted_address = await Crypto.DecryptList(obj.session, address_encrypted);
                            List<Decrypted_Object> decrypted_city = await Crypto.DecryptList(obj.session, city_encrypted);
                            List<Decrypted_Object> decrypted_postal_code = await Crypto.DecryptList(obj.session, postal_code_encrypted);
                            List<Decrypted_Object> decrypted_comment = await Crypto.DecryptList(obj.session, comment_encrypted);

                            foreach(var item in decrypted_records)
                            {
                                var construction_site_name = decrypted_construction_site.Where(s => s.id.Equals(item.id)).FirstOrDefault();
                                if (construction_site_name == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.construction_site_name = construction_site_name.decryptedValue;
                                }

                                var address = decrypted_address.Where(s => s.id.Equals(item.id)).FirstOrDefault();
                                if (address == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.address = address.decryptedValue;
                                }

                                var cities_list_id_FK = decrypted_city.Where(s => s.id.Equals(item.id)).FirstOrDefault();
                                if (cities_list_id_FK == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.cities_list_id_FK = cities_list_id_FK.decryptedValue;
                                }

                                var postal_code = decrypted_postal_code.Where(s => s.id.Equals(item.id)).FirstOrDefault();
                                if (postal_code == null)
                                {
                                    throw new Exception("3");
                                }
                                else
                                {
                                    item.postal_code = postal_code.decryptedValue;
                                }

                                var comment = decrypted_comment.Where(s => s.id.Equals(item.id)).FirstOrDefault();
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
        */
    }
}
