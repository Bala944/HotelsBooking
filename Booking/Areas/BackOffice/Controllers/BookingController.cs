using Booking.Areas.BackOffice.Data.Interface;
using Booking.Areas.BackOffice.Models.Input;
using Booking.Areas.BackOffice.Models.Output;
using Booking.Data;
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


        [Route("~/update-Booking-status")]
        [HttpPost]
        public async Task<IActionResult> UpdateBookingStatus([FromBody] BookingStatusDTO bookingStatusDTO)
        {
            Int16 result = await _bookingRepository.UpdateBookingStatus(bookingStatusDTO);

            MailingService mailingService = new MailingService();
            mailingService.SendEmail();
            return Ok(result);
        }
    }
}
