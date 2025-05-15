using ComputerPartsShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoryController : ControllerBase
	{
		static List<Category> Categories = new()
		{
			new Category { ID = 1, Name = "CPU/Processor" },
			new Category { ID = 2, Name = "RAM Memory" },
			new Category { ID = 3, Name = "Motherboard"},
			new Category { ID = 4, Name = "GPU/Graphics Card"},
			new Category { ID = 5, Name = "Computer Case"},
			new Category { ID = 6, Name = "Power Supply"},
			new Category { ID = 7, Name = "Hard Drive"},
			new Category { ID = 8, Name = "Solid State Drive"}
		};

		/// <summary> 
		/// Get method to get list of categories
		/// </summary>
		/// <returns>
		/// List of categories.
		/// </returns>
		[HttpGet]
		public ActionResult<List<Category>> GetCategoryList()
		{
			return Ok(Categories);
		}

		/// <summary>
		/// Get method to get category by its id
		/// </summary>
		/// <param name="id">Category id</param>
		/// <returns>Category by its id</returns>

		[HttpGet("{id}")]
		public ActionResult<Category> GetCategory(int id)
		{
			var category = Categories.FirstOrDefault(c => c.ID == id);
			if (category == null)
			{
				return NotFound();
			}
			return Ok(category);
		}

		/// <summary>
		/// Post method to create new category
		/// </summary>
		/// <param name="category">Category model to create</param>
		/// <returns>Newly created category with id.</returns>

		[HttpPost]
		public ActionResult CreateCategory(Category category)
		{
			category.ID = Categories.Count + 1;
			Categories.Add(category);
			return CreatedAtAction(nameof(CreateCategory), category);
		}

		/// <summary>
		/// Put method to update category by its id
		/// </summary>
		/// <param name="id">Category ID to update</param>
		/// <param name="updatedCategory">Category model to update</param>
		/// <returns>Properly updated category</returns>

		[HttpPut("{id}")]
		public ActionResult<Category> UpdateCategory(int id, Category updatedCategory)
		{
			var category = Categories.FirstOrDefault(c => c.ID == id);
			if (category == null)
			{
				return NotFound();
			}
			category.Name = updatedCategory.Name;

			return Ok(category);
		}

		/// <summary>
		/// Delete method to delete category by its id
		/// </summary>
		/// <param name="id">Category ID to delete</param>
		/// <returns>Information about successful deletion</returns>

		[HttpDelete("{id}")]
		public ActionResult DeleteCategory(int id)
		{
			var category = Categories.FirstOrDefault(c => c.ID == id);
			if (category == null)
			{
				return NotFound();
			}
			Categories.Remove(category);

			return Ok();
		}
	}
}
