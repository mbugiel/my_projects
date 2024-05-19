using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Item_On_Receipt.Input_Objects
{
    public class Get_Available_Items_To_Return_Data
    {
        public Session_Data session { get; set; }

        public long order_id { get; set; }

    }
}
