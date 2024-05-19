using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagemateAPI.Database.Tables
{
    [Table("company")]
    public class Company
    {
        [Key]
        public long id { get; set; }

        public byte[] name { get; set; }

        public byte[] surname { get; set; }

        public byte[] company_name { get; set; }

        public byte[] NIP { get; set; }

        public byte[] phone_number { get; set; }

        public byte[] email { get; set; }

        public byte[] address { get; set; }

        public Cities_List city_id_FK { get; set; }

        public byte[] postal_code { get; set; }

        public byte[] bank_name { get; set; }

        public byte[] bank_number { get; set; }

        public byte[] web_page { get; set; }

        public byte[] money_sign { get; set; }

        public byte[] money_sign_decimal { get; set; }

        public byte[]? company_logo { get; set; }

        public string? file_type { get; set; }
    }
}
