using Booking.Areas.BackOffice.Data.Interface;
using Booking.Areas.BackOffice.Models.Output;
using Booking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Areas.BackOffice.Controllers
{
    [Area("BackOffice")]
    [Authorize(Policy = Policies.RequireAdminClaim)]
    public class BookingController : Controller
	{
		private IBookingRepository _bookingRepository;
		public BookingController(IBookingRepository bookingRepository) 
		{
			_bookingRepository = bookingRepository;

        }

        [Route("~/view-Booking")]
        public async Task<IActionResult> ViewBooking()
		{
            List<BookingDTO> bookings = new List<BookingDTO>();
            bookings = await _bookingRepository.GetBookings();
			return View(bookings);
		}
	}
}
