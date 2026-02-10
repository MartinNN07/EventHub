namespace EventHub.Web.Models
{
	public class MyBookingsViewModel
	{
		public List<BookingViewModel> UpcomingBookings { get; set; } = new();
		public List<BookingViewModel> PastBookings { get; set; } = new();
		public int TotalBookings => UpcomingBookings.Count + PastBookings.Count;
		public int TotalUpcomingEvents => UpcomingBookings.Count;
		public int TotalTicketsBooked => UpcomingBookings.Sum(b => b.TicketsCount) + PastBookings.Sum(b => b.TicketsCount);
		public decimal TotalSpent => UpcomingBookings.Sum(b => b.TotalPrice) + PastBookings.Sum(b => b.TotalPrice);
	}
}
