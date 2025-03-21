using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BodyForce.Web.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class AdministrationController : Controller
    {
        private readonly IRoleService _roleService;
        public AdministrationController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public async Task<IActionResult> Roles()
        {
            var result = await _roleService.GetAllRoles();
            return View(result);
        }

        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole()
        {
            return View(new CreateRoleDto());
        }
        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole(CreateRoleDto roleDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.CreateRoleAsync(roleDto);
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles", "Administration");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(roleDto);
        }
        public async Task<IActionResult> EditRole(int RoleId)
        {
            var role = await _roleService.GetRoleById(RoleId); // Implement this method to fetch role details

            if (role == null)
            {
                return NotFound(); // If role not found, return NotFound
            }

            return View("CreateRole", new CreateRoleDto() { RoleId = role.Id,RoleName = role.Name});
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(CreateRoleDto roleDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.EditRoleAsync(roleDto);
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles", "Administration");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(roleDto);
        }
    }
}
