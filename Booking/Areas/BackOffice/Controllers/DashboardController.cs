using Booking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Areas.BackOffice.Controllers
{
    [Area("BackOffice")]
    [Authorize(Policy = Policies.RequireAdminClaim)]
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
