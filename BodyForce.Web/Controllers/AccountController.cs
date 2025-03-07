using Microsoft.AspNetCore.Mvc;

namespace BodyForce.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Route("signup")]
        public async Task<IActionResult> SignUp()
        {
            return View();
        }
        [Route("login")]
        public async Task<IActionResult> LogIn()
        {
            return View();
        }
    }
}
