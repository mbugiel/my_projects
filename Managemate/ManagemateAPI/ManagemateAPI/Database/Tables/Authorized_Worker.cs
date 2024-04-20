using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagemateAPI.Database.Tables
{
    [Table("authorized_worker")]
    public class Authorized_Worker
    {
        [Key, Required]
        public long id { get; set; }

        [Required]
        public Client client_id_FK { get; set; }

        [Required]
        public byte[] name { get; set; }

        [Required]
        public byte[] surname { get; set; }

        [Required]
        public byte[] phone_number { get; set; }


        public byte[] email { get; set; }

        [Required]
        public bool contact { get; set; }

        [Required]
        public bool collection { get; set; }

        public byte[]? comment { get; set; }
    }
}
