using System.ComponentModel.DataAnnotations;

namespace Booking.Areas.BackOffice.Models.Output
{
    public class OrderDiscount
    {
        public Int64 DiscountID { get; set; }

        public  Int64 RoomId { get; set; }
        public  string? RoomName { get; set; }

        public decimal DiscountPercentage { get; set; }

        public string? StartDate { get; set; }

        public string? ExpirationDate { get; set; }
    }

    public class RoomDTO
    {
        public Int64 RoomId { get; set; }
        public string? RoomName { get; set; }

    }
}
