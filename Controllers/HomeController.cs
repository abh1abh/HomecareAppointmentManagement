using Microsoft.AspNetCore.Mvc;

namespace HomecareAppointmentManagement.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index() // Home page
        {
            return View();
        }
    }
}