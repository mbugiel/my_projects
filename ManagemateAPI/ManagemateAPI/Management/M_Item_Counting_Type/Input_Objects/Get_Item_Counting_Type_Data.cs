using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Item_Counting_Type.Input_Objects
{
    public class Get_Item_Counting_Type_Data
    {
        public Session_Data session { get; set; }

        public long item_counting_type_id { get; set; }
    }
}
