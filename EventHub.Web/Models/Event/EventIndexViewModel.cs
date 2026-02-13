using EventHub.Web.Models.Category;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventHub.Web.Models.Event
{
	public class EventIndexViewModel
	{
		public List<EventViewModel> Events { get; set; } = new List<EventViewModel>();
		public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
		public string? SearchTerm { get; set; }
		public int? CategoryId { get; set; }
		public bool UpcomingOnly { get; set; } = true;
		public SelectList? CategoriesSelectList { get; set; }
		public int TotalEvents => Events.Count;
		public bool HasResults => Events.Any();
		public string ResultsMessage
		{
			get
			{
				if (!HasResults)
					return "No events found.";

				var filter = string.IsNullOrEmpty(SearchTerm) ? "" : $" matching '{SearchTerm}'";
				return $"{TotalEvents} event{(TotalEvents != 1 ? "s" : "")} found{filter}";
			}
		}
	}
}
