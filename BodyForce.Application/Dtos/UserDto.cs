using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? MemberCode { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime DOJ { get; set; }
    }
}
