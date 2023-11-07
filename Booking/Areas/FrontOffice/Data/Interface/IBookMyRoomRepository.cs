using Booking.Areas.FrontOffice.Models.Input;

namespace Booking.Areas.FrontOffice.Data.Interface
{
    public interface IBookMyRoomRepository
    {
        Task<List<RoomsDetailsDTO>> GetRooms(RoomFilterDTO roomFilterDTO);
        Task<RoomsDetailsDTO> GetRoomsById(Int64 roomId);
        Task<string> ConfirmBooking(RegistrationDetails registrationDetails);
    }
}
