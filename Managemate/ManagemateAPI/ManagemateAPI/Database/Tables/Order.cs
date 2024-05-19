using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagemateAPI.Database.Tables
{
    [Table("order")]
    public class Order
    {
        [Key, Required]
        public long id { get; set; }

        [Required]
        public byte[] order_name { get; set; }

        [Required]
        public Client client_id_FK { get; set; }

        [Required]
        public Construction_Site construction_site_id_FK { get; set; }

        [Required]
        public int status { get; set; }

        [Required]
        public DateTime creation_date { get; set; }

        public byte[]? comment { get; set; }

        public List<Receipt> receipts_FK { get; set; }


        public byte[] default_payment_method { get; set; }
        public int default_payment_date_offset { get; set; }
        public double default_discount { get; set; }
    }
}
