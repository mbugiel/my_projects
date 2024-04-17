using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagemateAPI.Database.Tables
{
    [Table("construction_site")]
    public class Construction_Site
    {
        [Key, Required]
        public long id { get; set; }

        [Required]
        public byte[] construction_site_name { get; set; }

        [Required]
        public byte[] address { get; set; }

        [Required]
        public Cities_List cities_list_id_FK { get; set; }

        [Required]
        public byte[] postal_code { get; set; }

        public byte[]? comment { get; set; }
    }
}
