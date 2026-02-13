using System.ComponentModel.DataAnnotations;

namespace EventHub.Web.Models.User
{
	public class ForgotPasswordViewModel
	{
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid email address")]
		[Display(Name = "Email")]
		public string Email { get; set; } = null!;
	}
}
