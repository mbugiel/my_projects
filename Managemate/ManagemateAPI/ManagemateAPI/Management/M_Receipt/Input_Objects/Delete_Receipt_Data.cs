using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Receipt.Input_Objects
{
    public class Delete_Receipt_Data
    {
        public Session_Data session { get; set; }

        public long receipt_id { get; set; }

    }
}
