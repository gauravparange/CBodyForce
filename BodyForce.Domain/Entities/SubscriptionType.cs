using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    [Table("SubscriptionType")]
    public class SubscriptionType : Audit
    {
        public int SubscriptionTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DurationInMonths { get; set; }
        public int DurationInDays { get; set; }
        public bool IsActive { get; set; }
        public double Price { get; set; }
    }
}
