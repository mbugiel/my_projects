using ManagemateAPI.Database.Context;
using ManagemateAPI.Database.Tables;
using ManagemateAPI.Encryption;
using ManagemateAPI.Encryption.Input_Objects;
using ManagemateAPI.Information;
using ManagemateAPI.Management.M_Item_Type.Input_Objects;
using ManagemateAPI.Management.M_Item_Type.Table_Model;
using ManagemateAPI.Management.M_Session.Manager;

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


        public async Task<string> AddItemType(Add_Item_Type_Data obj)
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

                    var item_type_checker = _context.Item_Type.Where(i => i.item_type.Equals(encrypted_type)).FirstOrDefault();

                    if (item_type_checker != null)
                    {
                        throw new Exception("18"); // Already exist
                    }
                    else
                    {
                        Item_Type new_record = new Item_Type
                        {
                            item_type = encrypted_type
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

        //EDIT
        public async Task<string> EditItemType(Edit_Item_Type_Data obj)
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

                        var item_type_checker = _context.Item_Type.Where(i => i.item_type.Equals(encrypted_type)).FirstOrDefault();

                        if (item_type_checker != null)
                        {
                            throw new Exception("18"); // Already in use
                        }
                        else
                        {

                            edited_record.item_type = encrypted_type;

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

        //Delete
        public async Task<string> DeleteItemType(Delete_Item_Type_Data obj)
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

                    var id_exits = _context.Item_Type.Where(i => i.id.Equals(obj.item_type_id)).FirstOrDefault();

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


        public async Task<List<Item_Type_Model>> GetItemTypes(Get_Item_Types_Data obj)
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

                    var item_types = _context.Item_Type.ToList();

                    if (item_types == null)
                    {
                        throw new Exception("19");// item types not found
                    }
                    else
                    {
                        List<Item_Type_Model> item_types_model = new List<Item_Type_Model>();

                        List<Encrypted_Object> encrypted_types = new List<Encrypted_Object>();

                        foreach (var type in item_types)
                        {
                            encrypted_types.Add(new Encrypted_Object { id = type.id, encryptedValue = type.item_type });
                        }


                        List<Decrypted_Object> decrypted_types = await Crypto.DecryptList(obj.session, encrypted_types);

                        if (decrypted_types == null)
                        {
                            throw new Exception("3"); // decryption error
                        }
                        else
                        {
                            foreach (var type in decrypted_types)
                            {
                                if (type.decryptedValue == null)
                                {
                                    throw new Exception("3"); // decryption error
                                }
                                else
                                {
                                    item_types_model.Add(new Item_Type_Model { id = type.id, item_type = type.decryptedValue });
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



        public async Task<Item_Type_Model> GetItemTypeById(Get_Item_Type_Data obj)
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
                        Item_Type_Model item_type_model = new Item_Type_Model { id = item_type.id, item_type = await Crypto.Decrypt(obj.session, item_type.item_type) };

                        return item_type_model;
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
