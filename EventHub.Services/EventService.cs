using EventHub.Data;
using EventHub.Data.Models;
using EventHub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Services.Implementations
{
	public class EventService : IEventService
	{
		private readonly ApplicationDbContext _context;

		public EventService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Event>> GetAllEventsAsync()
		{
			return await _context.Events
				.Include(e => e.Venue)
				.Include(e => e.Categories)
				.Include(e => e.Bookings)
				.OrderBy(e => e.Date)
				.ToListAsync();
		}

		public async Task<Event?> GetEventByIdAsync(int id)
		{
			return await _context.Events
				.Include(e => e.Venue)
				.Include(e => e.Categories)
				.Include(e => e.Bookings)
				.FirstOrDefaultAsync(e => e.Id == id);
		}

		public async Task<IEnumerable<Event>> GetUpcomingEventsAsync()
		{
			var now = DateTime.Now;
			return await _context.Events
				.Include(e => e.Venue)
				.Include(e => e.Categories)
				.Include(e => e.Bookings)
				.Where(e => e.Date > now)
				.OrderBy(e => e.Date)
				.ToListAsync();
		}

		public async Task<IEnumerable<Event>> GetEventsByCategoryAsync(int categoryId)
		{
			return await _context.Events
				.Include(e => e.Venue)
				.Include(e => e.Categories)
				.Include(e => e.Bookings)
				.Where(e => e.Categories.Any(c => c.Id == categoryId))
				.OrderBy(e => e.Date)
				.ToListAsync();
		}

		public async Task<IEnumerable<Event>> GetEventsByVenueAsync(int venueId)
		{
			return await _context.Events
				.Include(e => e.Venue)
				.Include(e => e.Categories)
				.Include(e => e.Bookings)
				.Where(e => e.VenueId == venueId)
				.OrderBy(e => e.Date)
				.ToListAsync();
		}

		public async Task<IEnumerable<Event>> SearchEventsAsync(string searchTerm)
		{
			if (string.IsNullOrWhiteSpace(searchTerm))
			{
				return await GetAllEventsAsync();
			}

			searchTerm = searchTerm.ToLower();

			return await _context.Events
				.Include(e => e.Venue)
				.Include(e => e.Categories)
				.Include(e => e.Bookings)
				.Where(e => e.Title.ToLower().Contains(searchTerm) ||
							e.Description.ToLower().Contains(searchTerm) ||
							e.Organizer.ToLower().Contains(searchTerm) ||
							e.Venue.Name.ToLower().Contains(searchTerm))
				.OrderBy(e => e.Date)
				.ToListAsync();
		}

		public async Task<IEnumerable<Event>> GetFeaturedEventsAsync(int count = 6)
		{
			var now = DateTime.Now;
			return await _context.Events
				.Include(e => e.Venue)
				.Include(e => e.Categories)
				.Include(e => e.Bookings)
				.Where(e => e.Date > now)
				.OrderBy(e => e.Date)
				.Take(count)
				.ToListAsync();
		}

		public async Task<Event> CreateEventAsync(Event eventModel)
		{
			_context.Events.Add(eventModel);
			await _context.SaveChangesAsync();
			return eventModel;
		}

		public async Task<Event?> UpdateEventAsync(Event eventModel)
		{
			var existingEvent = await _context.Events
				.Include(e => e.Categories)
				.FirstOrDefaultAsync(e => e.Id == eventModel.Id);

			if (existingEvent == null)
			{
				return null;
			}

			existingEvent.Title = eventModel.Title;
			existingEvent.Organizer = eventModel.Organizer;
			existingEvent.Description = eventModel.Description;
			existingEvent.Date = eventModel.Date;
			existingEvent.TicketPrice = eventModel.TicketPrice;
			existingEvent.ImageUrl = eventModel.ImageUrl;
			existingEvent.VenueId = eventModel.VenueId;

			existingEvent.Categories.Clear();
			if (eventModel.Categories != null && eventModel.Categories.Any())
			{
				foreach (var category in eventModel.Categories)
				{
					var existingCategory = await _context.Categories.FindAsync(category.Id);
					if (existingCategory != null)
					{
						existingEvent.Categories.Add(existingCategory);
					}
				}
			}

			await _context.SaveChangesAsync();
			return existingEvent;
		}

		public async Task<bool> DeleteEventAsync(int id)
		{
			var eventToDelete = await _context.Events.FindAsync(id);
			if (eventToDelete == null)
			{
				return false;
			}

			_context.Events.Remove(eventToDelete);
			await _context.SaveChangesAsync();
			return true;
		}
		public async Task<bool> EventExistsAsync(int id)
		{
			return await _context.Events.AnyAsync(e => e.Id == id);
		}

		public async Task<int> GetAvailableTicketsAsync(int eventId)
		{
			var eventEntity = await _context.Events
				.Include(e => e.Venue)
				.Include(e => e.Bookings)
				.FirstOrDefaultAsync(e => e.Id == eventId);

			if (eventEntity == null)
			{
				return 0;
			}

			var totalBooked = eventEntity.Bookings.Sum(b => b.TicketsCount);
			return eventEntity.Venue.Capacity - totalBooked;
		}

		public async Task<bool> IsEventBookableAsync(int eventId)
		{
			var eventEntity = await _context.Events
				.FirstOrDefaultAsync(e => e.Id == eventId);

			if (eventEntity == null)
			{
				return false;
			}

			if (eventEntity.Date <= DateTime.Now)
			{
				return false;
			}

			var availableTickets = await GetAvailableTicketsAsync(eventId);
			return availableTickets > 0;
		}
	}
}