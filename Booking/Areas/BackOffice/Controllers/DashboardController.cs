using Microsoft.AspNetCore.Mvc;

namespace Booking.Areas.BackOffice.Controllers
{
    [Area("BackOffice")]
    public class DashboardController : Controller
    {

        public DashboardController() {
        }

        [Route("~/dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
