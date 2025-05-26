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
		public async Task<ActionResult<List<CategoryResponse>>> GetCategoryListAsync(CancellationToken ct)
		{
			try
			{
				var categoryList = await _categoryService.GetListAsync(ct);

				return Ok(categoryList);
			}
			catch (OperationCanceledException)
			{
				return BadRequest();
			}
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<CategoryResponse>> GetCategoryAsync(int id, CancellationToken ct)
		{
			try
			{
				var category = await _categoryService.GetAsync(id, ct);

				if (category == null)
				{
					return NotFound();
				}

				return Ok(category);
			}
			catch (OperationCanceledException)
			{
				return BadRequest();
			}
		}

		[HttpPost]
		public async Task<ActionResult<CategoryResponse>> CreateCategoryAsync(CategoryRequest request, CancellationToken ct)
		{
			try
			{
				var category = await _categoryService.CreateAsync(request, ct);

				return CreatedAtAction(nameof(CreateCategoryAsync), category);
			}
			catch (OperationCanceledException)
			{
				return BadRequest();
			}
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<CategoryResponse>> UpdateCategoryAsync(int id, CategoryRequest request, CancellationToken ct)
		{
			try
			{
				var category = await _categoryService.GetAsync(id, ct);

				if (category == null)
				{
					return NotFound();
				}

				var updatedCategory = await _categoryService.UpdateAsync(id, request, ct);

				return Ok(updatedCategory);
			}
			catch (OperationCanceledException)
			{
				return BadRequest();
			}
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult> DeleteCategoryAsync(int id, CancellationToken ct)
		{
			try
			{
				var category = await _categoryService.GetAsync(id, ct);

				if (category == null)
				{
					return NotFound();
				}

				await _categoryService.DeleteAsync(id, ct);

				return Ok();
			}
			catch (OperationCanceledException)
			{
				return BadRequest();
			}
		}
	}
}
