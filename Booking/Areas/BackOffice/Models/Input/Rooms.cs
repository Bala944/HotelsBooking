namespace Booking.Areas.BackOffice.Models.Input
{
    public class RoomsDTO
    {
        public long RoomId { get; set; }
        public required string RoomNumber { get; set; }
        public required string RoomType { get; set; }
        public required string BedType { get; set; }
        public string? CancelationCharge { get; set; }
        public int MaxOccupancy { get; set; }
        public decimal Rate { get; set; }
        public string? Description { get; set; }
        public  int RoomStatus { get; set; }
    }

    public class RoomsDetailsDTO
    {
        public long RoomId { get; set; } = 0;
        public required string RoomNumber { get; set; }
        public required int BedTypeId { get; set; }
        public required int RoomTypeId { get; set; }
        public int CancelationCharge { get; set; }
        public int MaxOccupancy { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public int? IsActive { get;  set; }
       
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
