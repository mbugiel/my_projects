using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Helper.InputObjects.Client
{
    public class Get_Client_By_ID
    {
        public Session_Data session { get; set; }
        public long id_to_get { get; set; }
    }
}
