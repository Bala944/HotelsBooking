using Microsoft.VisualBasic;

namespace Booking.Areas.FrontOffice.Models.Input
{
    public class RoomandEventDetailsDTO
    {
        public List<RoomsDetailsDTO > RoomDetails { get; set; }
        public List<Event> events {  get; set; }
        public RoomFilterDTO roomFilterDTO { get; set; }
    }
    public class Feedback
    {
        public required string BookId  { get; set; }
        public long? BookingID { get; set; }
        public int ApplicationRating { get; set; }
        public int HotelRating { get; set; }
        public string? Comment { get; set; }
    }



    public class Event
    {
        public long? EventID { get; set; }
        public string? EventType { get; set; }
        public string? Description { get; set; }
        public string? Images { get; set; }
        public long TotalSeats { get; set; }
        public decimal? Price { get; set; }
        public long? SoldSeats { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public bool? IsActive { get; set; }
        public List<IFormFile>? FileImages { get; set; }
    }

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
        public int? TotalReservedQuantity { get; set; } = 0;
    }

    public class RoomFilterDTO
    {
        public string? CheckInDate { get; set; }
        public string? CheckOutDate { get; set; }
        public int? Adults { get; set; }
        public int? Children { get; set; }
        public int? Rooms { get; set; }
        public int? RoomType { get; set; }
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
        public int? RoomType { get; set; }
        public  Int64 SelectedRoomId { get; set; }

    }

    public class SelectedRoomDTO
    {
        public Int64 RoomId { get; set; }
        public bool IsViewMore { get; set; } = false;
        public string? Params { get; set; }
        public string? CheckInDate { get; set; }
        public string? CheckOutDate { get; set; }

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
        public  string paymentId { get; set; }
        public  string Orderid { get; set; }
        public  string sign { get; set; }

    }

   

        public class FinalBookingDetailsDTO
    {
        public required List<FinalBookingDetails> finalBookingDetails { get; set; }
        public  string Events { get; set; }
        public  string FilterParams { get; set; }
    }

    public class BookingRegistrationDTO
    {
        public FinalConfirmationData? finalBookingDetails { get; set; }
        public BookingQueryDTO? bookingQueryDTO { get; set; }
    }
    public class FinalBookingDetails
    {
        public Int64? RoomId { get; set; }
        public Int64? Count { get; set; }
        public decimal? Amount { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? Name { get; set; }

    }
   

    public class FinalConfirmationData
    {
        public List<RoomConfirmationDetailsDTO> roomConfirmationDetailsDTO { get; set; }
        public  List<EventConfirmationDetailsDTO> eventConfirmationDetailsDTO { get; set; }
	}

	public class RoomConfirmationDetailsDTO
	{
		public long? RoomId { get; set; }
		public string? Name { get; set; }
		public long? Count { get; set; }
		public long? DiscountId { get; set; }
		public decimal? Amount { get; set; }
		public decimal? TotalAmount { get; set; }
		public decimal? DiscountAmount { get; set; }
		public string? EmailId { get; set; }
		public string? OwnerEmailId { get; set; }
		public string? Phone { get; set; }
		public string? OrderId { get; set; }

	}

	public class EventConfirmationDetailsDTO
	{
		public long? EventId { get; set; }
		public string? Name { get; set; }
		public long? Count { get; set; }
		public decimal? Amount { get; set; }
		public decimal? TotalAmount { get; set; }

	}

    public class BookingSelectedDTO
    {
        public long? RoomId { get; set; }
        public long Count { get; set; }
        public string? EventIds { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
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
        public required int TotalCount { get; set; }
        public required string CheckIn { get; set; }
        public required string? CheckOut { get; set; }
        public  long? DiscountId { get; set; }
        public  decimal? DiscountAmount { get; set; }

        public  string? EventId { get; set; }
        public  string? EventCount { get; set; }
        public  string? EventAmount { get; set; }


    }

    public class OrderDTO
    {
        public string BookingStatus { get; set; }
        public string BookingOrderId { get; set; }
    }

    public class EventDTO
    {
        public long? EventID { get; set; }
        public string? EventType { get; set; }
        public string? Description { get; set; }
        public string? Images { get; set; }
        public decimal? Price { get; set; }
        public long? SoldSeats { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
    }
}