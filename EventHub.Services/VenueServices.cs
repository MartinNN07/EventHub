using EventHub.Data;
using EventHub.Data.Models;
using EventHub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Services.Implementations
{
	public class VenueService : IVenueService
	{
		private readonly ApplicationDbContext _context;

		public VenueService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Venue>> GetAllVenuesAsync()
		{
			return await _context.Venues
				.OrderBy(v => v.Name)
				.ToListAsync();
		}

		public async Task<Venue?> GetVenueByIdAsync(int id)
		{
			return await _context.Venues
				.FirstOrDefaultAsync(v => v.Id == id);
		}

		public async Task<Venue?> GetVenueWithEventsAsync(int id)
		{
			return await _context.Venues
				.Include(v => v.Events)
					.ThenInclude(e => e.Categories)
				.FirstOrDefaultAsync(v => v.Id == id);
		}

		public async Task<Venue> CreateVenueAsync(Venue venue)
		{
			_context.Venues.Add(venue);
			await _context.SaveChangesAsync();
			return venue;
		}

		public async Task<Venue?> UpdateVenueAsync(Venue venue)
		{
			var existingVenue = await _context.Venues.FindAsync(venue.Id);
			if (existingVenue == null)
			{
				return null;
			}

			existingVenue.Name = venue.Name;
			existingVenue.Address = venue.Address;
			existingVenue.Capacity = venue.Capacity;

			await _context.SaveChangesAsync();
			return existingVenue;
		}

		public async Task<bool> DeleteVenueAsync(int id)
		{
			var venue = await _context.Venues
				.Include(v => v.Events)
				.FirstOrDefaultAsync(v => v.Id == id);

			if (venue == null)
			{
				return false;
			}

			if (venue.Events.Any())
			{
				return false;
			}

			_context.Venues.Remove(venue);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> VenueExistsAsync(int id)
		{
			return await _context.Venues.AnyAsync(v => v.Id == id);
		}

		public async Task<bool> IsVenueAvailableAsync(int venueId, DateTime eventDate, int? excludeEventId = null)
		{
			var venue = await _context.Venues
				.Include(v => v.Events)
				.FirstOrDefaultAsync(v => v.Id == venueId);

			if (venue == null)
			{
				return false;
			}

			var hasConflict = venue.Events
				.Where(e => excludeEventId == null || e.Id != excludeEventId.Value)
				.Any(e => e.Date.Date == eventDate.Date);

			return !hasConflict;
		}

		public async Task<int> GetVenueCapacityAsync(int venueId)
		{
			var venue = await _context.Venues.FindAsync(venueId);
			return venue?.Capacity ?? 0;
		}
	}
}