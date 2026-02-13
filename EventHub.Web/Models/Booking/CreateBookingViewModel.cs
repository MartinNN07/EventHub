using System.ComponentModel.DataAnnotations;

namespace EventHub.Web.Models.Booking
{
	public class CreateBookingViewModel
	{
		public int EventId { get; set; }

		public string? EventTitle { get; set; }
		public string? EventOrganizer { get; set; }
		public string? EventDescription { get; set; }
		public DateTime EventDate { get; set; }
		public decimal TicketPrice { get; set; }
		public string? EventImageUrl { get; set; }

		public string? VenueName { get; set; }
		public string? VenueAddress { get; set; }
		public int VenueCapacity { get; set; }

		public int AvailableTickets { get; set; }
		public int TotalBookedTickets { get; set; }

		[Required]
		[Range(1, 20, ErrorMessage = "Please select between 1 and 20 tickets")]
		[Display(Name = "Number of Tickets")]
		public int TicketsCount { get; set; } = 1;

		public decimal TotalPrice => TicketPrice * TicketsCount;
		public string FormattedTotalPrice => TotalPrice == 0 ? "Free" : $"${TotalPrice:F2}";
		public bool IsAvailable => AvailableTickets > 0;
		public bool IsEventPassed => EventDate < DateTime.Now;
	}
}