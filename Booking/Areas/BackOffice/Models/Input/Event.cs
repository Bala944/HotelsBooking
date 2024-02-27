namespace Booking.Areas.BackOffice.Models.Input
{
    public class Event
    {
        public long? EventID { get; set; }
        public string? EventType { get; set; }
        public string? Description { get; set; }
        public string? Images { get; set; }
        public long TotalSeats { get; set; }
        public decimal? Price { get; set; }
        public long? SoldSeats { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public bool? IsActive { get; set; }
        public List<IFormFile>? FileImages { get; set; }
    }
}
