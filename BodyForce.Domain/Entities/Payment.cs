using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BodyForce
{
    [Table("Payment")]
    public class Payment : Audit
    {
        [Key]
        public int PaymentId { get; set; }
        public int UserId { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Notes { get; set; }

    }
}
