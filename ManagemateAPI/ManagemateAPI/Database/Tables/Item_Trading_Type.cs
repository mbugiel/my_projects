using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagemateAPI.Database.Tables
{
    [Table("item_trading_type")]
    public class Item_Trading_Type
    {
        [Key, Required]
        public long id { get; set; }

        [Required]
        public string trading_type_pl { get; set; }

        [Required]
        public string trading_type_en { get; set; }
    }
}
