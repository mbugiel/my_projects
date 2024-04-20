using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Cities_List.Input_Objects
{
    public class Get_Cities_List_By_Page
    {
        public Session_Data session { get; set; }
        public int page_ID { get; set; }
        public int page_Size { get; set; }
    }
}
