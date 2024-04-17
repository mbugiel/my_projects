using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Helper.InputObjects.Client
{
    public class Get_Client_Page
    {
        public Session_Data session { get; set; }
        public int page_ID { get; set; }
        public int page_Size { get; set; }
    }
}
