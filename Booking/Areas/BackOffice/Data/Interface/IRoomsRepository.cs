using Booking.Areas.BackOffice.Models;

namespace Booking.Areas.BackOffice.Data.Interface
{
    public interface IRoomsRepository
    {
        public Task<List<RoomsDTO>> GetRooms();
    }
}
