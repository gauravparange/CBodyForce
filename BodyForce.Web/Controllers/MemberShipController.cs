using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core.Infrastructure;

namespace BodyForce
{
    [Authorize]

    public class MemberShipController : Controller
    {
        private readonly IMemberShipService _memberShipService;
        public MemberShipController(IMemberShipService memberShipService)
        {
            _memberShipService = memberShipService;
        }
        [Authorize(Roles="Administrator")]
        public async Task<IActionResult> Member()
        {
            
            return View(await _memberShipService.GetAllMembers());
        }
        [Authorize(Roles="Administrator")]
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
            return View(await _memberShipService.ViewMemberShip(UserId));
        }
        public async Task<IActionResult> AddMembership(int UserId)
        {
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> EditMember(SignUpDto signUpDto)
        //{

        //    return View(await _memberShipService.EditMember(signUpDto));
        //}
    }
}
