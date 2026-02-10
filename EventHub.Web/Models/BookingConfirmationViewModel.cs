namespace EventHub.Web.Models
{
	public class BookingConfirmationViewModel
	{
		public int BookingId { get; set; }
		public DateTime BookingDate { get; set; }
		public int TicketsCount { get; set; }
		public decimal TotalPrice { get; set; }
		public int EventId { get; set; }
		public string EventTitle { get; set; } = null!;
		public string EventOrganizer { get; set; } = null!;
		public DateTime EventDate { get; set; }
		public string? EventImageUrl { get; set; }
		public string VenueName { get; set; } = null!;
		public string VenueAddress { get; set; } = null!;
		public string ConfirmationNumber { get; set; } = null!;
		public string UserEmail { get; set; } = null!;
	}
}
