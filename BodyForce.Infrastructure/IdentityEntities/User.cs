using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BodyForce
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ParentPhoneNo { get; set; }
        public DateTime DOB { get; set; }
        public DateTime DOJ { get; set; }
        public string? Address { get; set; }
        public string? Height { get; set; }
        public string? Weight { get; set; }
        public int CreatedBy { get; set; } 
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Boolean IsDeleted { get; set; } = false;
    }
}
