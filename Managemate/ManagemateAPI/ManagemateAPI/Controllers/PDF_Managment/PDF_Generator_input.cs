using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Controllers.PDF_Managment
{
    public class PDF_Generator_input
    {
        public Session_Data session { get; set; }
        public string html { get; set; }
    }
}
