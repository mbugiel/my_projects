using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Item_Type.Input_Objects
{
    public class Add_Item_Type_Data
    {
        public Session_Data session { get; set; }

        public string item_type { get; set; }
    }
}
