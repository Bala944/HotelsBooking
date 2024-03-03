namespace Booking.Models
{
	public class AuthDTO
	{
		public required string UserName { get; set; }
		public required string Password { get; set; }
	}

	public class MailDetailsDTO
	{
		public int MailType { get; set; }
		public string Email { get; set; }
		public string Subject { get; set; }
		public string? Body { get; set; }
		public string Content { get; set; }
	}
}
