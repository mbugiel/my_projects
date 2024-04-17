using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagemateAPI.Database.Tables
{
    [Table("item")]
    public class Item
    {
        [Key, Required]
        public long id { get; set; }

        [Required]
        public string catalog_number { get; set; }

        [Required]
        public byte[] product_name { get; set; }

        [Required]
        public Item_Type item_type_id_FK { get; set; }

        [Required]
        public double weight_kg { get; set; }

        [Required]
        public long count { get; set; }

        [Required]
        public long blocked_count { get; set; }

        [Required]
        public byte[] price { get; set; }

        [Required]
        public double tax_pct { get; set; }

        [Required]
        public Item_Trading_Type item_trading_type_id_FK { get; set; }

        [Required]
        public Item_Counting_Type item_counting_type_id_FK { get; set; }

        public byte[]? comment { get; set; }
    }
}
