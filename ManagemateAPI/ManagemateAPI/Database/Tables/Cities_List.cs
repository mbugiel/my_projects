using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagemateAPI.Database.Tables
{
    [Table("cities_list")]
    public class Cities_List
    {
        [Key, Required]
        public long id { get; set; }

        [Required]
        public byte[] city { get; set; }
    }
}
