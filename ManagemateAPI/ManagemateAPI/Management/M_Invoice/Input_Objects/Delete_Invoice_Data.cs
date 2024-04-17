using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Invoice.Input_Objects
{
    public class Delete_Invoice_Data
    {
        public Session_Data session { get; set; }
        public int invoice_id { get; set; }
    }
}
