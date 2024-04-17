using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Item_Type.Input_Objects
{
    public class Delete_Item_Type_Data
    {
        public Session_Data session { get; set; }

        public long item_type_id { get; set; }
    }
}
