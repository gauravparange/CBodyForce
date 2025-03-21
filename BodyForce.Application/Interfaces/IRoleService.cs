using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    public interface IRoleService
    {
        Task<IdentityResult> CreateRoleAsync(CreateRoleDto roleDto);
        Task<IdentityResult> EditRoleAsync(CreateRoleDto roleDto);
        Task<IEnumerable<Role>> GetAllRoles();
        Task<Role> GetRoleById(int RoleId);
    }
}
