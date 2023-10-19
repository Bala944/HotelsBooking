namespace Booking.Areas.BackOffice.Models.Input
{
    public class RoomsDTO
    {
        public long RoomId { get; set; }
        public required string RoomNumber { get; set; }
        public required string RoomType { get; set; }
        public required string BedType { get; set; }
        public int MaxOccupancy { get; set; }
        public decimal Rate { get; set; }
        public required string Description { get; set; }
        public required string RoomStatus { get; set; }
    }

    public class RoomTypeModel
    {
        public int RoomTypeId { get; set; }
        public required string TypeName { get; set; }
    }

    public class BedTypeModel
    {
        public int BedTypeId { get; set; }
        public required string TypeName { get; set; }
    }
}
