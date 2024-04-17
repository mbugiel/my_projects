using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_User.Input_Objects
{
    public class Update_Email_Data
    {
        public Session_Data SessionData { get; set; }
        public string newEmail { get; set; }
        public string EmailToken { get; set; }
    }
}
