using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Receipt.Input_Objects
{
    public class Get_Receipt_List_Data
    {
        public Session_Data session { get; set; }

        public long order_id { get; set; }

        public bool in_out { get; set; }

    }
}
