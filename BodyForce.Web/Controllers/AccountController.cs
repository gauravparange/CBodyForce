using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            ViewBag.SignUp = SignUp;
            return View(new SignUpDto());
        }
        [AllowAnonymous]

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto signUpDto)
        {
            if (ModelState.IsValid)
            {
               var result = await _userService.SignUpUserAsync(signUpDto);
                if (result.Succeeded)
                {
                    return RedirectToAction("login");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
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
                if (result.Succeeded)
                {
                    return RedirectToAction("Index","Home");
                }
                else if(result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, "LogIn Not Allowed");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid User Name or Password");
                }
            }
            return View(loginDto);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> IsEmailAvailable(string Email)
        {
            //Check If the Email Id is Already in the Database
            var user = await _userService.IsEmailAvailableAsync(Email);
            if (user == null)
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
