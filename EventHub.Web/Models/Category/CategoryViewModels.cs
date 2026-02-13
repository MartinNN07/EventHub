using System.ComponentModel.DataAnnotations;

namespace EventHub.Web.Models.Category
{
	public class CategoryViewModel
	{
		public int Id { get; set; }

		[Required]
		[StringLength(50)]
		public string Name { get; set; } = null!;

		public int EventCount { get; set; }
	}

	public class CreateCategoryViewModel
	{
		[Required(ErrorMessage = "Category name is required")]
		[StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters")]
		[Display(Name = "Category Name")]
		public string Name { get; set; } = null!;
	}

	public class EditCategoryViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Category name is required")]
		[StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters")]
		[Display(Name = "Category Name")]
		public string Name { get; set; } = null!;
	}
}
