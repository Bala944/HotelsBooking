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
        public decimal? DiscountPercentage { get; set; } = 0;
        public Int64 DiscountId { get; set; } = 0;
        public string? Tax { get; set; } = string.Empty;
        public string? Images { get; set; } = string.Empty;
        public string? Payment { get; set; } = string.Empty;
        public int? AvailableQuantity { get; set; } = 0;
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
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string EmailAddress { get; set; }
        public required string MobileNumber { get; set; }
        public required string CheckIn { get; set; }
        public required string CheckOut { get; set; }
        public required string BookingParams { get; set; }

    }

   

        public class FinalBookingDetailsDTO
    {
        public required List<FinalBookingDetails> finalBookingDetails { get; set; }
    }

    public class BookingRegistrationDTO
    {
        public List<FinalBookingDetails>? finalBookingDetails { get; set; }
        public BookingQueryDTO? bookingQueryDTO { get; set; }
    }
    public class FinalBookingDetails
    {
        public Int64? RoomId { get; set; }
        public Int64? Count { get; set; }
        public decimal? Amount { get; set; }
        public string? Name { get; set; }

    }

    public class RegistrationDetails
    {
        public required string FirstName { get; set; }
        public required  string LastName { get; set; }
        public required string EmailAddress { get; set; }
        public required string MobileNumber { get; set; }
        public required string RoomId { get; set; }
        public required string Count { get; set; }
        public required string Amount { get; set; }
        public required decimal TotalAmount { get; set; }
        public required string CheckIn { get; set; }
        public required string? CheckOut { get; set; }


    }

}