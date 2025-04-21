using BodyForce.Application;
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
        private readonly RoleManager<ApplicationRole> _roleManager;
        public RoleService(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<ResponseResult> CreateRoleAsync(CreateRoleDto roleDto)
        {
            try
            {
                bool roleExists = await _roleManager.RoleExistsAsync(roleDto.RoleName);
                if (roleExists)
                {
                    return new ResponseResult(false, new List<string> { "Role Already Exists" });
                }
                else
                {
                    var result = await _roleManager.CreateAsync(new ApplicationRole()
                    {
                        Name = roleDto.RoleName
                    });
                    if (result.Succeeded)
                    {
                        return new ResponseResult(true);
                    }
                    else
                    {
                        var errors = result.Errors.Select(e => e.Description).ToList();
                        return new ResponseResult(false, errors);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseResult(false,new List<string> { ex.Message});
            }
            
        }
        public async Task<ResponseResult> EditRoleAsync(CreateRoleDto roleDto)
        {
            try
            {
                var result = await _roleManager.UpdateAsync(new ApplicationRole()
                {
                    Id = roleDto.RoleId,
                    Name = roleDto.RoleName
                });
                if (result.Succeeded)
                {
                    return new ResponseResult(true);
                }
                else
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return new ResponseResult(false, errors);
                }
            }
            catch (Exception ex)
            {
                return new ResponseResult(false, new List<string> { ex.Message });
            }

        }
        public async Task<ResponseResult> DeleteRole(int Id)
        {
            try
            {
                var result = await _roleManager.FindByIdAsync(Id.ToString());
                if (result != null)
                {
                    result.IsDeleted = true;
                    await _roleManager.UpdateAsync(result);
                    return new ResponseResult(true);
                }
                return new ResponseResult (false, new List<string> { "Role not found." });              
            }
            catch (Exception ex)
            {
                return new ResponseResult(false, new List<string> { ex.Message });
            }
        }    
        public async Task<ResponseResult<IEnumerable<Role>>> GetAllRoles()
        {
            try
            {
                var allRoles = _roleManager.Roles.Where(x => x.IsDeleted == false).Select(s =>
                new Role() { Id = s.Id,Name = s.Name});
                if(allRoles.Any())
                {
                    return new ResponseResult<IEnumerable<Role>>(true, allRoles);
                }
                return new ResponseResult<IEnumerable<Role>>(false, new List<string> { "No Records Found." });
            }
            catch (Exception ex)
            {
                return new ResponseResult<IEnumerable<Role>>(false, new List<string> { ex.Message });
            }
            
        }    
        public async Task<ResponseResult<Role>> GetRoleById(int RoleId)
        {
            try
            {
                var role = _roleManager.Roles.Where(x => x.Id == RoleId).FirstOrDefault();

                if (role != null)
                {
                    var result = new Role()
                    {
                        Id = role.Id,
                        Name = role.Name
                    };
                    return new ResponseResult<Role>(true, result);
                }
                return new ResponseResult<Role>(false, new List<string> { "No Record Found." });

            }
            catch (Exception ex)
            {
                return new ResponseResult<Role>(false, new List<string> { ex.Message });
            }
        }
    }
}
