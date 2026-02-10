using EventHub.Data.Models;

namespace EventHub.Services.Interfaces
{
	public interface ICategoryService
	{
		/// <summary>
		/// Asynchronously retrieves all categories.
		/// </summary>
		/// <returns>A task that represents the asynchronous operation. The task result contains a collection of categories.</returns>
		Task<IEnumerable<Category>> GetAllCategoriesAsync();
		/// <summary>
		/// Asynchronously retrieves a category by its unique identifier.
		/// </summary>
		/// <param name="id">The unique identifier of the category to retrieve.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the category if found; otherwise,
		/// null.</returns>
		Task<Category?> GetCategoryByIdAsync(int id);
		/// <summary>
		/// Asynchronously retrieves a category by its identifier, including its associated events.
		/// </summary>
		/// <param name="id">The unique identifier of the category.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the category with its events if found;
		/// otherwise, null.</returns>
		Task<Category?> GetCategoryWithEventsAsync(int id);
		/// <summary>
		/// Asynchronously creates a new category.
		/// </summary>
		/// <param name="category">The category to create.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the created Category.</returns>
		Task<Category> CreateCategoryAsync(Category category);
		/// <summary>
		/// Asynchronously updates an existing category.
		/// </summary>
		/// <param name="category">The category entity with updated information.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains the updated category, or null if not
		/// found.</returns>
		Task<Category?> UpdateCategoryAsync(Category category);
		/// <summary>
		/// Asynchronously deletes a category by its identifier.
		/// </summary>
		/// <param name="id">The unique identifier of the category to delete.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains true if the category was deleted;
		/// otherwise, false.</returns>
		Task<bool> DeleteCategoryAsync(int id);
		/// <summary>
		/// Asynchronously determines whether a category with the specified ID exists.
		/// </summary>
		/// <param name="id">The ID of the category to check for existence.</param>
		/// <returns>A task that represents the asynchronous operation. The task result contains true if the category exists;
		/// otherwise, false.</returns>
		Task<bool> CategoryExistsAsync(int id);
		/// <summary>
		/// Determines whether a category with the specified name exists, optionally excluding a category by its ID.
		/// </summary>
		/// <param name="name">The category name to check for existence.</param>
		/// <param name="excludeId">An optional category ID to exclude from the check.</param>
		/// <returns>True if a category with the specified name exists; otherwise, false.</returns>
		Task<bool> CategoryNameExistsAsync(string name, int? excludeId = null);
	}
}