using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet]
		public async Task<ActionResult<List<CategoryResponse>>> GetCategoryListAsync()
		{
			var categoryList = await _categoryService.GetListAsync();
			return Ok(categoryList);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<CategoryResponse>> GetCategoryAsync(int id)
		{
			var category = await _categoryService.GetAsync(id);

			if (category == null)
			{
				return NotFound();
			}

			return Ok(category);
		}

		[HttpPost]
		public async Task<ActionResult<CategoryResponse>> CreateCategoryAsync(CategoryRequest request)
		{
			var category = await _categoryService.CreateAsync(request);

			return CreatedAtAction(nameof(CreateCategoryAsync), category);
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<CategoryResponse>> UpdateCategoryAsync(int id, CategoryRequest request)
		{
			var category = await _categoryService.GetAsync(id);

			if (category == null)
			{
				return NotFound();
			}

			var updatedCategory = await _categoryService.UpdateAsync(id, request);

			return Ok(updatedCategory);
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult> DeleteCategoryAsync(int id)
		{
			var category = await _categoryService.GetAsync(id);

			if (category == null)
			{
				return NotFound();
			}

			await _categoryService.DeleteAsync(id);

			return Ok();
		}
	}
}
