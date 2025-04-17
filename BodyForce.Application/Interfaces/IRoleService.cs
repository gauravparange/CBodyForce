
using BodyForce.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    public interface IRoleService
    {
        Task<ResponseResult> CreateRoleAsync(CreateRoleDto roleDto);
        Task<ResponseResult> EditRoleAsync(CreateRoleDto roleDto);
        Task<ResponseResult<IEnumerable<Role>>> GetAllRoles();
        Task<ResponseResult<Role>> GetRoleById(int RoleId);
        Task<ResponseResult> DeleteRole(int Id);
    }
}
