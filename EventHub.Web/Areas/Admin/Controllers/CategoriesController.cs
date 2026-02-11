using EventHub.Common;
using EventHub.Data.Models;
using EventHub.Services.Interfaces;
using EventHub.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Web.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = RoleConstants.AdminRole)]
	public class CategoriesController : Controller
	{
		private readonly ICategoryService _categoryService;
		private readonly ILogger<CategoriesController> _logger;

		public CategoriesController(
			ICategoryService categoryService,
			ILogger<CategoriesController> logger)
		{
			_categoryService = categoryService;
			_logger = logger;
		}

		// GET: Admin/Categories
		public async Task<IActionResult> Index()
		{
			try
			{
				var categories = await _categoryService.GetAllCategoriesAsync();
				var viewModels = categories.Select(c => new CategoryViewModel
				{
					Id = c.Id,
					Name = c.Name,
					EventCount = c.Events?.Count ?? 0
				}).OrderBy(c => c.Name).ToList();

				return View(viewModels);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading categories");
				TempData["Error"] = "Unable to load categories. Please try again.";
				return View(new List<CategoryViewModel>());
			}
		}

		// GET: Admin/Categories/Create
		public IActionResult Create()
		{
			return View(new CreateCategoryViewModel());
		}

		// POST: Admin/Categories/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateCategoryViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			try
			{
				// Check if category name already exists
				var nameExists = await _categoryService.CategoryNameExistsAsync(model.Name);
				if (nameExists)
				{
					ModelState.AddModelError("Name", "A category with this name already exists.");
					return View(model);
				}

				var category = new Category
				{
					Name = model.Name
				};

				await _categoryService.CreateCategoryAsync(category);
				TempData["Success"] = "Category created successfully!";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error creating category");
				ModelState.AddModelError("", "An error occurred while creating the category.");
				return View(model);
			}
		}

		// GET: Admin/Categories/Edit/5
		public async Task<IActionResult> Edit(int id)
		{
			try
			{
				var category = await _categoryService.GetCategoryByIdAsync(id);
				if (category == null)
				{
					TempData["Error"] = "Category not found.";
					return RedirectToAction(nameof(Index));
				}

				var viewModel = new EditCategoryViewModel
				{
					Id = category.Id,
					Name = category.Name
				};

				return View(viewModel);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading category for edit");
				TempData["Error"] = "Unable to load category.";
				return RedirectToAction(nameof(Index));
			}
		}

		// POST: Admin/Categories/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, EditCategoryViewModel model)
		{
			if (id != model.Id)
			{
				return NotFound();
			}

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			try
			{
				// Check if category name already exists (excluding current category)
				var nameExists = await _categoryService.CategoryNameExistsAsync(model.Name, model.Id);
				if (nameExists)
				{
					ModelState.AddModelError("Name", "A category with this name already exists.");
					return View(model);
				}

				var category = new Category
				{
					Id = model.Id,
					Name = model.Name
				};

				var result = await _categoryService.UpdateCategoryAsync(category);
				if (result == null)
				{
					TempData["Error"] = "Category not found.";
					return RedirectToAction(nameof(Index));
				}

				TempData["Success"] = "Category updated successfully!";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating category");
				ModelState.AddModelError("", "An error occurred while updating the category.");
				return View(model);
			}
		}

		// GET: Admin/Categories/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var category = await _categoryService.GetCategoryWithEventsAsync(id);
				if (category == null)
				{
					TempData["Error"] = "Category not found.";
					return RedirectToAction(nameof(Index));
				}

				var viewModel = new CategoryViewModel
				{
					Id = category.Id,
					Name = category.Name,
					EventCount = category.Events?.Count ?? 0
				};

				return View(viewModel);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading category for deletion");
				TempData["Error"] = "Unable to load category.";
				return RedirectToAction(nameof(Index));
			}
		}

		// POST: Admin/Categories/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			try
			{
				var category = await _categoryService.GetCategoryWithEventsAsync(id);
				if (category == null)
				{
					TempData["Error"] = "Category not found.";
					return RedirectToAction(nameof(Index));
				}

				// Check if category has events
				if (category.Events != null && category.Events.Any())
				{
					TempData["Error"] = $"Cannot delete category '{category.Name}' because it has {category.Events.Count} associated event(s). Please remove or reassign these events first.";
					return RedirectToAction(nameof(Index));
				}

				var result = await _categoryService.DeleteCategoryAsync(id);
				if (result)
				{
					TempData["Success"] = "Category deleted successfully!";
				}
				else
				{
					TempData["Error"] = "Unable to delete category.";
				}

				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error deleting category");
				TempData["Error"] = "An error occurred while deleting the category.";
				return RedirectToAction(nameof(Index));
			}
		}
	}
}
