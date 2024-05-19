namespace ManagemateAPI.Management.M_Company.Table_Model
{
    public class Company_Model
    {
        public string name { get; set; }

        public string surname { get; set; }

        public string company_name { get; set; }

        public string NIP { get; set; }

        public string phone_number { get; set; }

        public string email { get; set; }

        public string address { get; set; }

        public string city_name { get; set; }

        public string postal_code { get; set; }

        public string bank_name { get; set; }

        public string bank_number { get; set; }

        public string web_page { get; set; }

        public string money_sign { get; set; }

        public string money_sign_decimal { get; set; }

        public byte[]? company_logo { get; set; }

        public string? file_type { get; set; }
    }
}
