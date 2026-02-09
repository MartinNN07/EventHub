using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace EventHub.Data.Models
{
	public class Category
	{
		public int Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string Name { get; set; } = null!;
		public ICollection<Event> Events { get; set; } = new HashSet<Event>();
	}
}