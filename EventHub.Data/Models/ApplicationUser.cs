using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EventHub.Data.Models
{
	public class ApplicationUser : IdentityUser
	{
		[Required]
		[MaxLength(50)]
		public string FirstName { get; set; } = null!;

		[Required]
		[MaxLength(50)]
		public string LastName { get; set; } = null!;

		public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
	}
}