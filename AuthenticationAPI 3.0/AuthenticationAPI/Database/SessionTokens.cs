using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationAPI.Database
{
    [Table("session_tokens")]
    public class SessionTokens
    {
        [Required, Key]
        public long Id { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        public byte[] SessionToken { get; set; }

        [Required]
        public DateTime SessionLifetime { get; set; }
    }
}
