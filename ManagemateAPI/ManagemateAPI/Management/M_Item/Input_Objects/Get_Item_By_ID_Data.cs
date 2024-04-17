using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Item.Input_Objects
{
    public class Get_Item_By_ID_Data
    {
        public Session_Data session { get; set; }
        public long id_to_get { get; set; }
    }
}
