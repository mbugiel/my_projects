using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Client.Input_Objects
{
    public class Delete_Client_Data
    {
        public Session_Data session { get; set; }
        public long id { get; set; }
    }
}
