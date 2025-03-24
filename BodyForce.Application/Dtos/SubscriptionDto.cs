using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    public class SubscriptionDto
    {
        public int SubscriptionTypeId { get; set; }
        [Required(ErrorMessage ="Please enter the name of subcription")]
        [Display(Name = "Subscription Type")]
        [MaxLength(10,ErrorMessage ="Words should not be more than 10")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Subscription Months")]
        public int DurationInMonths { get; set; }
        [Display(Name = "Subscription Days")]
        public int DurationInDays { get; set; }
        [Display(Name = "Status")]
        [Required(ErrorMessage ="Status of subscription is required.")]
        public bool IsActive { get; set; }
        public string? Status { get; set; }
        [Required(ErrorMessage ="Please enter the price of this subscription.")]
        public double Price { get; set; }
    }
}
