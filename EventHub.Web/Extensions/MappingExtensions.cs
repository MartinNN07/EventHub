using EventHub.Data.Models;
using EventHub.Web.Models;

namespace EventHub.Web.Extensions
{
	public static class MappingExtensions
	{
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

		public static BookingViewModel ToViewModel(this Booking booking)
		{
			var eventDate = booking.Event?.Date ?? DateTime.MinValue;
			var canCancel = eventDate > DateTime.Now.AddDays(1);

			return new BookingViewModel
			{
				Id = booking.Id,
				BookingDate = booking.BookingDate,
				TicketsCount = booking.TicketsCount,
				TotalPrice = booking.TicketsCount * (booking.Event?.TicketPrice ?? 0),
				EventId = booking.EventId,
				EventTitle = booking.Event?.Title ?? "Unknown",
				EventOrganizer = booking.Event?.Organizer ?? "Unknown",
				EventDate = eventDate,
				TicketPrice = booking.Event?.TicketPrice ?? 0,
				EventImageUrl = booking.Event?.ImageUrl,
				VenueName = booking.Event?.Venue?.Name ?? "Unknown",
				VenueAddress = booking.Event?.Venue?.Address ?? "Unknown",
				CanCancel = canCancel,
				StatusBadgeClass = eventDate > DateTime.Now ? "bg-success" : "bg-secondary",
				StatusText = eventDate > DateTime.Now ? "Confirmed" : "Completed"
			};
		}

		public static BookingConfirmationViewModel ToConfirmationViewModel(this Booking booking, string userEmail)
		{
			return new BookingConfirmationViewModel
			{
				BookingId = booking.Id,
				BookingDate = booking.BookingDate,
				TicketsCount = booking.TicketsCount,
				TotalPrice = booking.TicketsCount * (booking.Event?.TicketPrice ?? 0),
				EventId = booking.EventId,
				EventTitle = booking.Event?.Title ?? "Unknown",
				EventOrganizer = booking.Event?.Organizer ?? "Unknown",
				EventDate = booking.Event?.Date ?? DateTime.MinValue,
				EventImageUrl = booking.Event?.ImageUrl,
				VenueName = booking.Event?.Venue?.Name ?? "Unknown",
				VenueAddress = booking.Event?.Venue?.Address ?? "Unknown",
				ConfirmationNumber = $"EVT-{booking.Id:D6}",
				UserEmail = userEmail
			};
		}
	}
}
