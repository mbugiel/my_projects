using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Authorized_Worker.Input_Objects
{
    public class Get_All_Authorized_Worker_Data
    {
        public Session_Data session { get; set; }

        public long client_id { get; set; }
    }
}
