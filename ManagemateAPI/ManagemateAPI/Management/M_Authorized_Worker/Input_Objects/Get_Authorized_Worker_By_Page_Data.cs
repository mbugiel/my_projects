using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Authorized_Worker.Input_Objects
{
    public class Get_Authorized_Worker_By_Page_Data
    {
        public Session_Data session { get; set; }
        public int page_ID { get; set; }
        public int page_Size { get; set; }
    }
}
