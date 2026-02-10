namespace EventHub.Web.Models
{
	public class BookingViewModel
	{
		public int Id { get; set; }
		public DateTime BookingDate { get; set; }
		public int TicketsCount { get; set; }
		public decimal TotalPrice { get; set; }
		
		// Event details
		public int EventId { get; set; }
		public string EventTitle { get; set; } = null!;
		public string EventOrganizer { get; set; } = null!;
		public DateTime EventDate { get; set; }
		public decimal TicketPrice { get; set; }
		public string? EventImageUrl { get; set; }
		
		// Venue details
		public string VenueName { get; set; } = null!;
		public string VenueAddress { get; set; } = null!;
		
		// Status
		public bool CanCancel { get; set; }
		public string StatusBadgeClass { get; set; } = "bg-success";
		public string StatusText { get; set; } = "Confirmed";
	}
}
