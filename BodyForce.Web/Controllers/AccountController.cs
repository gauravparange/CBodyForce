using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BodyForce.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        [AllowAnonymous]

        public IActionResult SignUp(bool SignUp = true)
        {
            TempData["SignUp"] = SignUp;
            return View(new SignUpDto());
        }
        [AllowAnonymous]

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto signUpDto)
        {
            if (ModelState.IsValid)
            {
                bool SignUp = Convert.ToBoolean(TempData["SignUp"]);
                var result = await _userService.SignUpUserAsync(signUpDto);
                if (result.Success)
                {
                    return RedirectToAction("Member", "Membership");
                }
                else
                {
                    foreach (var error in result.ErrorMessages)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
            }
            ViewBag.SignUp = true;
            return View(signUpDto);
        }
        [AllowAnonymous]
        public async Task<IActionResult> LogIn()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> LogIn(LogInDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LogInUserAsync(loginDto);
                if (result.Success)
                {
                    return RedirectToAction("Index","Home");
                }
                else 
                {
                    foreach (var error in result.ErrorMessages)
                    {
                         ModelState.AddModelError(string.Empty, error);
                    }                    
                }
            }
            return View(loginDto);
        }

        //public async Task<IActionResult> Settings()
        //{
        //    var user = await _userService.GetUserAsync();
        //    var userDto = new UserDto
        //    {
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        Email = user.Email,
        //        PhoneNumber = user.PhoneNumber,
        //        UserName = user.UserName
        //    };
        //    return View(userDto);
        //}

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> IsEmailAvailable(string Email)
        {
            //Check If the Email Id is Already in the Database
            var isAvailable = await _userService.IsEmailAvailableAsync(Email);
            if (isAvailable)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {Email} is already in use.");
            }
        }

        public async Task<IActionResult> LogOut()
        {
            _userService.LogOut();
            return RedirectToAction("LogIn","Account");
        }
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
    }
}
