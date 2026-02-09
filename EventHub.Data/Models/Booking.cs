using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventHub.Data.Models
{
	public class Booking
	{
		[Key]
		public int Id { get; set; }

		public DateTime BookingDate { get; set; }

		public int TicketsCount { get; set; }

		public string BuyerId { get; set; } = null!;
		[ForeignKey(nameof(BuyerId))]
		public ApplicationUser Buyer { get; set; } = null!;

		public int EventId { get; set; }
		[ForeignKey(nameof(EventId))]
		public Event Event { get; set; } = null!;
	}
}