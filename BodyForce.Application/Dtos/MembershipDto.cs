using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    public class MembershipDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Please select the type of subscription")]
        [Display(Name ="Subscripiton Type")]
        public int SubscriptionTypeId { get; set; }
        [Required(ErrorMessage = "Enter the amount paid by the member")]
        [Display(Name = "Amount Paid")]
        [DataType(DataType.Currency)]
        public double AmountPaid { get; set; }
        [Display(Name = "Payment Date")]
        [Required(ErrorMessage = "Please select the payment date")]
        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; }
        [Display(Name = "Mode of Payment")]
        [Required(ErrorMessage = "Please select the payment mode")]
        public string PaymentMethod  { get; set; }
        [MinLength(5)]
        public string? Notes { get; set; }
    }
}
