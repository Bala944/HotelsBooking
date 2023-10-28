using Booking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Areas.BackOffice.Controllers
{
    [Area("BackOffice")]
    [Authorize(Policy = Policies.RequireAdminClaim)]
    public class DiscountController : Controller
    {

        [Route("~/view-discount")]
        
        public IActionResult ViewDiscount()
        {
            return View();
        }

        public IActionResult AddDiscount()
        {
            return View();
        }
    }
}
