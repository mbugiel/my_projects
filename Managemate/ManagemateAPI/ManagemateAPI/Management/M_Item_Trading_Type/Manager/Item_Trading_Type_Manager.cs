using ManagemateAPI.Database.Context;
using ManagemateAPI.Management.M_Session.Manager;
using ManagemateAPI.Management.M_Item_Trading_Type.Input_Objects;
using ManagemateAPI.Management.M_Item_Trading_Type.Table_Model;


/*
 * This is an endpoint controller dedicated to the Item_Trading_Type table.
 * 
 * It contains methods for endpoints
 * - Get all 
 */
namespace ManagemateAPI.Management.M_Item_Trading_Type.Manager
{
    public class Item_Trading_Type_Manager
    {

        private DB_Context _context;
        private readonly IConfiguration _configuration;


        public Item_Trading_Type_Manager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /*
         * Get_All_Item_Trading_Type endpoint
         * This endpoint is used to to all the records from the Item_Trading_Type table.
         * 
         * It accepts Get_All_Item_Trading_Type_Data object.
         * The given object is handed over to the Get_All_Item_Trading_Type method in the Item_Trading_Type_Manager.
         */
        public async Task<List<Item_Trading_Type_Model>> Get_All_Item_Trading_Types(Get_All_Item_Trading_Types_Data obj)
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

                    var item_trading_types = _context.Item_Trading_Type.ToList();

                    if (item_trading_types == null)
                    {
                        throw new Exception("19");// item types not found
                    }
                    else
                    {
                        List<Item_Trading_Type_Model> item_trading_types_model = new List<Item_Trading_Type_Model>();


                        foreach (var trading_type in item_trading_types)
                        {
                            item_trading_types_model.Add(new Item_Trading_Type_Model { id = trading_type.id, trading_type_pl = trading_type.trading_type_pl, trading_type_en = trading_type.trading_type_en });
                        }




                        return item_trading_types_model;

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
