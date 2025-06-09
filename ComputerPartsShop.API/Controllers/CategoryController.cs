using ComputerPartsShop.Domain;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
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
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
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
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously retrieves an category by its ID.
		/// </summary>
		/// <param name="id">Category ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the category</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the category was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Category</returns>
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetCategoryAsync(int id, CancellationToken ct)
		{
			try
			{
				var category = await _categoryService.GetAsync(id, ct);

				return Ok(category);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously retrieves an category by its name.
		/// </summary>
		/// <param name="name">Category name</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the category</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the category was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Category</returns>
		[HttpGet("{name}")]
		public async Task<IActionResult> GetCategoryByNameAsync(string name, CancellationToken ct)
		{
			try
			{
				var category = await _categoryService.GetByNameAsync(name, ct);

				return Ok(category);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously creates a new category.
		/// </summary>
		/// <param name="request">Category model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the created category</response>		
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Created category</returns>
		[HttpPost]
		[Authorize(Roles = nameof(UserRole.Admin))]
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

				return Created(nameof(GetCategoryAsync), category);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously updates an category by its ID.
		/// </summary>
		/// <param name="id">Category ID</param>
		/// <param name="request">Updated category model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the updated category</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the category was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Updated category</returns>
		[HttpPut("{id:int}")]
		[Authorize(Roles = nameof(UserRole.Admin))]
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

				var updatedCategory = await _categoryService.UpdateAsync(id, request, ct);

				return Ok(updatedCategory);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously deletes an category by its ID.
		/// </summary>
		/// <param name="id">Category ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="204">Returns confirmation of deletion</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the category was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Deletion confirmation</returns>
		[HttpDelete("{id:int}")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> DeleteCategoryAsync(int id, CancellationToken ct)
		{
			try
			{
				await _categoryService.DeleteAsync(id, ct);

				return NoContent();
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}
	}
}
