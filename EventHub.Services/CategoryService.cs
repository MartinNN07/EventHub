using EventHub.Data;
using EventHub.Data.Models;
using EventHub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Services.Implementations
{
	public class CategoryService : ICategoryService
	{
		private readonly ApplicationDbContext _context;

		public CategoryService(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
		{
			return await _context.Categories
				.OrderBy(c => c.Name)
				.ToListAsync();
		}

		public async Task<Category?> GetCategoryByIdAsync(int id)
		{
			return await _context.Categories
				.FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task<Category?> GetCategoryWithEventsAsync(int id)
		{
			return await _context.Categories
				.Include(c => c.Events)
					.ThenInclude(e => e.Venue)
				.FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task<Category> CreateCategoryAsync(Category category)
		{
			_context.Categories.Add(category);
			await _context.SaveChangesAsync();
			return category;
		}

		public async Task<Category?> UpdateCategoryAsync(Category category)
		{
			var existingCategory = await _context.Categories.FindAsync(category.Id);
			if (existingCategory == null)
			{
				return null;
			}

			existingCategory.Name = category.Name;
			await _context.SaveChangesAsync();
			return existingCategory;
		}

		public async Task<bool> DeleteCategoryAsync(int id)
		{
			var category = await _context.Categories
				.Include(c => c.Events)
				.FirstOrDefaultAsync(c => c.Id == id);

			if (category == null)
			{
				return false;
			}

			if (category.Events.Any())
			{
				return false;
			}

			_context.Categories.Remove(category);
			await _context.SaveChangesAsync();
			return true;
		}
		public async Task<bool> CategoryExistsAsync(int id)
		{
			return await _context.Categories.AnyAsync(c => c.Id == id);
		}

		public async Task<bool> CategoryNameExistsAsync(string name, int? excludeId = null)
		{
			if (excludeId.HasValue)
			{
				return await _context.Categories
					.AnyAsync(c => c.Name.ToLower() == name.ToLower() && c.Id != excludeId.Value);
			}

			return await _context.Categories
				.AnyAsync(c => c.Name.ToLower() == name.ToLower());
		}
	}
}