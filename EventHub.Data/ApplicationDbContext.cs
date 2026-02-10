using EventHub.Data.Models;
using EventHub.Data.Seeding;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Event> Events { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Venue> Venues { get; set; }
		public DbSet<Booking> Bookings { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// many to many 
			builder.Entity<Event>()
				.HasMany(e => e.Categories)
				.WithMany(c => c.Events)
				.UsingEntity(j => j.ToTable("EventCategories"));

			// deleting behaiviour restriction
			builder.Entity<Booking>()
				.HasOne(b => b.Event)
				.WithMany(e => e.Bookings)
				.HasForeignKey(b => b.EventId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Entity<Booking>()
				.HasOne(b => b.Buyer)
				.WithMany(u => u.Bookings)
				.HasForeignKey(b => b.BuyerId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}