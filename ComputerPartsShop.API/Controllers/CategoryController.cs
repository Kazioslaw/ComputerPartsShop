using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryService _categoryService;
		private readonly IValidator<CategoryRequest> _categoryValidator;

		public CategoryController(ICategoryService categoryService, IValidator<CategoryRequest> categoryValidator)
		{
			_categoryService = categoryService;
			_categoryValidator = categoryValidator;
		}

		/// <summary>
		/// Asynchronously retrieves all categories.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the list of categories</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>List of categories</returns>
		[HttpGet]
		public async Task<IActionResult> GetCategoryListAsync(CancellationToken ct)
		{
			try
			{
				var categoryList = await _categoryService.GetListAsync(ct);

				return Ok(categoryList);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously retrieves an category by its ID.
		/// </summary>
		/// <param name="id">Category ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the category</response>
		/// <response code="404">Returns if the category was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Category</returns>
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetCategoryAsync(int id, CancellationToken ct)
		{
			try
			{
				var category = await _categoryService.GetAsync(id, ct);

				if (category == null)
				{
					return NotFound("Category not found");
				}

				return Ok(category);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously retrieves an category by its name.
		/// </summary>
		/// <param name="name">Category name</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the category</response>
		/// <response code="404">Returns if the category was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Category</returns>
		[HttpGet("{name}")]
		public async Task<IActionResult> GetCategoryByNameAsync(string name, CancellationToken ct)
		{
			try
			{
				var category = await _categoryService.GetByNameAsync(name, ct);

				if (category == null)
				{
					return NotFound("Category not found");
				}

				return Ok(category);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously creates a new category.
		/// </summary>
		/// <param name="request">Category model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the created category</response>		
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the category not created</response>
		/// <returns>Created category</returns>
		[HttpPost]
		public async Task<IActionResult> CreateCategoryAsync(CategoryRequest request, CancellationToken ct)
		{
			try
			{
				var validation = await _categoryValidator.ValidateAsync(request);

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
					return BadRequest(errors);
				}

				var category = await _categoryService.CreateAsync(request, ct);

				if (category == null)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Create failed");
				}

				return Created(nameof(GetCategoryAsync), category);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously updates an category by its ID.
		/// </summary>
		/// <param name="id">Category ID</param>
		/// <param name="request">Updated category model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the updated category</response>
		/// <response code="404">Returns if the category was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Updated category</returns>
		[HttpPut("{id:int}")]
		public async Task<IActionResult> UpdateCategoryAsync(int id, CategoryRequest request, CancellationToken ct)
		{
			try
			{
				var validation = await _categoryValidator.ValidateAsync(request);

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
					return BadRequest(errors);
				}

				var category = await _categoryService.GetAsync(id, ct);

				if (category == null)
				{
					return NotFound("Category not found");
				}

				var updatedCategory = await _categoryService.UpdateAsync(id, request, ct);

				if (updatedCategory == null)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Update failed");
				}

				return Ok(updatedCategory);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously deletes an category by its ID.
		/// </summary>
		/// <param name="id">Category ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="204">Returns confirmation of deletion</response>
		/// <response code="404">Returns if the category was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Deletion confirmation</returns>
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteCategoryAsync(int id, CancellationToken ct)
		{
			try
			{
				var category = await _categoryService.GetAsync(id, ct);

				if (category == null)
				{
					return NotFound("Category not found");
				}

				var isDeleted = await _categoryService.DeleteAsync(id, ct);

				if (!isDeleted)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Delete failed");
				}

				return NoContent();
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}
	}
}
