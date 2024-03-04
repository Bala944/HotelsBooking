using Booking.Areas.FrontOffice.Models.Input;

namespace Booking.Areas.FrontOffice.Data.Interface
{
    public interface IBookMyRoomRepository
    {
        Task<List<RoomsDetailsDTO>> GetRooms(RoomFilterDTO roomFilterDTO);
        Task<RoomandEventDetailsDTO> GetRoomsanEvents(RoomFilterDTO roomFilterDTO);
        Task<RoomsDetailsDTO> GetRoomsById(BookingQueryDTO bookingQueryDTO);
        Task<string> ConfirmBooking(RegistrationDetails registrationDetails);
        Task<EventDTO> GetEventDetailsById(long EventId);
        Task<FinalConfirmationData> GetRoomConfirmationDetails(BookingSelectedDTO bookingSelectedDTO);
    }
}
