using EventHub.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Data.Seeding
{
    public static class Seeder
    {
        public static async Task SeedAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Ensure database is created
            await context.Database.MigrateAsync();

            // Seed Roles FIRST (before users)
            await SeedRolesAsync(roleManager);

            // Check if data already exists
            if (context.Categories.Any())
            {
                return; // Database has been seeded
            }

            // Seed Categories
            var categories = new List<Category>
            {
                new Category { Name = "Music" },
                new Category { Name = "Sports" },
                new Category { Name = "Technology" },
                new Category { Name = "Arts & Culture" },
                new Category { Name = "Business" },
                new Category { Name = "Food & Drink" },
                new Category { Name = "Education" },
                new Category { Name = "Entertainment" }
            };
            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            // Seed Venues
            var venues = new List<Venue>
            {
                new Venue
                {
                    Name = "National Palace of Culture",
                    Address = "1 Bulgaria Square, Sofia",
                    Capacity = 4000
                },
                new Venue
                {
                    Name = "Arena Sofia",
                    Address = "Hladilnika, Sofia",
                    Capacity = 12000
                },
                new Venue
                {
                    Name = "Sofia Tech Park",
                    Address = "111 Tsarigradsko Shose, Sofia",
                    Capacity = 500
                },
                new Venue
                {
                    Name = "Inter Expo Center",
                    Address = "147 Tsarigradsko Shose, Sofia",
                    Capacity = 3000
                },
                new Venue
                {
                    Name = "Theater Sofia",
                    Address = "5 Yanko Sakazov Blvd, Sofia",
                    Capacity = 800
                }
            };
            await context.Venues.AddRangeAsync(venues);
            await context.SaveChangesAsync();

            // Seed Events
            var events = new List<Event>
            {
                new Event
                {
                    Title = "Rock Festival 2026",
                    Organizer = "Music Events Ltd",
                    Description = "Annual rock music festival featuring top bands from around the world. Experience unforgettable performances and great atmosphere!",
                    Date = new DateTime(2026, 6, 15, 18, 0, 0),
                    TicketPrice = 89.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1459749411175-04bf5292ceea?w=800",
                    VenueId = venues[1].Id,
                    Categories = new List<Category> { categories[0], categories[7] }
                },
                new Event
                {
                    Title = "Tech Summit 2026",
                    Organizer = "TechCon Bulgaria",
                    Description = "Leading technology conference with speakers from Microsoft, Google, and other tech giants. Network with industry professionals.",
                    Date = new DateTime(2026, 4, 20, 9, 0, 0),
                    TicketPrice = 150.00m,
                    ImageUrl = "https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=800",
                    VenueId = venues[2].Id,
                    Categories = new List<Category> { categories[2], categories[4] }
                },
                new Event
                {
                    Title = "Champions League Final Viewing",
                    Organizer = "Sports Arena",
                    Description = "Watch the Champions League final on giant screens with fellow football fans. Food and drinks available.",
                    Date = new DateTime(2026, 5, 30, 20, 0, 0),
                    TicketPrice = 25.00m,
                    ImageUrl = "https://images.unsplash.com/photo-1579952363873-27f3bade9f55?w=800",
                    VenueId = venues[1].Id,
                    Categories = new List<Category> { categories[1], categories[7] }
                },
                new Event
                {
                    Title = "Classical Music Evening",
                    Organizer = "Sofia Philharmonic",
                    Description = "Evening of classical masterpieces performed by the Sofia Philharmonic Orchestra. Featuring works by Mozart and Beethoven.",
                    Date = new DateTime(2026, 3, 12, 19, 30, 0),
                    TicketPrice = 45.00m,
                    ImageUrl = "https://images.unsplash.com/photo-1465847899084-d164df4dedc6?w=800",
                    VenueId = venues[4].Id,
                    Categories = new List<Category> { categories[0], categories[3] }
                },
                new Event
                {
                    Title = "Startup Pitch Night",
                    Organizer = "Innovation Hub",
                    Description = "Watch innovative startups pitch their ideas to investors. Great networking opportunity for entrepreneurs and investors.",
                    Date = new DateTime(2026, 4, 5, 18, 0, 0),
                    TicketPrice = 0.00m,
                    ImageUrl = "https://images.unsplash.com/photo-1559136555-9303baea8ebd?w=800",
                    VenueId = venues[2].Id,
                    Categories = new List<Category> { categories[2], categories[4] }
                },
                new Event
                {
                    Title = "Food & Wine Festival",
                    Organizer = "Culinary Masters",
                    Description = "Taste exceptional dishes from top chefs paired with premium wines. Cooking demonstrations and tastings included.",
                    Date = new DateTime(2026, 7, 10, 12, 0, 0),
                    TicketPrice = 65.00m,
                    ImageUrl = "https://images.unsplash.com/photo-1555939594-58d7cb561ad1?w=800",
                    VenueId = venues[3].Id,
                    Categories = new List<Category> { categories[5] }
                },
                new Event
                {
                    Title = "Digital Marketing Workshop",
                    Organizer = "Marketing Academy",
                    Description = "Hands-on workshop covering SEO, social media marketing, and content strategy. Perfect for marketers and business owners.",
                    Date = new DateTime(2026, 3, 25, 10, 0, 0),
                    TicketPrice = 120.00m,
                    ImageUrl = "https://images.unsplash.com/photo-1552664730-d307ca884978?w=800",
                    VenueId = venues[2].Id,
                    Categories = new List<Category> { categories[4], categories[6] }
                },
                new Event
                {
                    Title = "Contemporary Art Exhibition",
                    Organizer = "National Gallery",
                    Description = "Exhibition featuring contemporary Bulgarian and international artists. Opening reception with the artists.",
                    Date = new DateTime(2026, 5, 1, 17, 0, 0),
                    TicketPrice = 15.00m,
                    ImageUrl = "https://images.unsplash.com/photo-1547826039-bfc35e0f1ea8?w=800",
                    VenueId = venues[4].Id,
                    Categories = new List<Category> { categories[3] }
                }
            };
            await context.Events.AddRangeAsync(events);
            await context.SaveChangesAsync();

            // Seed Admin User
            await SeedAdminUserAsync(userManager);

            // Seed Sample Users
            var user1 = new ApplicationUser
            {
                UserName = "john.doe@example.com",
                Email = "john.doe@example.com",
                FirstName = "John",
                LastName = "Doe",
                EmailConfirmed = true
            };

            var user2 = new ApplicationUser
            {
                UserName = "jane.smith@example.com",
                Email = "jane.smith@example.com",
                FirstName = "Jane",
                LastName = "Smith",
                EmailConfirmed = true
            };

            var result1 = await userManager.CreateAsync(user1, "Test123!");
            var result2 = await userManager.CreateAsync(user2, "Test123!");

            // Assign User role to sample users
            if (result1.Succeeded)
            {
                await userManager.AddToRoleAsync(user1, "User");
            }

            if (result2.Succeeded)
            {
                await userManager.AddToRoleAsync(user2, "User");
            }

            // Seed Sample Bookings
            var bookings = new List<Booking>
            {
                new Booking
                {
                    BookingDate = DateTime.Now.AddDays(-5),
                    TicketsCount = 2,
                    BuyerId = user1.Id,
                    EventId = events[0].Id
                },
                new Booking
                {
                    BookingDate = DateTime.Now.AddDays(-3),
                    TicketsCount = 1,
                    BuyerId = user1.Id,
                    EventId = events[1].Id
                },
                new Booking
                {
                    BookingDate = DateTime.Now.AddDays(-2),
                    TicketsCount = 4,
                    BuyerId = user2.Id,
                    EventId = events[2].Id
                }
            };
            await context.Bookings.AddRangeAsync(bookings);
            await context.SaveChangesAsync();
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            // Create Admin role
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Create User role
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            var adminEmail = "admin@eventhub.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
            else
            {
                // Ensure existing admin has the admin role
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}