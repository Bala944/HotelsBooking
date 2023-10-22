using Microsoft.AspNetCore.Mvc;

namespace Booking.Areas.BackOffice.Controllers
{
    [Area("BackOffice")]
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
