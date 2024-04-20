using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagemateAPI.Database.Tables
{
    [Table("company")]
    public class Company
    {
        [Key, Required]
        public long id { get; set; }

        [Required]
        public byte[] name { get; set; }

        [Required]
        public byte[] surname { get; set; }

        [Required]
        public byte[] company_name { get; set; }

        [Required]
        public byte[] NIP { get; set; }

        [Required]
        public byte[] phone_number { get; set; }

        [Required]
        public byte[] email { get; set; }

        [Required]
        public byte[] address { get; set; }

        [Required]
        public Cities_List city_id_FK { get; set; }

        [Required]
        public byte[] postal_code { get; set; }

        [Required]
        public byte[] bank_name { get; set; }

        [Required]
        public byte[] bank_number { get; set; }

        public byte[] web_page { get; set; }

        [Required]
        public byte[] money_sign { get; set; }
    }
}
