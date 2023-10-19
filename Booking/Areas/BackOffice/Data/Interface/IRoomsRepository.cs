using Booking.Areas.BackOffice.Models.Input;
using Booking.Areas.BackOffice.Models.Output;

namespace Booking.Areas.BackOffice.Data.Interface
{
    public interface IRoomsRepository
    {
        public Task<List<RoomsDTO>> GetRooms();
        public Task<AddRoomDTO> GetAddRoomDetails();
    }
}
