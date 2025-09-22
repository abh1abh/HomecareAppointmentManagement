using Microsoft.AspNetCore.Mvc;

namespace HomecareAppointmentManagment.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}