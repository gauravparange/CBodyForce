using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core.Infrastructure;

namespace BodyForce
{
  
    [Authorize(Roles = "Administrator")]
    public class MemberShipController : Controller
    {
        private readonly IMemberShipService _memberShipService;
        private readonly IUserService _userService;
        public MemberShipController(IMemberShipService memberShipService, IUserService userService)
        {
            _memberShipService = memberShipService;
            _userService = userService;
        }
        
        public async Task<IActionResult> Member()
        {
            var result = await _userService.GetUserList();          
                return View(result.Data);
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
        [HttpPost]
        public async Task<IActionResult> EditMember(EditMemberDto editMemberDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.UpdateUserAsync(editMemberDto);
                if (result.Success)
                {
                    return RedirectToAction("Member", "MemberShip");
                }
                foreach (var error in result.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
            }
            return View();
        }
        public async Task<IActionResult> ViewMembership(int UserId)
        {
            ViewBag.UserId = UserId;
            return View(await _memberShipService.ViewMemberShip(UserId));
        }
        public async Task<IActionResult> AddMembership(int UserId)
        {
           
            return View(new MembershipDto()
            {
                UserId = UserId,
            });
        }
        public async Task<IActionResult> EditMembership(int MemberShipId, int UserId)
        {
            return View("AddMembership", await _memberShipService.GetMemberShip(MemberShipId));
        }
        [HttpPost]
        public async Task<IActionResult> AddMembership(MembershipDto membershipDto)
         {

            if (ModelState.IsValid)
            {
                var result = await _memberShipService.AddMemberShip(membershipDto);
                if (result.Success)
                {
                    return RedirectToAction("ViewMembership", "MemberShip", new { UserId = membershipDto.UserId } );
                }

                foreach (var errorMessage in result.ErrorMessages)
                {
                    ModelState.AddModelError("", errorMessage);
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditMembership(MembershipDto membershipDto)
        {

            if (ModelState.IsValid)
            {
                // Call the service method that now returns OperationResult
                var result = await _memberShipService.EditMemberShip(membershipDto);

                if (result.Success)  // Checking if the operation was successful
                {
                    // Redirect to the ViewMembership page if successful
                    return RedirectToAction("ViewMembership", "MemberShip", new { UserId = membershipDto.UserId });
                }
                else
                {
                    // If not successful, add errors to ModelState
                    foreach (var errorMessage in result.ErrorMessages)
                    {
                        ModelState.AddModelError("", errorMessage);
                    }
                }
            }
            return View(membershipDto);
        }
        //[HttpPost]
        //public async Task<IActionResult> EditMembership(SignUpDto signUpDto)
        //{

        //    return View(await _memberShipService.EditMember(signUpDto));
        //}
    }
}
