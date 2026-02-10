using System.ComponentModel.DataAnnotations;

namespace EventHub.Web.Models
{
	public class CreateBookingViewModel
	{
		public int EventId { get; set; }
		
		[Required(ErrorMessage = "Number of tickets is required")]
		[Range(1, 20, ErrorMessage = "You can book between 1 and 20 tickets")]
		[Display(Name = "Number of Tickets")]
		public int TicketsCount { get; set; } = 1;
		
		// Event details for display
		public string EventTitle { get; set; } = null!;
		public string EventOrganizer { get; set; } = null!;
		public string EventDescription { get; set; } = null!;
		public DateTime EventDate { get; set; }
		public decimal TicketPrice { get; set; }
		public string? EventImageUrl { get; set; }
		
		// Venue details
		public string VenueName { get; set; } = null!;
		public string VenueAddress { get; set; } = null!;
		public int VenueCapacity { get; set; }
		
		// Availability
		public int AvailableTickets { get; set; }
		public int TotalBookedTickets { get; set; }
		
		// Calculated properties
		public decimal TotalPrice => TicketsCount * TicketPrice;
		public bool IsAvailable => AvailableTickets > 0;
		public bool IsEventPassed => EventDate < DateTime.Now;
	}
}
