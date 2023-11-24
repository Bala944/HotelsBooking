using Booking.Models;

namespace Booking.Areas.BackOffice.Models.Output
{
    public class BookingDTO
    {
            public string BookingOrderId { get; set; }
            public string RoomName { get; set; }
            public long BookingId { get; set; }
            public string RoomType { get; set; }
            public int   Quantity { get; set; }
            public string CheckInDate { get; set; }
            public string CheckOutDate { get; set; }
            public string Price { get; set; }
            public string BookingDate { get; set; }
            public string CustomerName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public int Status { get; set; }

        public string EnBookingId
        {
            get
            {
                return EncryptionHelper.Encrypt(BookingId.ToString());
            }
        }
        
    }
}
