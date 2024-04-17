namespace ManagemateAPI.Management.M_Construction_Site.Table_Model
{
    public class Construction_Site_Model
    {
        public long id { get; set; }

        public string construction_site_name { get; set; }

        public string address { get; set; }

        public string cities_list_id_FK { get; set; }

        public string postal_code { get; set; }

        public string comment { get; set; }

    }
}
