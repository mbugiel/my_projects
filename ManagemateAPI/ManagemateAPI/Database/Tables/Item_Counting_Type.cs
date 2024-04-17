using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagemateAPI.Database.Tables
{
    [Table("item_counting_type")]
    public class Item_Counting_Type
    {
        [Required]
        public long id { get; set; }

        [Required]
        public string counting_type { get; set; }
    }
}
