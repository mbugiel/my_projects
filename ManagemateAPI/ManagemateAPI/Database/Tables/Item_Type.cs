using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagemateAPI.Database.Tables
{
    [Table("item_type")]
    public class Item_Type
    {
        [Key, Required]
        public long id { get; set; }

        [Required]
        public byte[] item_type { get; set; }


    }
}
