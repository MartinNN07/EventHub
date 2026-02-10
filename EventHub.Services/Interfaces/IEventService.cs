using EventHub.Data.Models;

namespace EventHub.Services.Interfaces
{
	public interface IEventService
	{
		/// <summary>
		/// Asynchronously retrieves all events.
		/// </summary>
		/// <returns> A task that represents the asynchronous operation. The task result contains a collection of Event objects.</returns>
		Task<IEnumerable<Event>> GetAllEventsAsync();
		/// <summary>
		/// Asynchronously retrieves an event by its unique identifier.
		/// </summary>
		/// <param name="id">The unique identifier of the event to retrieve.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the event if found; otherwise, null.</returns>
		Task<Event?> GetEventByIdAsync(int id);
		/// <summary>
		/// Asynchronously retrieves a collection of upcoming events.
		/// </summary>
		/// <returns>A task that represents the asynchronous operation. The task result contains a collection of upcoming Event
		/// objects.</returns>
		Task<IEnumerable<Event>> GetUpcomingEventsAsync();
		/// <summary>
		/// Asynchronously retrieves a collection of events belonging to the specified category.
		/// </summary>
		/// <param name="categoryId">The unique identifier of the event category.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains a collection of events for the given
		/// category.</returns>
		Task<IEnumerable<Event>> GetEventsByCategoryAsync(int categoryId);
		/// <summary>
		/// Asynchronously retrieves a collection of events for the specified venue.
		/// </summary>
		/// <param name="venueId">The unique identifier of the venue.</param>
		/// <returns>A task representing the asynchronous operation, containing a collection of events for the venue.</returns>
		Task<IEnumerable<Event>> GetEventsByVenueAsync(int venueId);
		/// <summary>
		/// Asynchronously searches for events matching the specified search term.
		/// </summary>
		/// <param name="searchTerm">The term to search for in event data.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains a collection of events matching the
		/// search criteria.</returns>
		Task<IEnumerable<Event>> SearchEventsAsync(string searchTerm);
		/// <summary>
		/// Asynchronously retrieves a collection of featured events.
		/// </summary>
		/// <param name="count">The maximum number of featured events to return. Defaults to 6.</param>
		/// <returns>A task representing the asynchronous operation, containing a collection of featured events.</returns>
		Task<IEnumerable<Event>> GetFeaturedEventsAsync(int count = 6);
		/// <summary>
		/// Asynchronously creates a new event.
		/// </summary>
		/// <param name="eventModel">The event data to create.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the created Event.</returns>
		Task<Event> CreateEventAsync(Event eventModel);
		/// <summary>
		/// Asynchronously updates an existing event.
		/// </summary>
		/// <param name="eventModel">The event model containing updated event information.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the updated event, or null if the
		/// event was not found.</returns>
		Task<Event?> UpdateEventAsync(Event eventModel);
		/// <summary>
		/// Asynchronously deletes the event with the specified identifier.
		/// </summary>
		/// <param name="id">The unique identifier of the event to delete.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains true if the event was successfully
		/// deleted; otherwise, false.</returns>
		Task<bool> DeleteEventAsync(int id);
		/// <summary>
		/// Asynchronously determines whether an event with the specified ID exists.
		/// </summary>
		/// <param name="id">The ID of the event to check for existence.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains true if the event exists; otherwise,
		/// false.</returns>
		Task<bool> EventExistsAsync(int id);
		/// <summary>
		/// Asynchronously retrieves the number of available tickets for the specified event.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the number of available tickets.</returns>
		Task<int> GetAvailableTicketsAsync(int eventId);
		/// <summary>
		/// Determines asynchronously whether the specified event is available for booking.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event to check.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains true if the event is bookable;
		/// otherwise, false.</returns>
		Task<bool> IsEventBookableAsync(int eventId);
	}
}