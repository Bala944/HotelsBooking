using Booking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Areas.BackOffice.Controllers
{
    [Area("BackOffice")]
    [Authorize(Policy = Policies.RequireAdminClaim)]
    public class BookingController : Controller
	{
		public BookingController() 
		{

		}

        [Route("~/view-Booking")]
        public IActionResult ViewBooking()
		{
			return View();
		}
	}
}
