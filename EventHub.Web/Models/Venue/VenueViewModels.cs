using System.ComponentModel.DataAnnotations;

namespace EventHub.Web.Models.Venue
{
	public class VenueViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string Address { get; set; } = null!;
		public int Capacity { get; set; }
		public int EventCount { get; set; }
	}

	public class CreateVenueViewModel
	{
		[Required(ErrorMessage = "Venue name is required")]
		[StringLength(100, ErrorMessage = "Venue name cannot exceed 100 characters")]
		[Display(Name = "Venue Name")]
		public string Name { get; set; } = null!;

		[Required(ErrorMessage = "Address is required")]
		[StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
		[Display(Name = "Address")]
		public string Address { get; set; } = null!;

		[Required(ErrorMessage = "Capacity is required")]
		[Range(1, 100000, ErrorMessage = "Capacity must be between 1 and 100,000")]
		[Display(Name = "Capacity")]
		public int Capacity { get; set; }
	}

	public class EditVenueViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Venue name is required")]
		[StringLength(100, ErrorMessage = "Venue name cannot exceed 100 characters")]
		[Display(Name = "Venue Name")]
		public string Name { get; set; } = null!;

		[Required(ErrorMessage = "Address is required")]
		[StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
		[Display(Name = "Address")]
		public string Address { get; set; } = null!;

		[Required(ErrorMessage = "Capacity is required")]
		[Range(1, 100000, ErrorMessage = "Capacity must be between 1 and 100,000")]
		[Display(Name = "Capacity")]
		public int Capacity { get; set; }

		public int EventCount { get; set; }
	}
}
