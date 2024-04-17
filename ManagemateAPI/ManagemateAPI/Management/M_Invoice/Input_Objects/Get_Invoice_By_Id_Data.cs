using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Invoice.Input_Objects
{
    public class Get_Invoice_By_Id_Data
    {
        public Session_Data session { get; set; }
        public long id_to_get { get; set; }
    }
}
