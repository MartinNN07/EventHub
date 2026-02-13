using EventHub.Common;
using EventHub.Data.Models;
using EventHub.Services.Interfaces;
using EventHub.Web.Models.Venue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Web.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = RoleConstants.AdminRole)]
	public class VenuesController : Controller
	{
		private readonly IVenueService _venueService;
		private readonly ILogger<VenuesController> _logger;

		public VenuesController(
			IVenueService venueService,
			ILogger<VenuesController> logger)
		{
			_venueService = venueService;
			_logger = logger;
		}

		// GET: Admin/Venues
		public async Task<IActionResult> Index()
		{
			try
			{
				var venues = await _venueService.GetAllVenuesAsync();
				var viewModels = venues.Select(v => new VenueViewModel
				{
					Id = v.Id,
					Name = v.Name,
					Address = v.Address,
					Capacity = v.Capacity,
					EventCount = v.Events?.Count ?? 0
				}).OrderBy(v => v.Name).ToList();

				return View(viewModels);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading venues");
				TempData["Error"] = "Unable to load venues. Please try again.";
				return View(new List<VenueViewModel>());
			}
		}

		// GET: Admin/Venues/Create
		public IActionResult Create()
		{
			return View(new CreateVenueViewModel());
		}

		// POST: Admin/Venues/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateVenueViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			try
			{
				var venue = new Venue
				{
					Name = model.Name,
					Address = model.Address,
					Capacity = model.Capacity
				};

				await _venueService.CreateVenueAsync(venue);
				TempData["Success"] = "Venue created successfully!";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error creating venue");
				ModelState.AddModelError("", "An error occurred while creating the venue.");
				return View(model);
			}
		}

		// GET: Admin/Venues/Edit/5
		public async Task<IActionResult> Edit(int id)
		{
			try
			{
				var venue = await _venueService.GetVenueWithEventsAsync(id);
				if (venue == null)
				{
					TempData["Error"] = "Venue not found.";
					return RedirectToAction(nameof(Index));
				}

				var viewModel = new EditVenueViewModel
				{
					Id = venue.Id,
					Name = venue.Name,
					Address = venue.Address,
					Capacity = venue.Capacity,
					EventCount = venue.Events?.Count ?? 0
				};

				return View(viewModel);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading venue for edit");
				TempData["Error"] = "Unable to load venue.";
				return RedirectToAction(nameof(Index));
			}
		}

		// POST: Admin/Venues/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, EditVenueViewModel model)
		{
			if (id != model.Id)
			{
				return NotFound();
			}

			if (!ModelState.IsValid)
			{
				var venue = await _venueService.GetVenueWithEventsAsync(id);
				if (venue != null)
				{
					model.EventCount = venue.Events?.Count ?? 0;
				}
				return View(model);
			}

			try
			{
				// Get current venue to check for existing bookings
				var currentVenue = await _venueService.GetVenueWithEventsAsync(id);
				if (currentVenue == null)
				{
					TempData["Error"] = "Venue not found.";
					return RedirectToAction(nameof(Index));
				}

				// Check if reducing capacity would affect existing events
				if (model.Capacity < currentVenue.Capacity)
				{
					var hasEvents = currentVenue.Events != null && currentVenue.Events.Any();
					if (hasEvents)
					{
						var maxBookings = currentVenue.Events!
							.SelectMany(e => e.Bookings)
							.GroupBy(b => b.EventId)
							.Select(g => g.Sum(b => b.TicketsCount))
							.DefaultIfEmpty(0)
							.Max();

						if (model.Capacity < maxBookings)
						{
							ModelState.AddModelError("Capacity", 
								$"Cannot reduce capacity below {maxBookings} as there are existing bookings that exceed this amount.");
							model.EventCount = currentVenue.Events.Count;
							return View(model);
						}
					}
				}

				var venue = new Venue
				{
					Id = model.Id,
					Name = model.Name,
					Address = model.Address,
					Capacity = model.Capacity
				};

				var result = await _venueService.UpdateVenueAsync(venue);
				if (result == null)
				{
					TempData["Error"] = "Venue not found.";
					return RedirectToAction(nameof(Index));
				}

				TempData["Success"] = "Venue updated successfully!";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating venue");
				ModelState.AddModelError("", "An error occurred while updating the venue.");
				return View(model);
			}
		}

		// GET: Admin/Venues/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var venue = await _venueService.GetVenueWithEventsAsync(id);
				if (venue == null)
				{
					TempData["Error"] = "Venue not found.";
					return RedirectToAction(nameof(Index));
				}

				var viewModel = new VenueViewModel
				{
					Id = venue.Id,
					Name = venue.Name,
					Address = venue.Address,
					Capacity = venue.Capacity,
					EventCount = venue.Events?.Count ?? 0
				};

				return View(viewModel);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading venue for deletion");
				TempData["Error"] = "Unable to load venue.";
				return RedirectToAction(nameof(Index));
			}
		}

		// POST: Admin/Venues/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			try
			{
				var venue = await _venueService.GetVenueWithEventsAsync(id);
				if (venue == null)
				{
					TempData["Error"] = "Venue not found.";
					return RedirectToAction(nameof(Index));
				}

				// Check if venue has events
				if (venue.Events != null && venue.Events.Any())
				{
					TempData["Error"] = $"Cannot delete venue '{venue.Name}' because it has {venue.Events.Count} associated event(s). Please remove or reassign these events first.";
					return RedirectToAction(nameof(Index));
				}

				var result = await _venueService.DeleteVenueAsync(id);
				if (result)
				{
					TempData["Success"] = "Venue deleted successfully!";
				}
				else
				{
					TempData["Error"] = "Unable to delete venue.";
				}

				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error deleting venue");
				TempData["Error"] = "An error occurred while deleting the venue.";
				return RedirectToAction(nameof(Index));
			}
		}
	}
}
