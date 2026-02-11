using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventHub.Web.Models
{
	public class AdminEventViewModel
	{
		public int Id { get; set; }
		public string Title { get; set; } = null!;
		public string Organizer { get; set; } = null!;
		public DateTime Date { get; set; }
		public decimal TicketPrice { get; set; }
		public string VenueName { get; set; } = null!;
		public List<string> CategoryNames { get; set; } = new List<string>();
		public int TotalBookings { get; set; }
		public bool IsPastEvent => Date < DateTime.Now;
		public string FormattedDate => Date.ToString("MMM dd, yyyy h:mm tt");
		public string FormattedPrice => TicketPrice == 0 ? "Free" : $"${TicketPrice:F2}";
	}

	public class CreateEventViewModel
	{
		[Required(ErrorMessage = "Title is required")]
		[StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
		[Display(Name = "Event Title")]
		public string Title { get; set; } = null!;

		[Required(ErrorMessage = "Organizer is required")]
		[StringLength(100, ErrorMessage = "Organizer name cannot exceed 100 characters")]
		[Display(Name = "Organizer")]
		public string Organizer { get; set; } = null!;

		[Required(ErrorMessage = "Description is required")]
		[StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
		[Display(Name = "Description")]
		public string Description { get; set; } = null!;

		[Required(ErrorMessage = "Event date is required")]
		[Display(Name = "Event Date & Time")]
		[DataType(DataType.DateTime)]
		public DateTime Date { get; set; } = DateTime.Now.AddDays(7);

		[Required(ErrorMessage = "Ticket price is required")]
		[Range(0.00, 10000.00, ErrorMessage = "Ticket price must be between $0.00 and $10,000.00")]
		[Display(Name = "Ticket Price")]
		[DataType(DataType.Currency)]
		public decimal TicketPrice { get; set; }

		[Display(Name = "Image URL")]
		[DataType(DataType.Url)]
		[StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
		public string? ImageUrl { get; set; }

		[Required(ErrorMessage = "Venue is required")]
		[Display(Name = "Venue")]
		public int VenueId { get; set; }

		[Required(ErrorMessage = "At least one category is required")]
		[Display(Name = "Categories")]
		public List<int> CategoryIds { get; set; } = new List<int>();

		// For dropdowns
		public SelectList? Venues { get; set; }
		public List<CategoryViewModel> AvailableCategories { get; set; } = new List<CategoryViewModel>();
	}

	public class EditEventViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Title is required")]
		[StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
		[Display(Name = "Event Title")]
		public string Title { get; set; } = null!;

		[Required(ErrorMessage = "Organizer is required")]
		[StringLength(100, ErrorMessage = "Organizer name cannot exceed 100 characters")]
		[Display(Name = "Organizer")]
		public string Organizer { get; set; } = null!;

		[Required(ErrorMessage = "Description is required")]
		[StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
		[Display(Name = "Description")]
		public string Description { get; set; } = null!;

		[Required(ErrorMessage = "Event date is required")]
		[Display(Name = "Event Date & Time")]
		[DataType(DataType.DateTime)]
		public DateTime Date { get; set; }

		[Required(ErrorMessage = "Ticket price is required")]
		[Range(0.00, 10000.00, ErrorMessage = "Ticket price must be between $0.00 and $10,000.00")]
		[Display(Name = "Ticket Price")]
		[DataType(DataType.Currency)]
		public decimal TicketPrice { get; set; }

		[Display(Name = "Image URL")]
		[DataType(DataType.Url)]
		[StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
		public string? ImageUrl { get; set; }

		[Required(ErrorMessage = "Venue is required")]
		[Display(Name = "Venue")]
		public int VenueId { get; set; }

		[Required(ErrorMessage = "At least one category is required")]
		[Display(Name = "Categories")]
		public List<int> CategoryIds { get; set; } = new List<int>();

		// For dropdowns
		public SelectList? Venues { get; set; }
		public List<CategoryViewModel> AvailableCategories { get; set; } = new List<CategoryViewModel>();

		public int TotalBookings { get; set; }
	}
}
