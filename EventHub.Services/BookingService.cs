using EventHub.Data;
using EventHub.Data.Models;
using EventHub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Services.Implementations
{
	public class BookingService : IBookingService
	{
		private readonly ApplicationDbContext _context;

		public BookingService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Booking>> GetUserBookingsAsync(string userId)
		{
			return await _context.Bookings
				.Include(b => b.Event)
					.ThenInclude(e => e.Venue)
				.Include(b => b.Event)
					.ThenInclude(e => e.Categories)
				.Where(b => b.BuyerId == userId)
				.OrderByDescending(b => b.BookingDate)
				.ToListAsync();
		}

		public async Task<Booking?> GetBookingByIdAsync(int id)
		{
			return await _context.Bookings
				.Include(b => b.Event)
					.ThenInclude(e => e.Venue)
				.Include(b => b.Event)
					.ThenInclude(e => e.Categories)
				.Include(b => b.Buyer)
				.FirstOrDefaultAsync(b => b.Id == id);
		}

		public async Task<IEnumerable<Booking>> GetEventBookingsAsync(int eventId)
		{
			return await _context.Bookings
				.Include(b => b.Buyer)
				.Where(b => b.EventId == eventId)
				.OrderByDescending(b => b.BookingDate)
				.ToListAsync();
		}

		public async Task<int> GetTotalBookedTicketsAsync(int eventId)
		{
			return await _context.Bookings
				.Where(b => b.EventId == eventId)
				.SumAsync(b => b.TicketsCount);
		}

		public async Task<Booking?> CreateBookingAsync(string userId, int eventId, int ticketsCount)
		{
			if (!await CanUserBookEventAsync(userId, eventId, ticketsCount))
			{
				return null;
			}

			var booking = new Booking
			{
				BuyerId = userId,
				EventId = eventId,
				TicketsCount = ticketsCount,
				BookingDate = DateTime.Now
			};

			_context.Bookings.Add(booking);
			await _context.SaveChangesAsync();

			return await GetBookingByIdAsync(booking.Id);
		}

		public async Task<bool> CancelBookingAsync(int bookingId, string userId)
		{
			var booking = await _context.Bookings
				.Include(b => b.Event)
				.FirstOrDefaultAsync(b => b.Id == bookingId);

			if (booking == null || booking.BuyerId != userId)
			{
				return false;
			}

			if (booking.Event.Date <= DateTime.Now)
			{
				return false;
			}

			_context.Bookings.Remove(booking);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> CanUserBookEventAsync(string userId, int eventId, int ticketsCount)
		{
			var eventEntity = await _context.Events
				.Include(e => e.Venue)
				.Include(e => e.Bookings)
				.FirstOrDefaultAsync(e => e.Id == eventId);

			if (eventEntity == null)
			{
				return false;
			}

			if (eventEntity.Date <= DateTime.Now)
			{
				return false;
			}

			if (ticketsCount <= 0)
			{
				return false;
			}

			var totalBooked = eventEntity.Bookings.Sum(b => b.TicketsCount);
			var availableTickets = eventEntity.Venue.Capacity - totalBooked;

			if (ticketsCount > availableTickets)
			{
				return false;
			}

			return true;
		}

		public async Task<bool> IsBookingOwnedByUserAsync(int bookingId, string userId)
		{
			return await _context.Bookings
				.AnyAsync(b => b.Id == bookingId && b.BuyerId == userId);
		}

		public async Task<bool> BookingExistsAsync(int bookingId)
		{
			return await _context.Bookings.AnyAsync(b => b.Id == bookingId);
		}
	}
}