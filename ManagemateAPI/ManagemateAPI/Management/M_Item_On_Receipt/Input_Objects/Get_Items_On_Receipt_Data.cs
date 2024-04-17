using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Item_On_Receipt.Input_Objects
{
    public class Get_Items_On_Receipt_Data
    {
        public Session_Data session { get; set; }

        public long receipt_id { get; set; }

    }
}
