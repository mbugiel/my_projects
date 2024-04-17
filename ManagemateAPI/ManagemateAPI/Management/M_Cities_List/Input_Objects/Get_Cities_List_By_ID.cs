using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Cities_List.Input_Objects
{
    public class Get_Cities_List_By_ID
    {
        public Session_Data session { get; set; }
        public long id_to_get { get; set; }
    }
}
