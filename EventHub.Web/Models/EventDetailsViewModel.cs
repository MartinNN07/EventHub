namespace EventHub.Web.Models
{
	public class EventDetailsViewModel
	{
		public int Id { get; set; }
		public string Title { get; set; } = null!;
		public string Organizer { get; set; } = null!;
		public string Description { get; set; } = null!;
		public DateTime Date { get; set; }
		public decimal TicketPrice { get; set; }
		public string? ImageUrl { get; set; }
		public int VenueId { get; set; }
		public string VenueName { get; set; } = null!;
		public string VenueAddress { get; set; } = null!;
		public int VenueCapacity { get; set; }
		public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
		public int AvailableTickets { get; set; }
		public int TotalBookedTickets { get; set; }
		public bool IsBookable { get; set; }
		public bool IsPastEvent => Date < DateTime.Now;
		public string FormattedDate => Date.ToString("dddd, MMMM dd, yyyy");
		public string FormattedTime => Date.ToString("h:mm tt");
		public string FormattedPrice => TicketPrice == 0 ? "Free" : $"${TicketPrice:F2}";
		public int BookingPercentage => VenueCapacity > 0 
			? (int)((double)TotalBookedTickets / VenueCapacity * 100) 
			: 0;
	}
}
