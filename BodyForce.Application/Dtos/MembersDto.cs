using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    public class MembersDto 
    {
        public int MemberId { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? MemberCode { get; set; }
        public string? PhoneNo { get; set; }
        public DateTime DOJ { get; set; }
        public string? MembershipStatus { get; set; }
        public DateTime? MembershipStartDate { get; set; }
        public DateTime? MembershipEndDate { get; set; }

    }
}
