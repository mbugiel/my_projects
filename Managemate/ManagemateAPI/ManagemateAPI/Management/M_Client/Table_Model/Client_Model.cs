namespace ManagemateAPI.Management.M_Client.Table_Model
{
    public class Client_Model
    {

        public long id { get; set; }

        public string surname { get; set; }

        public string name { get; set; }

        public string company_name { get; set; }

        public string NIP { get; set; }

        public string phone_number { get; set; }

        public string email { get; set; }

        public string address { get; set; }

        public string city_id_FK { get; set; }

        public string postal_code { get; set; }

        public string comment { get; set; }

    }
}
