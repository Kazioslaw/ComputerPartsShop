using ComputerPartsShop.Domain;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Infrastructure;
using ComputerPartsShop.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Authorize]
	[Route("[controller]")]
	public class ProductController : ControllerBase
	{
		private readonly IProductService _productService;
		private readonly IValidator<ProductRequest> _productValidator;

		public ProductController(IProductService productService, ICategoryRepository categoryRepository, IValidator<ProductRequest> productValidator)
		{
			_productService = productService;
			_productValidator = productValidator;
		}

		/// <summary>
		/// Asynchronously retrieves all products.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the list of products</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>List of products</returns>
		[HttpGet]
		public async Task<IActionResult> GetProductListAsync(CancellationToken ct)
		{
			try
			{
				var productList = await _productService.GetListAsync(ct);

				return Ok(productList);
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
		/// Asynchronously retrieves an product by its ID.
		/// </summary>
		/// <param name="id">Product ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the product</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the product was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Product</returns>
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetProductAsync(int id, CancellationToken ct)
		{
			try
			{
				var product = await _productService.GetAsync(id, ct);

				return Ok(product);
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
		/// Asynchronously creates a new product.
		/// </summary>
		/// <param name="request">Product model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the created product</response>
		/// <response code="400">Returns if the category name was empty or invalid</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Created product</returns>

		[HttpPost]
		[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> CreateProductAsync(ProductRequest request, CancellationToken ct)
		{
			try
			{
				var validation = _productValidator.Validate(request);

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
					return BadRequest(errors);
				}

				var product = await _productService.CreateAsync(request, ct);

				return Ok(product);
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
		/// Asynchronously updates an product by its ID.
		/// </summary>
		/// <param name="id">Product ID</param>
		/// <param name="request">Updated product model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the updated product</response>
		/// <response code="400">Returns if the category name was empty or invalid</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the product was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Updated product</returns>

		[HttpPut("{id:int}")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> UpdateProductAsync(int id, ProductRequest request, CancellationToken ct)
		{
			try
			{
				var validation = _productValidator.Validate(request);

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
					return BadRequest(errors);
				}

				var updatedProduct = await _productService.UpdateAsync(id, request, ct);

				return Ok(updatedProduct);
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
		/// Asynchronously deletes an product by its ID.
		/// </summary>
		/// <param name="id">Product ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="204">Returns confirmation of deletion</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the product was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Deletion confirmation</returns>
		[HttpDelete("{id:int}")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> DeleteProductAsync(int id, CancellationToken ct)
		{
			try
			{
				await _productService.DeleteAsync(id, ct);

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
