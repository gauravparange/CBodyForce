using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core.Infrastructure;

namespace BodyForce
{
  
    [Authorize(Roles = "Administrator")]
    public class MemberShipController : Controller
    {
        private readonly IMemberShipService _memberShipService;
        public MemberShipController(IMemberShipService memberShipService)
        {
            _memberShipService = memberShipService;
        }
        
        public async Task<IActionResult> Member()
        {
            
            return View(await _memberShipService.GetAllMembers());
        }
        public async Task<IActionResult> AddMember()
        {
            ViewBag.ForMember = true;
            return RedirectToAction("SignUp","Account",new {SignUp = false});
        }
        public async Task<IActionResult> EditMember(int UserId)
        {            
            return View(await _memberShipService.GetMember(UserId));
        }
        public async Task<IActionResult> ViewMembership(int UserId)
        {
            ViewBag.UserId = UserId;
            return View(await _memberShipService.ViewMemberShip(UserId));
        }
        public async Task<IActionResult> AddMembership(int UserId)
        {
            return View(await _memberShipService.GetMemberShip(UserId));
        }
        [HttpPost]
        public async Task<IActionResult> AddMembership(MembershipDto membershipDto)
        {

            if (ModelState.IsValid)
            {
                var result = await _memberShipService.AddMemberShip(membershipDto);
                if (result.Succeeded)
                {
                    return RedirectToAction("ViewMembership", "MemberShip", new { UserId = membershipDto.UserId } );
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> EditMember(SignUpDto signUpDto)
        //{

        //    return View(await _memberShipService.EditMember(signUpDto));
        //}
    }
}
