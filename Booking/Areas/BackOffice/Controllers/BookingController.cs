using Booking.Areas.BackOffice.Data.Interface;
using Booking.Areas.BackOffice.Models.Input;
using Booking.Areas.BackOffice.Models.Output;
using Booking.Areas.FrontOffice.Models.Input;
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
		private IMailing _mailing;
		public BookingController(IBookingRepository bookingRepository, IMailing mailing) 
		{
			_bookingRepository = bookingRepository;
            _mailing = mailing;

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

            FinalConfirmationData bookingDetailsDTO = new FinalConfirmationData();

            bookingDetailsDTO = await _bookingRepository.GetConfirmStatus(Convert.ToInt64(EncryptionHelper.Decrypt(bookingStatusDTO.BookingId)));

            List<MailDetailsDTO> mailDetails = new List<MailDetailsDTO>();

			mailDetails = await _mailing.GetMailDetails(3);

            MailingService mailingService = new MailingService();


            for (int i = 0; i < mailDetails.Count; i++)
            {
                string Booking = "<table style='width:100%;'>";
                decimal? totalAmount = 0;

                for (int j = 0; j < bookingDetailsDTO.roomConfirmationDetailsDTO.Count; j++)
                {
                    Booking += $"<tr><td style='padding: 8px;'>{bookingDetailsDTO.roomConfirmationDetailsDTO[j].Name}</td><td style='padding: 8px;'>X {bookingDetailsDTO.roomConfirmationDetailsDTO[j].Count}</td><td style='padding: 8px;'>   {bookingDetailsDTO.roomConfirmationDetailsDTO[j].Amount}</td></tr>";
                    totalAmount += bookingDetailsDTO.roomConfirmationDetailsDTO[j].Amount;
                }

                if (bookingDetailsDTO.eventConfirmationDetailsDTO != null)
                {
                    for (int j = 0; j < bookingDetailsDTO.eventConfirmationDetailsDTO.Count; j++)
                    {
                        Booking += $"<tr><td style='padding: 8px;'>             {bookingDetailsDTO.eventConfirmationDetailsDTO[j].Name}</td><td style='padding: 8px;'></td><td style='padding: 8px;'>   {bookingDetailsDTO.eventConfirmationDetailsDTO[j].Amount}</td></tr>";
                        totalAmount += bookingDetailsDTO.eventConfirmationDetailsDTO[j].Amount;
                    }
                }

                Booking += $"<tfoot><tr><td colspan='2' style='text-align:right;padding: 8px;'>Total Amount:</td><td style='padding: 8px;'>{totalAmount}</td></tr></tfoot>";
                Booking += "</table>";
                string formattedHtmlContent = string.Empty;
                string Email = null;
                if (mailDetails[i].MailType == 3)
                {
                    // Get the current application domain
                    AppDomain domain = AppDomain.CurrentDomain;
                    // Get the base directory for the domain
                    string baseDirectory = domain.BaseDirectory;

                    formattedHtmlContent = string.Format(mailDetails[i].Content,$"{baseDirectory}/attachments/logo/logopng.png", bookingDetailsDTO.roomConfirmationDetailsDTO[0].OrderId, Booking, $"{baseDirectory}/feedback/BookingId={bookingStatusDTO.BookingId}");
                    Email = bookingDetailsDTO.roomConfirmationDetailsDTO[0].EmailId;
                }
                else if (mailDetails[i].MailType == 5)
                {
                    string Booking1 = "<table style='width:100%;'>";


                    for (int j = 0; j < bookingDetailsDTO.roomConfirmationDetailsDTO.Count; j++)
                    {
                        Booking1 += $"<tr><td style='padding: 8px;'>{bookingDetailsDTO.roomConfirmationDetailsDTO[j].Name}</td><td style='padding: 8px;'>X {bookingDetailsDTO.roomConfirmationDetailsDTO[j].Count}</td><td style='padding: 8px;'>  </td></tr>";

                    }

                    if (bookingDetailsDTO.eventConfirmationDetailsDTO != null)
                    {
                        for (int j = 0; j < bookingDetailsDTO.eventConfirmationDetailsDTO.Count; j++)
                        {
                            Booking1 += $"<tr><td style='padding: 8px;'>             {bookingDetailsDTO.eventConfirmationDetailsDTO[j].Name}</td><td style='padding: 8px;'></td><td style='padding: 8px;'> </td></tr>";
                        }
                    }

                    Booking1 += $"<tfoot><tr><td colspan='2' style='text-align:right;padding: 8px;'></td><td style='padding: 8px;'></td></tr></tfoot>";
                    Booking1 += "</table>";

                    formattedHtmlContent = string.Format(mailDetails[i].Content,"", bookingDetailsDTO.roomConfirmationDetailsDTO[0].OrderId, Booking1);
                    Email = bookingDetailsDTO.roomConfirmationDetailsDTO[0].OwnerEmailId;
                }
                

                mailingService.SendEmail(mailDetails[i].Subject, formattedHtmlContent, Email);
            }
           

            return Ok(result);
        }
    }
}
