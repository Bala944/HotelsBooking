using System.ComponentModel.DataAnnotations;

namespace Booking.Areas.BackOffice.Models.Output
{
    public class OrderDiscount
    {
        public int DiscountID { get; set; }

        [Required]
        public  Int64 RoomId { get; set; }
        public  string RoomName { get; set; }

        [Display(Name = "Discount Percentage")]
        public decimal DiscountPercentage { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Expiration Date")]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }
    }
}
