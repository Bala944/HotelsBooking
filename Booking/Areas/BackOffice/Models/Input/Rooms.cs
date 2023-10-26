namespace Booking.Areas.BackOffice.Models.Input
{
    public class RoomsDTO
    {
        public long RoomId { get; set; }
        public required string RoomNumber { get; set; }
        public  string? RoomType { get; set; }
        public  string? BedType { get; set; }
        public string? CancelationCharge { get; set; }
        public int MaxOccupancy { get; set; }
        public decimal Rate { get; set; }
        public string? Description { get; set; }
        public  string? RoomStatus { get; set; }
        public int payment { get; set; }
    }

    public class RoomsDetailsDTO
    {
        public long RoomId { get; set; } = 0;
        public  string? RoomNumber { get; set; }
        public  int BedTypeId { get; set; }
        public  int RoomTypeId { get; set; }
        public int CancelationCharge { get; set; }
        public int MaxOccupancy { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? Images { get; set; }
        public List<IFormFile>? FileImages { get; set; }
        public int? Status { get; set; }
        public int? IsActive { get;  set; }
        public int? Payment { get;  set; }
       
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
