﻿using ManagemateAPI.Management.M_Session.Input_Objects;

namespace ManagemateAPI.Management.M_Company.Input_Objects
{
    public class Edit_Company_Data
    {
        public Session_Data session { get; set; }

        public long id { get; set; }

        public string name { get; set; }

        public string surname { get; set; }

        public string company_name { get; set; }

        public string NIP { get; set; }

        public string phone_number { get; set; }

        public string email { get; set; }

        public string address { get; set; }

        public long city_id_FK { get; set; }

        public string postal_code { get; set; }

        public string bank_name { get; set; }

        public string bank_number { get; set; }

        public string web_page { get; set; }

        public string money_sign { get; set; }
    }
}