using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Construction_Site.Input_Objects
{
    public class Get_Construction_Site_By_ID
    {
        public Session_Data session { get; set; }
        public long id_to_get { get; set; }
    }
}
