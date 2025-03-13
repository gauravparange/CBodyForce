using Microsoft.AspNetCore.Mvc;

namespace BodyForce
{
    public class MemberShipController : Controller
    {

        public IActionResult Member()
        {
            return View();
        }
    }
}
