using EventHub.Web.Models.Category;
using EventHub.Web.Models.Event;

namespace EventHub.Web.Models.Home
{
	public class HomeViewModel
	{
		public List<EventViewModel> FeaturedEvents { get; set; } = new List<EventViewModel>();
		public List<CategoryViewModel> PopularCategories { get; set; } = new List<CategoryViewModel>();
		public int TotalUpcomingEvents { get; set; }
		public int TotalCategories { get; set; }
	}
}
