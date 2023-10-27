using Booking.Areas.FrontOffice.Models.Input;

namespace Booking.Areas.FrontOffice.Models.Output
{
    public class BookMyRoomResultDTO
    {
        public RoomFilterDTO? roomFilterDTO { get; set; }
        public List<RoomsDetailsDTO>? listRoomsDetailsDTO { get; set; }
	}

    public class SingleRoomDetails
    {
        public RoomsDetailsDTO? roomDetails { get; set; }
        public BookingQueryDTO? bookingQueryData { get; set; }
    }
}
