using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Infrastructure;
using ComputerPartsShop.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProductController : ControllerBase
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IProductService _productService;
		private readonly IValidator<ProductRequest> _productValidator;

		public ProductController(IProductService productService, ICategoryRepository categoryRepository, IValidator<ProductRequest> productValidator)
		{
			_categoryRepository = categoryRepository;
			_productService = productService;
			_productValidator = productValidator;
		}

		/// <summary>
		/// Asynchronously retrieves all products.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the list of products</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
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
		}

		/// <summary>
		/// Asynchronously retrieves an product by its ID.
		/// </summary>
		/// <param name="id">Product ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the product</response>
		/// <response code="404">Returns if the product was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Product</returns>
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetProductAsync(int id, CancellationToken ct)
		{
			try
			{
				var product = await _productService.GetAsync(id, ct);

				if (product == null)
				{
					return NotFound("Product not found");
				}

				return Ok(product);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously creates a new product.
		/// </summary>
		/// <param name="request">Product model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the created product</response>
		/// <response code="400">Returns if the category name was empty or invalid</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Created product</returns>

		[HttpPost]
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

				var category = await _categoryRepository.GetByNameAsync(request.CategoryName, ct);

				if (category == null)
				{
					return BadRequest("Invalid category name");
				}

				var product = await _productService.CreateAsync(request, ct);

				if (product == null)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Create failed");
				}

				return Ok(product);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
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
		/// <response code="404">Returns if the product was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Updated product</returns>

		[HttpPut("{id:int}")]
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

				var product = await _productService.GetAsync(id, ct);

				if (product == null)
				{
					return NotFound("Product not found");
				}

				var category = await _categoryRepository.GetByNameAsync(request.CategoryName, ct);

				if (category == null)
				{
					return BadRequest("Invalid category name");
				}

				var updatedProduct = await _productService.UpdateAsync(id, request, ct);

				if (updatedProduct == null)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Update failed");
				}

				return Ok(updatedProduct);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously deletes an product by its ID.
		/// </summary>
		/// <param name="id">Product ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="204">Returns confirmation of deletion</response>
		/// <response code="404">Returns if the product was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Deletion confirmation</returns>
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteProductAsync(int id, CancellationToken ct)
		{
			try
			{
				var product = await _productService.GetAsync(id, ct);

				if (product == null)
				{
					return NotFound("Product not found");
				}

				var isDeleted = await _productService.DeleteAsync(id, ct);

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
