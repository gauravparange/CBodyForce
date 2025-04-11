
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace BodyForce.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDashboardService _dashboardservice;

        public HomeController(ILogger<HomeController> logger, IDashboardService dashboardservice)
        {
            _logger = logger;
            _dashboardservice = dashboardservice;
        }
        public async Task<IActionResult> Index()
        {
            return View( await _dashboardservice.GetDashboardCount());
        }
        public async Task<IActionResult> GetList(string category)
        {
            var data = await _dashboardservice.GetList(category);
            return PartialView("_MembershipList", data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
