using Microsoft.AspNetCore.Mvc;

namespace BodyForce
{
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
        public async Task<IActionResult> EditMember(int UserId)
        {
            
            return View(await _memberShipService.GetMember(UserId));
        }
        [HttpPost]
        public async Task<IActionResult> EditMember(SignUpDto signUpDto)
        {

            return View(await _memberShipService.EditMember(signUpDto));
        }
    }
}
