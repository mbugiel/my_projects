using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationAPI.Database
{
    [Table("confirmation_codes")]
    public class ConfirmationCodes
    {
        [Required, Key]
        public byte[] Email { get; set; }

        [Required]
        public byte[] ConfirmationCode { get; set; }

        [Required]
        public DateTime CodeLifetime { get; set; }

        [Required]
        public int Attempts { get; set; }

        [Required]
        public DateTime Delay { get; set; }
    }
}
