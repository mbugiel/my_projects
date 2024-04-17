using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagemateAPI.Database.Tables
{
    [Table("invoice")]
    public class Invoice
    {
        [Key, Required]
        public long id { get; set; }

        [Required]
        public string prefix { get; set; }

        [Required]
        public long year { get; set; }

        [Required]
        public long month { get; set; }

        [Required]
        public long number { get; set; }

        [Required]
        public Order order_id_FK { get; set; }

        [Required]
        public DateOnly issue_date { get; set; }

        [Required]
        public DateOnly sale_date { get; set; }

        [Required]
        public DateOnly payment_date { get; set; }

        [Required]
        public byte[] payment_method { get; set; }

        [Required]
        public double discount { get; set; }

        [Required]
        public byte[] net_worth { get; set; }

        [Required]
        public byte[] tax_worth { get; set; }

        [Required]
        public byte[] gross_worth { get; set; }

        public byte[]? comment { get; set; }

        public byte[]? comment_2 { get; set; }
    }
}
