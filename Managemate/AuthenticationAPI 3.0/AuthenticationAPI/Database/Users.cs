using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationAPI.Database
{
    [Table("users")]
    public class Users
    {
        [Required, Key]
        public long Id { get; set; }

        [Required]
        public byte[] Username { get; set; }

        [Required]
        public byte[] UsernameNormalized { get; set; }

        [Required]
        public byte[] Email { get; set; }

        [Required]
        public byte[] Password { get; set; }

        [Required]
        public bool TwoStepLogin { get; set; }

        [Required]
        public byte[] Key { get; set; }

        [Required]
        public byte[] Iv { get; set; }

    }
}
