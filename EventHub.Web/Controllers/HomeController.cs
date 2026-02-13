using EventHub.Services.Interfaces;
using EventHub.Web.Extensions;
using EventHub.Web.Models.Event;
using EventHub.Web.Models.Home;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly IEventService _eventService;
		private readonly ICategoryService _categoryService;
		private readonly ILogger<HomeController> _logger;

		public HomeController(
			IEventService eventService,
			ICategoryService categoryService,
			ILogger<HomeController> logger)
		{
			_eventService = eventService;
			_categoryService = categoryService;
			_logger = logger;
		}

		public async Task<IActionResult> Index()
		{
			try
			{
				var featuredEvents = await _eventService.GetFeaturedEventsAsync(6);
				var categories = await _categoryService.GetAllCategoriesAsync();
				var upcomingEvents = await _eventService.GetUpcomingEventsAsync();

				var viewModel = new HomeViewModel
				{
					FeaturedEvents = new List<EventViewModel>(),
					PopularCategories = categories.Select(c => c.ToViewModel()).ToList(),
					TotalUpcomingEvents = upcomingEvents.Count(),
					TotalCategories = categories.Count()
				};

				foreach (var eventItem in featuredEvents)
				{
					var availableTickets = await _eventService.GetAvailableTicketsAsync(eventItem.Id);
					var isBookable = await _eventService.IsEventBookableAsync(eventItem.Id);
					viewModel.FeaturedEvents.Add(eventItem.ToViewModel(availableTickets, isBookable));
				}

				return View(viewModel);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading home page");
				return View("Error");
			}
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View();
		}
	}
}
