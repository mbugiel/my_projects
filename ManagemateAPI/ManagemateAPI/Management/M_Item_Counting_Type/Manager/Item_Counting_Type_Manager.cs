using ManagemateAPI.Database.Context;
using ManagemateAPI.Database.Tables;
using ManagemateAPI.Information;
using ManagemateAPI.Management.M_Item_Counting_Type.Input_Objects;
using ManagemateAPI.Management.M_Item_Counting_Type.Table_Model;
using ManagemateAPI.Management.M_Session.Manager;

namespace ManagemateAPI.Management.M_Item_Counting_Type.Manager
{
    public class Item_Counting_Type_Manager
    {

        private DB_Context _context;
        private readonly IConfiguration _configuration;


        public Item_Counting_Type_Manager(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<string> AddItemCountingType(Add_Item_Counting_Type_Data obj)
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

                    var counting_type_checker = _context.Item_Counting_Type.Where(i => i.counting_type.Equals(obj.counting_type)).FirstOrDefault();

                    if (counting_type_checker != null)
                    {
                        throw new Exception("18"); // Already exist
                    }
                    else
                    {
                        Item_Counting_Type new_record = new Item_Counting_Type
                        {
                            counting_type = obj.counting_type
                        };
                        _context.Item_Counting_Type.Add(new_record);
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
        public async Task<string> EditItemCountingType(Edit_Item_Counting_Type_Data obj)
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

                    var edited_record = _context.Item_Counting_Type.Where(c => c.id.Equals(obj.item_counting_type_id)).FirstOrDefault();

                    if (edited_record != null)
                    {
                        var counting_type_checker = _context.Item_Counting_Type.Where(i => i.counting_type.Equals(obj.counting_type)).FirstOrDefault();

                        if (counting_type_checker != null)
                        {
                            throw new Exception("18"); // Already in use
                        }
                        else
                        {

                            edited_record.counting_type = obj.counting_type;

                            _context.SaveChanges();

                            return Info.SUCCESSFULLY_ADDED;
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
        public async Task<string> DeleteItemCountingType(Delete_Item_Counting_Type_Data obj)
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

                    var id_exits = _context.Item_Counting_Type.Where(i => i.id.Equals(obj.item_counting_type_id)).FirstOrDefault();

                    if (id_exits == null)
                    {
                        throw new Exception("19");// counting type not found
                    }
                    else
                    {
                        _context.Item_Counting_Type.Remove(id_exits);
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


        public async Task<List<Item_Counting_Type_Model>> GetItemCountingTypes(Get_Item_Counting_Types_Data obj)
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

                    var item_counting_types = _context.Item_Counting_Type.ToList();

                    if (item_counting_types == null)
                    {
                        throw new Exception("19");// counting types not found
                    }
                    else
                    {
                        List<Item_Counting_Type_Model> item_counting_types_list = new List<Item_Counting_Type_Model>();

                        foreach (var type in item_counting_types)
                        {
                            item_counting_types_list.Add(new Item_Counting_Type_Model { id = type.id, counting_type = type.counting_type });
                        }

                        return item_counting_types_list;

                    }



                }
                else
                {
                    throw new Exception("1");//_1_SESSION_NOT_FOUND
                }

            }

        }



        public async Task<Item_Counting_Type_Model> GetItemCountingTypeById(Get_Item_Counting_Type_Data obj)
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

                    var item_counting_type = _context.Item_Counting_Type.Where(c => c.id.Equals(obj.item_counting_type_id)).FirstOrDefault();

                    if (item_counting_type == null)
                    {
                        throw new Exception("19");// counting type not found
                    }
                    else
                    {
                        Item_Counting_Type_Model item_counting_type_model = new Item_Counting_Type_Model { id = item_counting_type.id, counting_type = item_counting_type.counting_type };

                        return item_counting_type_model;
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
