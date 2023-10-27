using Booking.Areas.BackOffice.Models.Input;
using Booking.Areas.FrontOffice.Models.Input;

namespace Booking.Areas.BackOffice.Models.Output
{
    public class AddRoomDTO
    {
        public  List<RoomTypeModel>?  roomTypeModel { get; set; }
        public  List<BedTypeModel>? bedTypeModel { get; set; }
    }

    
}
