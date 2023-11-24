namespace Booking.Areas.BackOffice.Models.Input
{
    public class BookingStatusDTO
    {
        public required string BookingId { get; set; }
        public int BookingStatus { get; set; }
    }
}
