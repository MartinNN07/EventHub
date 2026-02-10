using EventHub.Services.Interfaces;
using EventHub.Web.Extensions;
using EventHub.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventHub.Web.Controllers
{
	public class EventsController : Controller
	{
		private readonly IEventService _eventService;
		private readonly ICategoryService _categoryService;
		private readonly ILogger<EventsController> _logger;

		public EventsController(
			IEventService eventService,
			ICategoryService categoryService,
			ILogger<EventsController> logger)
		{
			_eventService = eventService;
			_categoryService = categoryService;
			_logger = logger;
		}

		public async Task<IActionResult> Index(string? searchTerm, int? categoryId, bool upcomingOnly = true)
		{
			try
			{
				IEnumerable<Data.Models.Event> events;

				if (!string.IsNullOrWhiteSpace(searchTerm))
				{
					events = await _eventService.SearchEventsAsync(searchTerm);
				}
				else if (categoryId.HasValue)
				{
					events = await _eventService.GetEventsByCategoryAsync(categoryId.Value);
				}
				else if (upcomingOnly)
				{
					events = await _eventService.GetUpcomingEventsAsync();
				}
				else
				{
					events = await _eventService.GetAllEventsAsync();
				}
				var categories = await _categoryService.GetAllCategoriesAsync();

				var viewModel = new EventIndexViewModel
				{
					SearchTerm = searchTerm,
					CategoryId = categoryId,
					UpcomingOnly = upcomingOnly,
					Categories = categories.Select(c => c.ToViewModel()).ToList(),
					CategoriesSelectList = new SelectList(categories, "Id", "Name", categoryId),
					Events = new List<EventViewModel>()
				};

				foreach (var eventItem in events)
				{
					var availableTickets = await _eventService.GetAvailableTicketsAsync(eventItem.Id);
					var isBookable = await _eventService.IsEventBookableAsync(eventItem.Id);
					viewModel.Events.Add(eventItem.ToViewModel(availableTickets, isBookable));
				}

				return View(viewModel);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading events");
				TempData["Error"] = "Unable to load events. Please try again.";
				return View(new EventIndexViewModel());
			}
		}
		public async Task<IActionResult> Details(int id)
		{
			try
			{
				var eventItem = await _eventService.GetEventByIdAsync(id);
				if (eventItem == null)
				{
					TempData["Error"] = "Event not found.";
					return RedirectToAction(nameof(Index));
				}

				var availableTickets = await _eventService.GetAvailableTicketsAsync(id);
				var isBookable = await _eventService.IsEventBookableAsync(id);

				var viewModel = eventItem.ToDetailsViewModel(availableTickets, isBookable);

				return View(viewModel);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading event details for ID {EventId}", id);
				TempData["Error"] = "Unable to load event details.";
				return RedirectToAction(nameof(Index));
			}
		}
		public async Task<IActionResult> Category(int id)
		{
			var category = await _categoryService.GetCategoryByIdAsync(id);
			if (category == null)
			{
				return NotFound();
			}

			return RedirectToAction(nameof(Index), new { categoryId = id });
		}
	}
}
