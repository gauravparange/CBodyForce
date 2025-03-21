using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        public RoleService(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IdentityResult> CreateRoleAsync(CreateRoleDto roleDto)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleDto.RoleName);
            if (roleExists)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Role Already Exists"
                });
            }
            else
            {
                return await _roleManager.CreateAsync(new Role()
                {
                    Name = roleDto.RoleName
                });
            }
        }
        public async Task<IdentityResult> EditRoleAsync(CreateRoleDto roleDto)
        {        
            return await _roleManager.UpdateAsync(new Role()
            {
                Id = roleDto.RoleId,
                Name = roleDto.RoleName
            });
        }
        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            var allRoles = _roleManager.Roles.Where(x => x.IsDeleted == false);
            return allRoles;
        }    
        public async Task<Role> GetRoleById(int RoleId)
        {
            var result = _roleManager.Roles.Where(x => x.Id == RoleId).FirstOrDefault();
            return result;
        }
    }
}
