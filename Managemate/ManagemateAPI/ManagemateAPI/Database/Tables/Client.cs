using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagemateAPI.Database.Tables
{
    [Table("client")]
    public class Client
    {
        [Key, Required]
        public long id { get; set; }

        [Required]
        public byte[] surname { get; set; }

        [Required]
        public byte[] name { get; set; }


        public byte[] company_name { get; set; }


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

        public byte[]? comment { get; set; }
    }
}
