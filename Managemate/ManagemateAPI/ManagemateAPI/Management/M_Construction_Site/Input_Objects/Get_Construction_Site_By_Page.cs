using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Construction_Site.Input_Objects
{
    public class Get_Construction_Site_By_Page
    {
        public Session_Data session { get; set; }
        public int page_ID { get; set; }
        public int page_Size { get; set; }
    }
}
