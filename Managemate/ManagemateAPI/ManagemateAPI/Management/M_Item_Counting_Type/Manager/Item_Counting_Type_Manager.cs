using ManagemateAPI.Database.Context;
using ManagemateAPI.Database.Tables;
using ManagemateAPI.Information;
using ManagemateAPI.Management.M_Item_Counting_Type.Input_Objects;
using ManagemateAPI.Management.M_Item_Counting_Type.Table_Model;
using ManagemateAPI.Management.M_Session.Manager;

/*
 * This is the Item_Counting_Type_Manager with methods dedicated to the Item_Counting_Type table.
 * 
 * It contains methods to:
 * add records,
 * edit records,
 * delete records,
 * get record by id,
 * get all the records.
 */
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

        /* 
         * Add_Item_Counting_Type method
         * This method is used to add new records to the Item_Counting_Type table.
         * 
         * It accepts Add_Item_Counting_Type_Data object as input.
         * It then adds new record with values based on the data given in the input object.
         */
        public async Task<string> Add_Item_Counting_Type(Add_Item_Counting_Type_Data obj)
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

                    var counting_type_checker = _context.Item_Counting_Type.Where(i => i.counting_type.Equals(obj.counting_type) && i.id > 0).FirstOrDefault();

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

        /* 
         * Edit_Item_Counting_Type method
         * This method is used to edit a record in the Item_Counting_Type table.
         * 
         * It accepts Edit_Item_Counting_Type_Data object as input.
         * It then changes values of a record with those given in the input object only if its ID matches the one in the input object.
         */
        public async Task<string> Edit_Item_Counting_Type(Edit_Item_Counting_Type_Data obj)
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
                        var counting_type_checker = _context.Item_Counting_Type.Where(i => i.counting_type.Equals(obj.counting_type) && i.id > 0).FirstOrDefault();

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

        /*
         * Delete_Item_Counting_Type method
         * This method is used to a record from the Item_Counting_Type table.
         *  
         * It accepts Delete_Item_Counting_Type_Data object as input.
         * Then it deletes a record if its ID matches the one given in the input object.
         */
        public async Task<string> Delete_Item_Counting_Type(Delete_Item_Counting_Type_Data obj)
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

                    var id_exits = _context.Item_Counting_Type.Where(i => i.id.Equals(obj.item_counting_type_id) && i.id > 0).FirstOrDefault();

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

        /*
         * Get_Item_Counting_Type_By_ID method
         * This method gets a record from the Item_Counting_Type table by its ID and returns it.
         * 
         * It accepts Get_Item_Counting_Type_By_ID_Data object as input.
         * Then it gets a records that has the same ID as the ID given in the input object
         */
        public async Task<Item_Counting_Type_Model> Get_Item_Counting_Type_By_ID(Get_Item_Counting_Type_Data obj)
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

        /*
         * Get_All_Item_Counting_Type method
         * This method gets all of the records in the Item_Counting_Type table and returns them in a list.
         * 
         * It accepts Get_All_Item_Counting_Type_Data object as input.
         */
        public async Task<List<Item_Counting_Type_Model>> Get_All_Item_Counting_Type(Get_Item_Counting_Type_Data obj)
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

                    var item_counting_types = _context.Item_Counting_Type.Where(i => i.id > 0).ToList();

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
    }
}
