using Microsoft.VisualBasic;

namespace Booking.Areas.FrontOffice.Models.Input
{
    public class RoomsDetailsDTO
    {
        public long RoomId { get; set; }
        public string? RoomNumber { get; set; }
        public string? RoomType { get; set; }
        public string? BedType { get; set; }
        public string? CancelationCharge { get; set; }
        public int MaxOccupancy { get; set; }
        public decimal Rate { get; set; }
        public string? Description { get; set; }
        public string? RoomStatus { get; set; }
        public decimal? Discount { get; set; } = 0;
        public string? Tax { get; set; } = string.Empty;
        public string? Images { get; set; } = string.Empty;
    }

    public class RoomFilterDTO
    {
        public DateAndTime? CheckInDate { get; set; }
        public DateAndTime? CheckOutDate { get; set; }
        public int? Adults { get; set; }
        public int Children { get; set; }
        public int Rooms { get; set; }
        public bool IsViewMore { get; set; } =false;

    }




}