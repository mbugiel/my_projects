using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Receipt.Input_Objects
{
    public class Get_Receipts_Data
    {
        public Session_Data session { get; set; }

        public long order_id { get; set; }

        public int pageid { get; set; }

        public int pageSize { get; set; }
    }
}
