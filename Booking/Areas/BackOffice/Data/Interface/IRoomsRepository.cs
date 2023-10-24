using Booking.Areas.BackOffice.Models.Input;
using Booking.Areas.BackOffice.Models.Output;

namespace Booking.Areas.BackOffice.Data.Interface
{
    public interface IRoomsRepository
    {
         Task<List<RoomsDTO>> GetRooms();
         Task<AddRoomDTO> GetAddRoomDetails();
         Task<int> SaveRoomDetails(RoomsDetailsDTO roomsDetailsDTO);
         Task<int> DeleteRoomDetails(int RoomId);
    }
}
