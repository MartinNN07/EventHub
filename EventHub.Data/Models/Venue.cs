using System.ComponentModel.DataAnnotations;

namespace EventHub.Data.Models
{
	public class Venue
	{
		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Name { get; set; } = null!;

		[Required]
		[MaxLength(200)]
		public string Address { get; set; } = null!;

		public int Capacity { get; set; }

		public ICollection<Event> Events { get; set; } = new HashSet<Event>();
	}
}