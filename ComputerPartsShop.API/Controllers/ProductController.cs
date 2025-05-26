using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Infrastructure;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProductController : ControllerBase
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IProductService _productService;

		public ProductController(IProductService productService, ICategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
			_productService = productService;
		}

		[HttpGet]
		public async Task<ActionResult<List<ProductResponse>>> GetProductListAsync(CancellationToken ct)
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

		[HttpGet("{id:int}")]
		public async Task<ActionResult<ProductResponse>> GetProductAsync(int id, CancellationToken ct)
		{
			try
			{
				var product = await _productService.GetAsync(id, ct);

				if (product == null)
				{
					return NotFound();
				}

				return Ok(product);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		[HttpPost]
		public async Task<ActionResult<ProductResponse>> CreateProductAsync(ProductRequest request, CancellationToken ct)
		{
			try
			{
				var category = await _categoryRepository.GetByNameAsync(request.CategoryName, ct);

				if (category == null)
				{
					return BadRequest();
				}

				var product = await _productService.CreateAsync(request, ct);

				return CreatedAtAction(nameof(CreateProductAsync), product);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<ProductResponse>> UpdateProductAsync(int id, ProductRequest request, CancellationToken ct)
		{
			try
			{
				var product = await _productService.GetAsync(id, ct);

				if (product == null)
				{
					return NotFound();
				}

				var category = await _categoryRepository.GetByNameAsync(request.CategoryName, ct);

				if (category == null)
				{
					return BadRequest();
				}

				var updatedProduct = await _productService.UpdateAsync(id, request, ct);

				return Ok(updatedProduct);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		[HttpDelete]
		public async Task<ActionResult> DeleteProductAsync(int id, CancellationToken ct)
		{
			try
			{
				var product = await _productService.GetAsync(id, ct);

				if (product == null)
				{
					return NotFound();
				}

				await _productService.DeleteAsync(id, ct);

				return Ok();
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}
	}
}
