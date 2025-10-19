using Microsoft.AspNetCore.Mvc;

namespace HomecareAppointmentManagment.Controllers
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