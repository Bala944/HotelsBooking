using Booking.Areas.BackOffice.Models.Input;
using Booking.Areas.BackOffice.Models.Output;

namespace Booking.Areas.BackOffice.Data.Interface
{
    public interface IRoomsRepository
    {
         Task<List<RoomsDTO>> GetRooms();
         Task<RoomsDetailsDTO> GetRoomDetailsById(Int64 RoomId);
         Task<AddRoomDTO> GetAddRoomDetails();
         Task<int> SaveRoomDetails(RoomsDetailsDTO roomsDetailsDTO);
         Task<int> DeleteRoomDetails(Int64 RoomId);
    }
}
