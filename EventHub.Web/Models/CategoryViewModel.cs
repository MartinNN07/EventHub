namespace EventHub.Web.Models
{
	public class CategoryViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public int EventCount { get; set; }
	}
}
