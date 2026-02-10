using System.ComponentModel.DataAnnotations;

namespace EventHub.Web.Models
{
	public class UserProfileViewModel
	{
		public string UserId { get; set; } = null!;

		[Required(ErrorMessage = "First name is required")]
		[StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
		[Display(Name = "First Name")]
		public string FirstName { get; set; } = null!;

		[Required(ErrorMessage = "Last name is required")]
		[StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; } = null!;

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid email address")]
		[Display(Name = "Email")]
		public string Email { get; set; } = null!;

		[Display(Name = "Phone Number")]
		[Phone(ErrorMessage = "Invalid phone number")]
		public string? PhoneNumber { get; set; }

		public int TotalBookings { get; set; }
		public int UpcomingEvents { get; set; }
		public DateTime? MemberSince { get; set; }
	}
}
