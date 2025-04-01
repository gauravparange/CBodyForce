using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    public class ViewMembershipDto
    {
        public int MemberShipId { get; set; }
        public int UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? RenewalDate { get; set; }
        public int SubscriptionTypeId { get; set; }
        public string? SubscriptionName { get; set; }
        public string? Payment { get; set; }
    }
}
