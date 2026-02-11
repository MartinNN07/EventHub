using EventHub.Common;
using EventHub.Data.Models;
using EventHub.Services.Interfaces;
using EventHub.Web.Extensions;
using EventHub.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventHub.Web.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = RoleConstants.AdminRole)]
	public class EventsController : Controller
	{
		private readonly IEventService _eventService;
		private readonly ICategoryService _categoryService;
		private readonly IVenueService _venueService;
		private readonly IBookingService _bookingService;
		private readonly ILogger<EventsController> _logger;

		public EventsController(
			IEventService eventService,
			ICategoryService categoryService,
			IVenueService venueService,
			IBookingService bookingService,
			ILogger<EventsController> logger)
		{
			_eventService = eventService;
			_categoryService = categoryService;
			_venueService = venueService;
			_bookingService = bookingService;
			_logger = logger;
		}

		// GET: Admin/Events
		public async Task<IActionResult> Index()
		{
			try
			{
				var events = await _eventService.GetAllEventsAsync();
				var viewModels = events.Select(e => new AdminEventViewModel
				{
					Id = e.Id,
					Title = e.Title,
					Organizer = e.Organizer,
					Date = e.Date,
					TicketPrice = e.TicketPrice,
					VenueName = e.Venue?.Name ?? "Unknown",
					CategoryNames = e.Categories?.Select(c => c.Name).ToList() ?? new List<string>(),
					TotalBookings = e.Bookings?.Sum(b => b.TicketsCount) ?? 0
				}).OrderByDescending(e => e.Date).ToList();

				return View(viewModels);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading events");
				TempData["Error"] = "Unable to load events. Please try again.";
				return View(new List<AdminEventViewModel>());
			}
		}

		// GET: Admin/Events/Create
		public async Task<IActionResult> Create()
		{
			try
			{
				var venues = await _venueService.GetAllVenuesAsync();
				var categories = await _categoryService.GetAllCategoriesAsync();

				var viewModel = new CreateEventViewModel
				{
					Venues = new SelectList(venues, "Id", "Name"),
					AvailableCategories = categories.Select(c => c.ToViewModel()).ToList()
				};

				return View(viewModel);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading create event form");
				TempData["Error"] = "Unable to load form. Please try again.";
				return RedirectToAction(nameof(Index));
			}
		}

		// POST: Admin/Events/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateEventViewModel model)
		{
			if (!ModelState.IsValid)
			{
				await LoadCreateFormData(model);
				return View(model);
			}

			try
			{
				// Validate event date is in the future
				if (model.Date <= DateTime.Now)
				{
					ModelState.AddModelError("Date", "Event date must be in the future.");
					await LoadCreateFormData(model);
					return View(model);
				}

				// Validate venue exists
				var venueExists = await _venueService.VenueExistsAsync(model.VenueId);
				if (!venueExists)
				{
					ModelState.AddModelError("VenueId", "Selected venue does not exist.");
					await LoadCreateFormData(model);
					return View(model);
				}

				// Check venue availability
				var isVenueAvailable = await _venueService.IsVenueAvailableAsync(model.VenueId, model.Date);
				if (!isVenueAvailable)
				{
					ModelState.AddModelError("VenueId", "This venue is already booked for the selected date and time.");
					await LoadCreateFormData(model);
					return View(model);
				}

				// Validate categories
				if (model.CategoryIds == null || !model.CategoryIds.Any())
				{
					ModelState.AddModelError("CategoryIds", "Please select at least one category.");
					await LoadCreateFormData(model);
					return View(model);
				}

				// Get selected categories
				var selectedCategories = new List<Category>();
				foreach (var categoryId in model.CategoryIds)
				{
					var category = await _categoryService.GetCategoryByIdAsync(categoryId);
					if (category != null)
					{
						selectedCategories.Add(category);
					}
				}

				var eventEntity = new Event
				{
					Title = model.Title,
					Organizer = model.Organizer,
					Description = model.Description,
					Date = model.Date,
					TicketPrice = model.TicketPrice,
					ImageUrl = model.ImageUrl,
					VenueId = model.VenueId,
					Categories = selectedCategories
				};

				await _eventService.CreateEventAsync(eventEntity);
				TempData["Success"] = "Event created successfully!";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error creating event");
				ModelState.AddModelError("", "An error occurred while creating the event.");
				await LoadCreateFormData(model);
				return View(model);
			}
		}

		// GET: Admin/Events/Edit/5
		public async Task<IActionResult> Edit(int id)
		{
			try
			{
				var eventEntity = await _eventService.GetEventByIdAsync(id);
				if (eventEntity == null)
				{
					TempData["Error"] = "Event not found.";
					return RedirectToAction(nameof(Index));
				}

				var venues = await _venueService.GetAllVenuesAsync();
				var categories = await _categoryService.GetAllCategoriesAsync();

				var viewModel = new EditEventViewModel
				{
					Id = eventEntity.Id,
					Title = eventEntity.Title,
					Organizer = eventEntity.Organizer,
					Description = eventEntity.Description,
					Date = eventEntity.Date,
					TicketPrice = eventEntity.TicketPrice,
					ImageUrl = eventEntity.ImageUrl,
					VenueId = eventEntity.VenueId,
					CategoryIds = eventEntity.Categories?.Select(c => c.Id).ToList() ?? new List<int>(),
					Venues = new SelectList(venues, "Id", "Name", eventEntity.VenueId),
					AvailableCategories = categories.Select(c => c.ToViewModel()).ToList(),
					TotalBookings = eventEntity.Bookings?.Sum(b => b.TicketsCount) ?? 0
				};

				return View(viewModel);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading event for edit");
				TempData["Error"] = "Unable to load event.";
				return RedirectToAction(nameof(Index));
			}
		}

		// POST: Admin/Events/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, EditEventViewModel model)
		{
			if (id != model.Id)
			{
				return NotFound();
			}

			if (!ModelState.IsValid)
			{
				await LoadEditFormData(model);
				return View(model);
			}

			try
			{
				var existingEvent = await _eventService.GetEventByIdAsync(id);
				if (existingEvent == null)
				{
					TempData["Error"] = "Event not found.";
					return RedirectToAction(nameof(Index));
				}

				// Validate event date is in the future (unless it's already past)
				if (model.Date <= DateTime.Now && existingEvent.Date > DateTime.Now)
				{
					ModelState.AddModelError("Date", "Cannot change event date to the past.");
					await LoadEditFormData(model);
					return View(model);
				}

				// Validate venue exists
				var venueExists = await _venueService.VenueExistsAsync(model.VenueId);
				if (!venueExists)
				{
					ModelState.AddModelError("VenueId", "Selected venue does not exist.");
					await LoadEditFormData(model);
					return View(model);
				}

				// Check venue availability (excluding current event)
				var isVenueAvailable = await _venueService.IsVenueAvailableAsync(model.VenueId, model.Date, model.Id);
				if (!isVenueAvailable)
				{
					ModelState.AddModelError("VenueId", "This venue is already booked for the selected date and time.");
					await LoadEditFormData(model);
					return View(model);
				}

				// Validate categories
				if (model.CategoryIds == null || !model.CategoryIds.Any())
				{
					ModelState.AddModelError("CategoryIds", "Please select at least one category.");
					await LoadEditFormData(model);
					return View(model);
				}

				// Get selected categories
				var selectedCategories = new List<Category>();
				foreach (var categoryId in model.CategoryIds)
				{
					var category = await _categoryService.GetCategoryByIdAsync(categoryId);
					if (category != null)
					{
						selectedCategories.Add(category);
					}
				}

				existingEvent.Title = model.Title;
				existingEvent.Organizer = model.Organizer;
				existingEvent.Description = model.Description;
				existingEvent.Date = model.Date;
				existingEvent.TicketPrice = model.TicketPrice;
				existingEvent.ImageUrl = model.ImageUrl;
				existingEvent.VenueId = model.VenueId;
				existingEvent.Categories = selectedCategories;

				var result = await _eventService.UpdateEventAsync(existingEvent);
				if (result == null)
				{
					TempData["Error"] = "Event not found.";
					return RedirectToAction(nameof(Index));
				}

				TempData["Success"] = "Event updated successfully!";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating event");
				ModelState.AddModelError("", "An error occurred while updating the event.");
				await LoadEditFormData(model);
				return View(model);
			}
		}

		// GET: Admin/Events/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var eventEntity = await _eventService.GetEventByIdAsync(id);
				if (eventEntity == null)
				{
					TempData["Error"] = "Event not found.";
					return RedirectToAction(nameof(Index));
				}

				var viewModel = new AdminEventViewModel
				{
					Id = eventEntity.Id,
					Title = eventEntity.Title,
					Organizer = eventEntity.Organizer,
					Date = eventEntity.Date,
					TicketPrice = eventEntity.TicketPrice,
					VenueName = eventEntity.Venue?.Name ?? "Unknown",
					CategoryNames = eventEntity.Categories?.Select(c => c.Name).ToList() ?? new List<string>(),
					TotalBookings = eventEntity.Bookings?.Sum(b => b.TicketsCount) ?? 0
				};

				return View(viewModel);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading event for deletion");
				TempData["Error"] = "Unable to load event.";
				return RedirectToAction(nameof(Index));
			}
		}

		// POST: Admin/Events/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			try
			{
				var eventEntity = await _eventService.GetEventByIdAsync(id);
				if (eventEntity == null)
				{
					TempData["Error"] = "Event not found.";
					return RedirectToAction(nameof(Index));
				}

				// Check if event has bookings
				var bookings = await _bookingService.GetEventBookingsAsync(id);
				if (bookings.Any())
				{
					TempData["Error"] = $"Cannot delete event '{eventEntity.Title}' because it has {bookings.Count()} booking(s). Please cancel all bookings first.";
					return RedirectToAction(nameof(Index));
				}

				var result = await _eventService.DeleteEventAsync(id);
				if (result)
				{
					TempData["Success"] = "Event deleted successfully!";
				}
				else
				{
					TempData["Error"] = "Unable to delete event.";
				}

				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error deleting event");
				TempData["Error"] = "An error occurred while deleting the event.";
				return RedirectToAction(nameof(Index));
			}
		}

		private async Task LoadCreateFormData(CreateEventViewModel model)
		{
			var venues = await _venueService.GetAllVenuesAsync();
			var categories = await _categoryService.GetAllCategoriesAsync();

			model.Venues = new SelectList(venues, "Id", "Name");
			model.AvailableCategories = categories.Select(c => c.ToViewModel()).ToList();
		}

		private async Task LoadEditFormData(EditEventViewModel model)
		{
			var venues = await _venueService.GetAllVenuesAsync();
			var categories = await _categoryService.GetAllCategoriesAsync();

			model.Venues = new SelectList(venues, "Id", "Name", model.VenueId);
			model.AvailableCategories = categories.Select(c => c.ToViewModel()).ToList();
		}
	}
}
