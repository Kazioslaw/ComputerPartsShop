using ComputerPartsShop.Domain.DTOs;
using ComputerPartsShop.Infrastructure;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class ProductController : ControllerBase
	{
		private readonly CategoryRepository _categoryRepository;
		private readonly ProductService _productService;

		public ProductController(ProductService productService, CategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
			_productService = productService;
		}

		[HttpGet]
		public async Task<ActionResult<List<ProductResponse>>> GetProductList()
		{
			var productList = await _productService.GetList();

			return Ok(productList);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<ProductResponse>> GetProduct(int id)
		{
			var product = await _productService.Get(id);

			if (product == null)
			{
				return NotFound();
			}

			return Ok(product);
		}

		[HttpPost]
		public async Task<ActionResult<ProductResponse>> CreateProduct(ProductRequest request)
		{
			var category = await _categoryRepository.GetByName(request.CategoryName);

			if (category == null)
			{
				return BadRequest();
			}

			var product = await _productService.Create(request);

			return CreatedAtAction(nameof(CreateProduct), product);
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<ProductResponse>> UpdateProduct(int id, ProductRequest request)
		{
			var product = await _productService.Get(id);

			if (product == null)
			{
				return NotFound();
			}

			var category = await _categoryRepository.GetByName(request.CategoryName);

			if (category == null)
			{
				return BadRequest();
			}

			var updatedProduct = await _productService.Update(id, request);

			return Ok(updatedProduct);
		}

		[HttpDelete]
		public async Task<ActionResult> DeleteProduct(int id)
		{
			var product = await _productService.Get(id);

			if (product == null)
			{
				return NotFound();
			}

			await _productService.Delete(id);

			return Ok();
		}
	}
}
