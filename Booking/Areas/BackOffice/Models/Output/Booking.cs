namespace Booking.Areas.BackOffice.Models.Output
{
    public class BookingDTO
    {
            public string RoomName { get; set; }
            public string RoomType { get; set; }
            public int   Quantity { get; set; }
            public string CheckInDate { get; set; }
            public string CheckOutDate { get; set; }
            public string Price { get; set; }
            public string BookingDate { get; set; }
            public string CustomerName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Status { get; set; }
        
    }
}
