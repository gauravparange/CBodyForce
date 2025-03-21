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
        public async Task<IActionResult> ViewMembership(int MembershipId)
        {

            return View();
        }
        public async Task<IActionResult> EditMember(int UserId)
        {
            
            return View(await _memberShipService.GetMember(UserId));
        }
        //[HttpPost]
        //public async Task<IActionResult> EditMember(SignUpDto signUpDto)
        //{

        //    return View(await _memberShipService.EditMember(signUpDto));
        //}
    }
}
