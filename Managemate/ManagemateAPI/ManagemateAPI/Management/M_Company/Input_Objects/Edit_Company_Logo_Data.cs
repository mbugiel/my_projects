using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Company.Input_Objects
{
    public class Edit_Company_Logo_Data
    {
        public Session_Data session { get; set; }

        public IFormFile? company_logo { get; set; }

    }
}
