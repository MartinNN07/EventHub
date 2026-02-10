using EventHub.Data.Models;

namespace EventHub.Services.Interfaces
{
	public interface IVenueService
	{
		/// <summary>
		/// Asynchronously retrieves all venues.
		/// </summary>
		/// <returns>A task that represents the asynchronous operation. The task result contains a collection of Venue objects.</returns>
		Task<IEnumerable<Venue>> GetAllVenuesAsync();
		/// <summary>
		/// Asynchronously retrieves a venue by its unique identifier.
		/// </summary>
		/// <param name="id">The unique identifier of the venue to retrieve.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the venue if found; otherwise, null.</returns>
		Task<Venue?> GetVenueByIdAsync(int id);
		/// <summary>
		/// Asynchronously retrieves a venue by its identifier, including its associated events.
		/// </summary>
		/// <param name="id">The unique identifier of the venue.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the venue with its events, or null if
		/// not found.</returns>
		Task<Venue?> GetVenueWithEventsAsync(int id);
		/// <summary>
		/// Asynchronously creates a new venue.
		/// </summary>
		/// <param name="venue">The venue to create.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the created Venue.</returns>
		Task<Venue> CreateVenueAsync(Venue venue);
		/// <summary>
		/// Asynchronously updates the specified venue in the data store.
		/// </summary>
		/// <param name="venue">The venue entity with updated information.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the updated venue, or null if the
		/// venue does not exist.</returns>
		Task<Venue?> UpdateVenueAsync(Venue venue);
		/// <summary>
		/// Asynchronously deletes a venue by its identifier.
		/// </summary>
		/// <param name="id">The unique identifier of the venue to delete.</param>
		/// <returns>True if the venue was successfully deleted; otherwise, false.</returns>
		Task<bool> DeleteVenueAsync(int id);
		/// <summary>
		/// Asynchronously determines whether a venue with the specified ID exists.
		/// </summary>
		/// <param name="id">The ID of the venue to check for existence.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains true if the venue exists; otherwise,
		/// false.</returns>
		Task<bool> VenueExistsAsync(int id);
		/// <summary>
		/// Checks asynchronously if a venue is available on a specified date, optionally excluding a specific event.
		/// </summary>
		/// <param name="venueId">The unique identifier of the venue to check.</param>
		/// <param name="eventDate">The date to check for venue availability.</param>
		/// <param name="excludeEventId">An optional event ID to exclude from the availability check.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains true if the venue is available;
		/// otherwise, false.</returns>
		Task<bool> IsVenueAvailableAsync(int venueId, DateTime eventDate, int? excludeEventId = null);
		/// <summary>
		/// Asynchronously retrieves the capacity of the specified venue.
		/// </summary>
		/// <param name="venueId">The unique identifier of the venue.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the capacity of the venue.</returns>
		Task<int> GetVenueCapacityAsync(int venueId);
	}
}