using EventHub.Services.Interfaces;
using EventHub.Web.Extensions;
using EventHub.Web.Models.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventHub.Web.Controllers
{
	[Authorize]
	public class BookingsController : Controller
	{
		private readonly IBookingService _bookingService;
		private readonly IEventService _eventService;
		private readonly UserManager<EventHub.Data.Models.ApplicationUser> _userManager;
		private readonly ILogger<BookingsController> _logger;

		public BookingsController(
			IBookingService bookingService,
			IEventService eventService,
			UserManager<EventHub.Data.Models.ApplicationUser> userManager,
			ILogger<BookingsController> logger)
		{
			_bookingService = bookingService;
			_eventService = eventService;
			_userManager = userManager;
			_logger = logger;
		}

		// GET: Bookings
		public async Task<IActionResult> Index()
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (string.IsNullOrEmpty(userId))
				{
					return RedirectToAction("Login", "Account");
				}

				var bookings = await _bookingService.GetUserBookingsAsync(userId);
				var now = DateTime.Now;

				var viewModel = new MyBookingsViewModel
				{
					UpcomingBookings = bookings
						.Where(b => b.Event.Date >= now)
						.OrderBy(b => b.Event.Date)
						.Select(b => b.ToViewModel())
						.ToList(),
					PastBookings = bookings
						.Where(b => b.Event.Date < now)
						.OrderByDescending(b => b.Event.Date)
						.Select(b => b.ToViewModel())
						.ToList()
				};

				return View(viewModel);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading user bookings");
				TempData["Error"] = "Unable to load your bookings. Please try again.";
				return View(new MyBookingsViewModel());
			}
		}

		// GET: Bookings/Create/5
		public async Task<IActionResult> Create(int eventId)
		{
			try
			{
				var eventItem = await _eventService.GetEventByIdAsync(eventId);
				if (eventItem == null)
				{
					TempData["Error"] = "Event not found.";
					return RedirectToAction("Index", "Events");
				}

				var availableTickets = await _eventService.GetAvailableTicketsAsync(eventId);
				var isBookable = await _eventService.IsEventBookableAsync(eventId);

				if (!isBookable)
				{
					TempData["Error"] = "This event is not available for booking.";
					return RedirectToAction("Details", "Events", new { eventId });
				}

				var viewModel = new CreateBookingViewModel
				{
					EventId = eventItem.Id,
					EventTitle = eventItem.Title,
					EventOrganizer = eventItem.Organizer,
					EventDescription = eventItem.Description,
					EventDate = eventItem.Date,
					TicketPrice = eventItem.TicketPrice,
					EventImageUrl = eventItem.ImageUrl,
					VenueName = eventItem.Venue?.Name ?? "Unknown",
					VenueAddress = eventItem.Venue?.Address ?? "Unknown",
					VenueCapacity = eventItem.Venue?.Capacity ?? 0,
					AvailableTickets = availableTickets,
					TotalBookedTickets = eventItem.Bookings?.Sum(b => b.TicketsCount) ?? 0,
					TicketsCount = 1
				};

				return View(viewModel);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading booking form for event ID {EventId}", eventId);
				TempData["Error"] = "Unable to load booking form.";
				return RedirectToAction("Index", "Events");
			}
		}

		// POST: Bookings/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateBookingViewModel model)
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (string.IsNullOrEmpty(userId))
				{
					return RedirectToAction("Login", "Account");
				}

				if (!ModelState.IsValid)
				{
					// Reload event data for the view
					var eventItem = await _eventService.GetEventByIdAsync(model.EventId);
					if (eventItem != null)
					{
						model.EventTitle = eventItem.Title;
						model.EventOrganizer = eventItem.Organizer;
						model.EventDescription = eventItem.Description;
						model.EventDate = eventItem.Date;
						model.TicketPrice = eventItem.TicketPrice;
						model.EventImageUrl = eventItem.ImageUrl;
						model.VenueName = eventItem.Venue?.Name ?? "Unknown";
						model.VenueAddress = eventItem.Venue?.Address ?? "Unknown";
						model.VenueCapacity = eventItem.Venue?.Capacity ?? 0;
						model.AvailableTickets = await _eventService.GetAvailableTicketsAsync(model.EventId);
						model.TotalBookedTickets = eventItem.Bookings?.Sum(b => b.TicketsCount) ?? 0;
					}
					return View(model);
				}

				// Verify event exists and is bookable
				var canBook = await _bookingService.CanUserBookEventAsync(userId, model.EventId, model.TicketsCount);
				if (!canBook)
				{
					ModelState.AddModelError("", "Unable to complete booking. The event may be sold out or not available.");
					var eventItem = await _eventService.GetEventByIdAsync(model.EventId);
					if (eventItem != null)
					{
						model.EventTitle = eventItem.Title;
						model.EventOrganizer = eventItem.Organizer;
						model.EventDescription = eventItem.Description;
						model.EventDate = eventItem.Date;
						model.TicketPrice = eventItem.TicketPrice;
						model.EventImageUrl = eventItem.ImageUrl;
						model.VenueName = eventItem.Venue?.Name ?? "Unknown";
						model.VenueAddress = eventItem.Venue?.Address ?? "Unknown";
						model.VenueCapacity = eventItem.Venue?.Capacity ?? 0;
						model.AvailableTickets = await _eventService.GetAvailableTicketsAsync(model.EventId);
						model.TotalBookedTickets = eventItem.Bookings?.Sum(b => b.TicketsCount) ?? 0;
					}
					return View(model);
				}

				// Create the booking
				var booking = await _bookingService.CreateBookingAsync(userId, model.EventId, model.TicketsCount);
				if (booking == null)
				{
					ModelState.AddModelError("", "Failed to create booking. Please try again.");
					var eventItem = await _eventService.GetEventByIdAsync(model.EventId);
					if (eventItem != null)
					{
						model.EventTitle = eventItem.Title;
						model.EventOrganizer = eventItem.Organizer;
						model.EventDescription = eventItem.Description;
						model.EventDate = eventItem.Date;
						model.TicketPrice = eventItem.TicketPrice;
						model.EventImageUrl = eventItem.ImageUrl;
						model.VenueName = eventItem.Venue?.Name ?? "Unknown";
						model.VenueAddress = eventItem.Venue?.Address ?? "Unknown";
						model.VenueCapacity = eventItem.Venue?.Capacity ?? 0;
						model.AvailableTickets = await _eventService.GetAvailableTicketsAsync(model.EventId);
						model.TotalBookedTickets = eventItem.Bookings?.Sum(b => b.TicketsCount) ?? 0;
					}
					return View(model);
				}

				TempData["Success"] = "Booking created successfully!";
				return RedirectToAction(nameof(Confirmation), new { id = booking.Id });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error creating booking for event ID {EventId}", model.EventId);
				ModelState.AddModelError("", "An error occurred while creating your booking. Please try again.");
				return View(model);
			}
		}

		// GET: Bookings/Confirmation/5
		public async Task<IActionResult> Confirmation(int id)
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (string.IsNullOrEmpty(userId))
				{
					return RedirectToAction("Login", "Account");
				}

				var booking = await _bookingService.GetBookingByIdAsync(id);
				if (booking == null)
				{
					TempData["Error"] = "Booking not found.";
					return RedirectToAction(nameof(Index));
				}

				// Verify the booking belongs to the current user
				if (booking.BuyerId != userId)
				{
					TempData["Error"] = "You don't have permission to view this booking.";
					return RedirectToAction(nameof(Index));
				}

				var user = await _userManager.FindByIdAsync(userId);
				var viewModel = booking.ToConfirmationViewModel(user?.Email ?? "");

				return View(viewModel);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading booking confirmation for ID {BookingId}", id);
				TempData["Error"] = "Unable to load booking confirmation.";
				return RedirectToAction(nameof(Index));
			}
		}

		// POST: Bookings/Cancel/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Cancel(int id)
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (string.IsNullOrEmpty(userId))
				{
					return RedirectToAction("Login", "Account");
				}

				// Verify booking exists
				var bookingExists = await _bookingService.BookingExistsAsync(id);
				if (!bookingExists)
				{
					TempData["Error"] = "Booking not found.";
					return RedirectToAction(nameof(Index));
				}

				// Verify ownership
				var isOwner = await _bookingService.IsBookingOwnedByUserAsync(id, userId);
				if (!isOwner)
				{
					TempData["Error"] = "You don't have permission to cancel this booking.";
					return RedirectToAction(nameof(Index));
				}

				// Cancel the booking
				var result = await _bookingService.CancelBookingAsync(id, userId);
				if (result)
				{
					TempData["Success"] = "Booking cancelled successfully.";
				}
				else
				{
					TempData["Error"] = "Unable to cancel booking. It may be too late to cancel.";
				}

				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error cancelling booking ID {BookingId}", id);
				TempData["Error"] = "An error occurred while cancelling your booking.";
				return RedirectToAction(nameof(Index));
			}
		}

		// AJAX: Check availability
		[HttpGet]
		public async Task<IActionResult> CheckAvailability(int eventId, int ticketsCount)
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (string.IsNullOrEmpty(userId))
				{
					return Json(new { success = false, message = "User not authenticated" });
				}

				var canBook = await _bookingService.CanUserBookEventAsync(userId, eventId, ticketsCount);
				var availableTickets = await _eventService.GetAvailableTicketsAsync(eventId);

				return Json(new
				{
					success = canBook,
					availableTickets = availableTickets,
					message = canBook ? "Tickets are available" : "Not enough tickets available"
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error checking availability for event ID {EventId}", eventId);
				return Json(new { success = false, message = "Error checking availability" });
			}
		}
	}
}
