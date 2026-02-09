using EventHub.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Data.Seeding
{
    public static class Seeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // 1. Seed Roles
            await SeedRolesAsync(roleManager);

            // 2. Seed Admin User
            await SeedAdminAsync(userManager);

            // 3. Seed Categories
            await SeedCategoriesAsync(dbContext);

            // 4. Seed Venues
            await SeedVenuesAsync(dbContext);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Administrator"))
            {
                await roleManager.CreateAsync(new IdentityRole("Administrator"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }
        }

        private static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
        {
            if (await userManager.FindByEmailAsync("admin@eventhub.com") == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin@eventhub.com",
                    Email = "admin@eventhub.com",
                    FirstName = "System",
                    LastName = "Admin",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "Admin123!"); // Сложи си сигурна парола

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Administrator");
                }
            }
        }

        private static async Task SeedCategoriesAsync(ApplicationDbContext db)
        {
            if (db.Categories.Any()) return;

            var categories = new List<Category>
            {
                new Category { Name = "Музика" },
                new Category { Name = "Спорт" },
                new Category { Name = "Театър" },
                new Category { Name = "Кино" },
                new Category { Name = "Обучения" },
                new Category { Name = "Други" }
            };

            await db.Categories.AddRangeAsync(categories);
            await db.SaveChangesAsync();
        }

        private static async Task SeedVenuesAsync(ApplicationDbContext db)
        {
            if (db.Venues.Any()) return;

            var venues = new List<Venue>
            {
                new Venue { Name = "НДК - Зала 1", Address = "гр. София, бул. Витоша 1", Capacity = 3000 },
                new Venue { Name = "Арена София", Address = "гр. София, бул. Асен Йорданов 1", Capacity = 12000 },
                new Venue { Name = "Античен театър", Address = "гр. Пловдив, ул. Цар Ивайло 2", Capacity = 3500 },
                new Venue { Name = "Онлайн събитие", Address = "Zoom / Microsoft Teams", Capacity = 1000 }
            };

            await db.Venues.AddRangeAsync(venues);
            await db.SaveChangesAsync();
        }
    }
}