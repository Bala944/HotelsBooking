using Booking.Areas.BackOffice.Models.Input;
using Booking.Areas.BackOffice.Models.Output;
using Booking.Areas.FrontOffice.Models.Input;

namespace Booking.Areas.BackOffice.Data.Interface
{
    public interface IBookingRepository
    {
         Task<List<BookingDTO>> GetBookings();
         Task<Int16> UpdateBookingStatus(BookingStatusDTO bookingStatusDTO);
         Task<FinalConfirmationData> GetConfirmStatus(long BookingId);
        
    }
}
