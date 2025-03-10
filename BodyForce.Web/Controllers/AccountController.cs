using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BodyForce.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult SignUp()
        {
            return View();
        }
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
            return View(signUpDto);
        }
        
        public async Task<IActionResult> LogIn()
        {
            return View();
        }
        [HttpPost]
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
                    ModelState.AddModelError(string.Empty, "logIn Not Allowed");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Credentials");
                }
            }
            return View(loginDto);
        }

    }
}
