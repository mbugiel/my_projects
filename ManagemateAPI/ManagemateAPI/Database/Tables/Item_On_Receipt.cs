using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagemateAPI.Database.Tables
{
    [Table("item_on_receipt")]
    public class Item_On_Receipt
    {
        [Key, Required]
        public long id { get; set; }

        [Required]
        public Receipt receipt_id_FK { get; set; }

        [Required]
        public Item item_id_FK { get; set; }

        [Required]
        public double count { get; set; }

        [Required]
        public double summary_weight { get; set; }

        [Required]
        public byte[] annotation { get; set; }


    }
}
