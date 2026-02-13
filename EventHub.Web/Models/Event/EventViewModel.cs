using System.ComponentModel.DataAnnotations;

namespace EventHub.Web.Models.Event
{
	public class EventViewModel
	{
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Title { get; set; } = null!;

		[Required]
		[StringLength(100)]
		public string Organizer { get; set; } = null!;

		[Required]
		[StringLength(500)]
		public string Description { get; set; } = null!;

		[Required]
		[Display(Name = "Event Date")]
		[DataType(DataType.DateTime)]
		public DateTime Date { get; set; }

		[Required]
		[Display(Name = "Ticket Price")]
		[Range(0.00, 10000.00)]
		[DataType(DataType.Currency)]
		public decimal TicketPrice { get; set; }

		[Display(Name = "Image URL")]
		[DataType(DataType.ImageUrl)]
		public string? ImageUrl { get; set; }
		public string VenueName { get; set; } = null!;
		public string VenueAddress { get; set; } = null!;
		public int VenueCapacity { get; set; }

		public List<string> CategoryNames { get; set; } = new List<string>();
		public int AvailableTickets { get; set; }
		public bool IsBookable { get; set; }
		public bool IsPastEvent => Date < DateTime.Now;
		public string FormattedDate => Date.ToString("dddd, MMMM dd, yyyy 'at' h:mm tt");
		public string FormattedPrice => TicketPrice == 0 ? "Безплатно" : $"€{TicketPrice:F2}";
	}
}
