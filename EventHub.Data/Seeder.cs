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
			await context.Database.MigrateAsync();

			await SeedRolesAsync(roleManager);

			if (context.Categories.Any())
			{
				return;
			}

			var categories = new List<Category>
			{
				new Category { Name = "Музика" },
				new Category { Name = "Спорт" },
				new Category { Name = "Технологии" },
				new Category { Name = "Изкуство и култура" },
				new Category { Name = "Бизнес" },
				new Category { Name = "Храна и напитки" },
				new Category { Name = "Образование" },
				new Category { Name = "Развлечения" }
			};
			await context.Categories.AddRangeAsync(categories);
			await context.SaveChangesAsync();

			var venues = new List<Venue>
			{
				new Venue
				{
					Name = "Национален дворец на културата",
					Address = "пл. България 1, София",
					Capacity = 4000
				},
				new Venue
				{
					Name = "Арена София",
					Address = "Хладилника, София",
					Capacity = 12000
				},
				new Venue
				{
					Name = "София Тех Парк",
					Address = "бул. Царигradско шосе 111, София",
					Capacity = 500
				},
				new Venue
				{
					Name = "Интер Експо Център",
					Address = "бул. Цариградско шосе 147, София",
					Capacity = 3000
				},
				new Venue
				{
					Name = "Театър София",
					Address = "бул. Янко Сакъзов 5, София",
					Capacity = 800
				}
			};
			await context.Venues.AddRangeAsync(venues);
			await context.SaveChangesAsync();

			var events = new List<Event>
			{
				new Event
				{
					Title = "Рок фестивал 2026",
					Organizer = "Мюзик Ивънтс ООД",
					Description = "Ежегоден рок фестивал с участието на топ групи от цял свят. Изживейте незабравими изпълнения и страхотна атмосфера!",
					Date = new DateTime(2026, 6, 15, 18, 0, 0),
					TicketPrice = 89.99m,
					ImageUrl = "https://images.unsplash.com/photo-1459749411175-04bf5292ceea?w=800",
					VenueId = venues[1].Id,
					Categories = new List<Category> { categories[0], categories[7] }
				},
				new Event
				{
					Title = "Технологична конференция 2026",
					Organizer = "ТехКон България",
					Description = "Водеща технологична конференция с лектори от Microsoft, Google и други технологични гиганти. Създайте мрежа от контакти с професионалисти от индустрията.",
					Date = new DateTime(2026, 4, 20, 9, 0, 0),
					TicketPrice = 150.00m,
					ImageUrl = "https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=800",
					VenueId = venues[2].Id,
					Categories = new List<Category> { categories[2], categories[4] }
				},
				new Event
				{
					Title = "Финал на Шампионска лига - гледане",
					Organizer = "Спортс Арена",
					Description = "Гледайте финала на Шампионска лига на гигантски екрани заедно с футболни фенове. Налични храна и напитки.",
					Date = new DateTime(2026, 5, 30, 20, 0, 0),
					TicketPrice = 25.00m,
					ImageUrl = "https://images.unsplash.com/photo-1579952363873-27f3bade9f55?w=800",
					VenueId = venues[1].Id,
					Categories = new List<Category> { categories[1], categories[7] }
				},
				new Event
				{
					Title = "Вечер на класическа музика",
					Organizer = "Софийска филхармония",
					Description = "Вечер на класически шедьоври в изпълнение на Софийска филхармония. Включва произведения на Моцарт и Бетовен.",
					Date = new DateTime(2026, 3, 12, 19, 30, 0),
					TicketPrice = 45.00m,
					ImageUrl = "https://images.unsplash.com/photo-1465847899084-d164df4dedc6?w=800",
					VenueId = venues[4].Id,
					Categories = new List<Category> { categories[0], categories[3] }
				},
				new Event
				{
					Title = "Вечер на стартъп презентации",
					Organizer = "Иновационен хъб",
					Description = "Гледайте иновативни стартъпи как представят идеите си пред инвеститори. Страхотна възможност за networking между предприемачи и инвеститори.",
					Date = new DateTime(2026, 4, 5, 18, 0, 0),
					TicketPrice = 0.00m,
					ImageUrl = "https://images.unsplash.com/photo-1559136555-9303baea8ebd?w=800",
					VenueId = venues[2].Id,
					Categories = new List<Category> { categories[2], categories[4] }
				},
				new Event
				{
					Title = "Фестивал на храната и виното",
					Organizer = "Кулинарни майстори",
					Description = "Опитайте изключителни ястия от топ готвачи, съчетани с премиум вина. Включени кулинарни демонстрации и дегустации.",
					Date = new DateTime(2026, 7, 10, 12, 0, 0),
					TicketPrice = 65.00m,
					ImageUrl = "https://images.unsplash.com/photo-1555939594-58d7cb561ad1?w=800",
					VenueId = venues[3].Id,
					Categories = new List<Category> { categories[5] }
				},
				new Event
				{
					Title = "Уъркшоп по дигитален маркетинг",
					Organizer = "Маркетинг академия",
					Description = "Практически уъркшоп, покриващ SEO, маркетинг в социалните мрежи и стратегия за съдържание. Перфектен за маркетолози и бизнес собственици.",
					Date = new DateTime(2026, 3, 25, 10, 0, 0),
					TicketPrice = 120.00m,
					ImageUrl = "https://images.unsplash.com/photo-1552664730-d307ca884978?w=800",
					VenueId = venues[2].Id,
					Categories = new List<Category> { categories[4], categories[6] }
				},
				new Event
				{
					Title = "Изложба на съвременно изкуство",
					Organizer = "Национална галерия",
					Description = "Изложба с участието на съвременни български и международни художници. Откриване с присъствието на художниците.",
					Date = new DateTime(2026, 5, 1, 17, 0, 0),
					TicketPrice = 15.00m,
					ImageUrl = "https://images.unsplash.com/photo-1547826039-bfc35e0f1ea8?w=800",
					VenueId = venues[4].Id,
					Categories = new List<Category> { categories[3] }
				}
			};
			await context.Events.AddRangeAsync(events);
			await context.SaveChangesAsync();

			await SeedAdminUserAsync(userManager);

			var user1 = new ApplicationUser
			{
				UserName = "john.doe@example.com",
				Email = "john.doe@example.com",
				FirstName = "Иван",
				LastName = "Иванов",
				EmailConfirmed = true
			};

			var user2 = new ApplicationUser
			{
				UserName = "jane.smith@example.com",
				Email = "jane.smith@example.com",
				FirstName = "Мария",
				LastName = "Петрова",
				EmailConfirmed = true
			};

			var result1 = await userManager.CreateAsync(user1, "Test123!");
			var result2 = await userManager.CreateAsync(user2, "Test123!");

			if (result1.Succeeded)
			{
				await userManager.AddToRoleAsync(user1, "User");
			}

			if (result2.Succeeded)
			{
				await userManager.AddToRoleAsync(user2, "User");
			}

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
			if (!await roleManager.RoleExistsAsync("Admin"))
			{
				await roleManager.CreateAsync(new IdentityRole("Admin"));
			}

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
					FirstName = "Админ",
					LastName = "Потребител",
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
				if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
				{
					await userManager.AddToRoleAsync(adminUser, "Admin");
				}
			}
		}
	}
}