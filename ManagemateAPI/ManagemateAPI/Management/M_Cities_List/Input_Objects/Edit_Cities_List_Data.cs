using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Cities_List.Input_Objects
{
    public class Edit_Cities_List_Data
    {
        public Session_Data session { get; set; }
        public long id { get; set; }
        public string city { get; set; }
    }
}
