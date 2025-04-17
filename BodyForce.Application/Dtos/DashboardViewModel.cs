using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    public class DashboardViewModel
    {
        public int ActiveMembershipsCount { get; set; }
        public int SubscriptionRenewalCount { get; set; }
        public int PendingPaymentsCount { get; set; }
        public int InActiveMembershipsCount { get; set; }
    }
}
