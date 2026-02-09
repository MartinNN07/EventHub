using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventHub.Data.Models
{
	public class Event
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Title { get; set; } = null!;

		[Required]
		[MaxLength(100)]
		public string Organizer { get; set; } = null!;

		[Required]
		[MaxLength(500)]
		public string Description { get; set; } = null!;

		[Required]
		public DateTime Date { get; set; }

		[Required]
		[Column(TypeName = "decimal(18,2)")]
		[Range(0.00, 10000.00, ErrorMessage = "Цената трябва да е положително число.")]
		public decimal TicketPrice { get; set; }

		public string? ImageUrl { get; set; }

		[Required]
		public int VenueId { get; set; }

		[ForeignKey(nameof(VenueId))]
		public Venue Venue { get; set; } = null!;

		public ICollection<Category> Categories { get; set; } = new HashSet<Category>();

		public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
	}
}