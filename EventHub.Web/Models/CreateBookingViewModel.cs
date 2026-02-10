using System.ComponentModel.DataAnnotations;

namespace EventHub.Web.Models
{
	public class CreateBookingViewModel
	{
		public int EventId { get; set; }
		public string EventTitle { get; set; } = null!;
		public DateTime EventDate { get; set; }
		public decimal TicketPrice { get; set; }
		public int AvailableTickets { get; set; }

		[Required]
		[Range(1, 100, ErrorMessage = "Please select between 1 and 100 tickets")]
		[Display(Name = "Number of Tickets")]
		public int TicketsCount { get; set; } = 1;
		public decimal TotalPrice => TicketPrice * TicketsCount;
		public string FormattedTotalPrice => TotalPrice == 0 ? "Free" : $"${TotalPrice:F2}";
	}
}
