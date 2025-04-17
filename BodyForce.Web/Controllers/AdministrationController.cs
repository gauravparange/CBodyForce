using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BodyForce.Web.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class AdministrationController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly ISubscriptionService _subscriptionService;
        public AdministrationController(IRoleService roleService, ISubscriptionService subscriptionService)
        {
            _roleService = roleService;
            _subscriptionService = subscriptionService;
        }
        public async Task<IActionResult> Roles()
        {
            var result = await _roleService.GetAllRoles();
            return View(result.Data);
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
                if (result.Success)
                {
                    return RedirectToAction("Roles", "Administration");
                }
                foreach (var error in result.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
            }
            return View(roleDto);
        }
        public async Task<IActionResult> EditRole(int RoleId)
        {
            var result = await _roleService.GetRoleById(RoleId); // Implement this method to fetch role details

            if (result.Data == null)
            {
                return NotFound(); // If role not found, return NotFound
            }
            var role = result.Data;
            return View("CreateRole", new CreateRoleDto() { RoleId = role.Id,RoleName = role.Name});
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(CreateRoleDto roleDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.EditRoleAsync(roleDto);
                if (result.Success)
                {
                    return RedirectToAction("Roles", "Administration");
                }
                foreach (var error in result.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
            }
            return View(roleDto);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRole(int Id)
        {
            var result = await _roleService.DeleteRole(Id);
            if (result.Success)
            {
                TempData["Success"] = "Role deleted successfully.";
                return RedirectToAction("Roles", "Administration");
            }
            TempData["Error"] = string.Join("; ", result.ErrorMessages.Select(e => e));
            return RedirectToAction("Roles", "Administration");
        }

        public async Task<IActionResult> SubscriptionTypes()
        {
            var result = await _subscriptionService.GetAllSubscripitonAsync();
            return View(result);
        }
        public async Task<IActionResult> AddSubscriptionType()
        {
            return View(new SubscriptionDto());
        }
        [HttpPost]
        public async Task<IActionResult> AddSubscriptionType(SubscriptionDto subscriptionDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _subscriptionService.AddSubscription(subscriptionDto);
                if (result.Success)
                {
                    return RedirectToAction("SubscriptionTypes", "Administration");
                }
                foreach (var error in result.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
            }
            return View();
        }
        public async Task<IActionResult> EditSubscriptionType(int subscriptionTypeId)
        {
            var result = await _subscriptionService.GetSubscripitonTypeAsyncVyId(subscriptionTypeId);
            if(result != null)
            {
                return View("AddSubscriptionType",result);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditSubscriptionType(SubscriptionDto subscriptionDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _subscriptionService.EditSubscription(subscriptionDto);
                if (result.Success)
                {
                    return RedirectToAction("SubscriptionTypes", "Administration");
                }
                foreach (var error in result.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
            }
            return View(subscriptionDto);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSubscription(int Id)
        {
             var result =  await _subscriptionService.DeleteSubscription(Id);

            if (result.Success)
            {
                TempData["Success"] = "Role deleted successfully.";
                return RedirectToAction("SubscriptionTypes", "Administration");
            }
            TempData["Error"] = string.Join("; ", result.ErrorMessages.Select(e => e));
            return RedirectToAction("SubscriptionTypes", "Administration");           
        }
    }
}
