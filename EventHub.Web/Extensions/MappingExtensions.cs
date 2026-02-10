using EventHub.Data.Models;
using EventHub.Web.Models;

namespace EventHub.Web.Extensions
{
	public static class MappingExtensions
	{
		// Event mappings
		public static EventViewModel ToViewModel(this Event eventEntity, int availableTickets = 0, bool isBookable = false)
		{
			return new EventViewModel
			{
				Id = eventEntity.Id,
				Title = eventEntity.Title,
				Organizer = eventEntity.Organizer,
				Description = eventEntity.Description,
				Date = eventEntity.Date,
				TicketPrice = eventEntity.TicketPrice,
				ImageUrl = eventEntity.ImageUrl,
				VenueName = eventEntity.Venue?.Name ?? "Unknown",
				VenueAddress = eventEntity.Venue?.Address ?? "Unknown",
				VenueCapacity = eventEntity.Venue?.Capacity ?? 0,
				CategoryNames = eventEntity.Categories?.Select(c => c.Name).ToList() ?? new List<string>(),
				AvailableTickets = availableTickets,
				IsBookable = isBookable
			};
		}

		public static EventDetailsViewModel ToDetailsViewModel(this Event eventEntity, int availableTickets, bool isBookable)
		{
			var totalBooked = eventEntity.Bookings?.Sum(b => b.TicketsCount) ?? 0;

			return new EventDetailsViewModel
			{
				Id = eventEntity.Id,
				Title = eventEntity.Title,
				Organizer = eventEntity.Organizer,
				Description = eventEntity.Description,
				Date = eventEntity.Date,
				TicketPrice = eventEntity.TicketPrice,
				ImageUrl = eventEntity.ImageUrl,
				VenueId = eventEntity.VenueId,
				VenueName = eventEntity.Venue?.Name ?? "Unknown",
				VenueAddress = eventEntity.Venue?.Address ?? "Unknown",
				VenueCapacity = eventEntity.Venue?.Capacity ?? 0,
				Categories = eventEntity.Categories?.Select(c => new CategoryViewModel
				{
					Id = c.Id,
					Name = c.Name
				}).ToList() ?? new List<CategoryViewModel>(),
				AvailableTickets = availableTickets,
				TotalBookedTickets = totalBooked,
				IsBookable = isBookable
			};
		}

		public static CategoryViewModel ToViewModel(this Category category)
		{
			return new CategoryViewModel
			{
				Id = category.Id,
				Name = category.Name,
				EventCount = category.Events?.Count ?? 0
			};
		}

		// Booking mappings
		public static BookingViewModel ToViewModel(this Booking booking)
		{
			var isPastEvent = booking.Event.Date < DateTime.Now;

			return new BookingViewModel
			{
				Id = booking.Id,
				BookingDate = booking.BookingDate,
				TicketsCount = booking.TicketsCount,
				TotalPrice = booking.TicketsCount * booking.Event.TicketPrice,
				EventId = booking.EventId,
				EventTitle = booking.Event.Title,
				EventOrganizer = booking.Event.Organizer,
				EventDate = booking.Event.Date,
				TicketPrice = booking.Event.TicketPrice,
				EventImageUrl = booking.Event.ImageUrl,
				VenueName = booking.Event.Venue?.Name ?? "Unknown",
				VenueAddress = booking.Event.Venue?.Address ?? "Unknown",
				CanCancel = !isPastEvent,
				StatusBadgeClass = isPastEvent ? "bg-secondary" : "bg-success",
				StatusText = isPastEvent ? "Past Event" : "Confirmed"
			};
		}

		public static BookingConfirmationViewModel ToConfirmationViewModel(this Booking booking, string userEmail)
		{
			return new BookingConfirmationViewModel
			{
				BookingId = booking.Id,
				BookingDate = booking.BookingDate,
				TicketsCount = booking.TicketsCount,
				TotalPrice = booking.TicketsCount * booking.Event.TicketPrice,
				EventId = booking.EventId,
				EventTitle = booking.Event.Title,
				EventOrganizer = booking.Event.Organizer,
				EventDate = booking.Event.Date,
				EventImageUrl = booking.Event.ImageUrl,
				VenueName = booking.Event.Venue?.Name ?? "Unknown",
				VenueAddress = booking.Event.Venue?.Address ?? "Unknown",
				ConfirmationNumber = $"BK{booking.Id:D6}",
				UserEmail = userEmail
			};
		}
	}
}