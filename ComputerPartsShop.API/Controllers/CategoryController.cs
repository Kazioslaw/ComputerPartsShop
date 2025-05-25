using ComputerPartsShop.Domain.DTOs;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class CategoryController : ControllerBase
	{
		private readonly CategoryService _categoryService;

		public CategoryController(CategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet]
		public async Task<ActionResult<List<CategoryResponse>>> GetCategoryList()
		{
			var categoryList = await _categoryService.GetList();
			return Ok(categoryList);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<CategoryResponse>> GetCategory(int id)
		{
			var category = await _categoryService.Get(id);

			if (category == null)
			{
				return NotFound();
			}

			return Ok(category);
		}

		[HttpPost]
		public async Task<ActionResult<CategoryResponse>> CreateCategory(CategoryRequest request)
		{
			var category = await _categoryService.Create(request);

			return CreatedAtAction(nameof(CreateCategory), category);
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<CategoryResponse>> UpdateCategory(int id, CategoryRequest request)
		{
			var category = await _categoryService.Get(id);

			if (category == null)
			{
				return NotFound();
			}

			var updatedCategory = await _categoryService.Update(id, request);

			return Ok(updatedCategory);
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult> DeleteCategory(int id)
		{
			var category = await _categoryService.Get(id);

			if (category == null)
			{
				return NotFound();
			}

			await _categoryService.Delete(id);

			return Ok();
		}
	}
}
