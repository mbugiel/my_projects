using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Construction_Site.Input_Objects
{
    public class Edit_Construction_Site_Data
    {
        public Session_Data session { get; set; }
        public long id { get; set; }
        public string construction_site_name { get; set; }
        public string address { get; set; }
        public long cities_list_id_fk { get; set; }
        public string postal_code { get; set; }
        public string comment { get; set; }
    }
}
