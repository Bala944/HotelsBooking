using Booking.Areas.BackOffice.Models.Input;
using Booking.Areas.BackOffice.Models.Output;

namespace Booking.Areas.BackOffice.Data.Interface
{
    public interface IBookingRepository
    {
         Task<List<BookingDTO>> GetBookings();
        
    }
}
