using ComputerPartsShop.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class CategoryController : ControllerBase
	{
		[HttpGet]
		public ActionResult<List<CategoryResponse>> GetCategoryList()
		{
			return Ok(new List<CategoryResponse>());
		}

		[HttpGet("{id:int}")]
		public ActionResult<CategoryResponse> GetCategory(Guid id)
		{
			return Ok();
		}

		[HttpPost]
		public ActionResult<CategoryResponse> CreateCategory(CategoryRequest request)
		{
			return Ok();
		}

		[HttpPut("{id:int}")]
		public ActionResult<CategoryResponse> UpdateCategory(int id, CategoryRequest request)
		{
			return Ok();
		}

		[HttpDelete("{id:int}")]
		public ActionResult DeleteCategory(int id)
		{
			return Ok();
		}
	}
}
