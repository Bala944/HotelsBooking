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
        public int MaxChild { get; set; }
        public decimal Rate { get; set; }
        public string? Description { get; set; }
        public string? RoomStatus { get; set; }
        public decimal? Discount { get; set; } = 0;
        public string? Tax { get; set; } = string.Empty;
        public string? Images { get; set; } = string.Empty;
        public string? Payment { get; set; } = string.Empty;
    }

    public class RoomFilterDTO
    {
        public string? CheckInDate { get; set; }
        public string? CheckOutDate { get; set; }
        public int? Adults { get; set; }
        public int? Children { get; set; }
        public int? Rooms { get; set; }
        public bool IsViewMore { get; set; } =false;
        public string? Params { get; set; } =string.Empty;

    }
    public class BookingQueryDTO
    {
        public string? CheckInDate { get; set; }
        public string? CheckOutDate { get; set; }
        public int? Adults { get; set; }
        public int? Children { get; set; }
        public int? Rooms { get; set; }
        public  Int64 SelectedRoomId { get; set; }

    }

    public class SelectedRoomDTO
    {
        public Int64 RoomId { get; set; }
        public bool IsViewMore { get; set; } = false;
        public string? Params { get; set; }

    }

    public class RoomRegisterDTO
    {
        public Int64 RoomId { get; set; }
        public string? TotalPrice { get; set; }

    }

    public class CustomerAndBookingDetails
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? MobileNumber { get; set; }

    }



}