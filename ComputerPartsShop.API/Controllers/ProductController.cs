using ComputerPartsShop.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class ProductController : ControllerBase
	{
		[HttpGet]
		public ActionResult<List<ProductResponse>> GetProductList()
		{
			return Ok();
		}

		[HttpGet("{id:int}")]
		public ActionResult<ProductResponse> GetProduct(int id)
		{
			return Ok();
		}

		[HttpPost]
		public ActionResult<ProductResponse> CreateProduct(ProductRequest request)
		{
			return Ok();
		}

		[HttpPut("{id:int}")]
		public ActionResult<ProductResponse> UpdateProduct(int id, ProductResponse product)
		{
			return Ok();
		}

		[HttpDelete]
		public ActionResult DeleteProduct(int id)
		{
			return Ok();
		}
	}
}
