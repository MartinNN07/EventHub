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
	}
}
