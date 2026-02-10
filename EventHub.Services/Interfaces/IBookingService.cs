using EventHub.Data.Models;

namespace EventHub.Services.Interfaces
{
	public interface IBookingService
	{
		/// <summary>
		/// Asynchronously retrieves all bookings associated with the specified user.
		/// </summary>
		/// <param name="userId">The unique identifier of the user whose bookings are to be retrieved.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains a collection of bookings for the user.</returns>
		Task<IEnumerable<Booking>> GetUserBookingsAsync(string userId);
		/// <summary>
		/// Asynchronously retrieves a booking by its unique identifier.
		/// </summary>
		/// <param name="id">The unique identifier of the booking to retrieve.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the booking if found; otherwise, null.</returns>
		Task<Booking?> GetBookingByIdAsync(int id);
		/// <summary>
		/// Asynchronously retrieves all bookings for the specified event.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains a collection of bookings for the
		/// event.</returns>
		Task<IEnumerable<Booking>> GetEventBookingsAsync(int eventId);
		/// <summary>
		/// Asynchronously retrieves the total number of booked tickets for a specified event.
		/// </summary>
		/// <param name="eventId">The unique identifier of the event.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the total number of booked tickets.</returns>
		Task<int> GetTotalBookedTicketsAsync(int eventId);
		/// <summary>
		/// Asynchronously creates a booking for a specified user and event with the given number of tickets.
		/// </summary>
		/// <param name="userId">The identifier of the user making the booking.</param>
		/// <param name="eventId">The identifier of the event to book.</param>
		/// <param name="ticketsCount">The number of tickets to book.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the created Booking if successful;
		/// otherwise, null.</returns>
		Task<Booking?> CreateBookingAsync(string userId, int eventId, int ticketsCount);
		/// <summary>
		/// Cancels a booking for the specified user asynchronously.
		/// </summary>
		/// <param name="bookingId">The unique identifier of the booking to cancel.</param>
		/// <param name="userId">The identifier of the user requesting the cancellation.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains true if the cancellation was
		/// successful; otherwise, false.</returns>
		Task<bool> CancelBookingAsync(int bookingId, string userId);
		/// <summary>
		/// Determines whether the specified user can book the given number of tickets for an event asynchronously.
		/// </summary>
		/// <param name="userId">The unique identifier of the user.</param>
		/// <param name="eventId">The unique identifier of the event.</param>
		/// <param name="ticketsCount">The number of tickets the user wants to book.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains true if the user can book the event;
		/// otherwise, false.</returns>
		Task<bool> CanUserBookEventAsync(string userId, int eventId, int ticketsCount);
		/// <summary>
		/// Determines whether a booking is owned by the specified user.
		/// </summary>
		/// <param name="bookingId">The unique identifier of the booking.</param>
		/// <param name="userId">The unique identifier of the user.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains true if the booking is owned by the
		/// user; otherwise, false.</returns>
		Task<bool> IsBookingOwnedByUserAsync(int bookingId, string userId);
		/// <summary>
		/// Asynchronously determines whether a booking with the specified ID exists.
		/// </summary>
		/// <param name="bookingId">The unique identifier of the booking to check.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains true if the booking exists; otherwise,
		/// false.</returns>
		Task<bool> BookingExistsAsync(int bookingId);
	}
}