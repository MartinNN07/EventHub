using System.ComponentModel.DataAnnotations;

namespace EventHub.Web.Models
{
	public class ChangePasswordViewModel
	{
		[Required(ErrorMessage = "Current password is required")]
		[DataType(DataType.Password)]
		[Display(Name = "Current Password")]
		public string CurrentPassword { get; set; } = null!;

		[Required(ErrorMessage = "New password is required")]
		[StringLength(100, ErrorMessage = "Password must be at least {2} characters long", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "New Password")]
		public string NewPassword { get; set; } = null!;

		[Required(ErrorMessage = "Please confirm your new password")]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm New Password")]
		[Compare("NewPassword", ErrorMessage = "Passwords do not match")]
		public string ConfirmPassword { get; set; } = null!;
	}
}
