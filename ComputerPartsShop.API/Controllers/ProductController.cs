using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Infrastructure;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
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
		public async Task<ActionResult<List<ProductResponse>>> GetProductListAsync()
		{
			var productList = await _productService.GetListAsync();

			return Ok(productList);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<ProductResponse>> GetProductAsync(int id)
		{
			var product = await _productService.GetAsync(id);

			if (product == null)
			{
				return NotFound();
			}

			return Ok(product);
		}

		[HttpPost]
		public async Task<ActionResult<ProductResponse>> CreateProductAsync(ProductRequest request)
		{
			var category = await _categoryRepository.GetByNameAsync(request.CategoryName);

			if (category == null)
			{
				return BadRequest();
			}

			var product = await _productService.CreateAsync(request);

			return CreatedAtAction(nameof(CreateProductAsync), product);
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<ProductResponse>> UpdateProductAsync(int id, ProductRequest request)
		{
			var product = await _productService.GetAsync(id);

			if (product == null)
			{
				return NotFound();
			}

			var category = await _categoryRepository.GetByNameAsync(request.CategoryName);

			if (category == null)
			{
				return BadRequest();
			}

			var updatedProduct = await _productService.UpdateAsync(id, request);

			return Ok(updatedProduct);
		}

		[HttpDelete]
		public async Task<ActionResult> DeleteProductAsync(int id)
		{
			var product = await _productService.GetAsync(id);

			if (product == null)
			{
				return NotFound();
			}

			await _productService.DeleteAsync(id);

			return Ok();
		}
	}
}
