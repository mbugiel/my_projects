using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Invoice.Input_Objects
{
    public class Get_Invoice_Available_List_Data
    {
        public Session_Data session { get; set; }

        public long order_id { get; set; }
    }
}
