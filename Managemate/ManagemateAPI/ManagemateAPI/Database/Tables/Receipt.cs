using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagemateAPI.Database.Tables
{
    [Table("receipt")]
    public class Receipt
    {
        [Key, Required]
        public long id { get; set; }

        [Required]
        public bool in_out { get; set; }

        [Required]
        public DateTime date { get; set; }

        [Required]
        public Order order_id_FK { get; set; }

        public byte[] element { get; set; }

        public byte[] transport { get; set; }

        public double summary_weight { get; set; }

        public byte[]? comment { get; set; }

        public bool reservation {  get; set; }

        public List<Item_On_Receipt> items_on_receipt_FK { get; set; }
    }
}
