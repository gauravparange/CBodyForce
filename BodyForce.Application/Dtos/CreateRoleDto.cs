using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    public class CreateRoleDto
    {
        public int RoleId { get; set; }
        [Display(Name = "Role Name")]
        [Required]
        public string RoleName { get; set; }
    }
}
