using ComputerPartsShop.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class CountryController : ControllerBase
	{
		[HttpGet]
		public ActionResult<List<CountryResponse>> GetCountryList()
		{
			return Ok();
		}

		[HttpGet("{id:int}")]
		public ActionResult<DetailedCountryResponse> GetCountry()
		{
			return Ok();
		}

		[HttpPost]
		public ActionResult<CountryResponse> CreateCountry(CountryRequest request)
		{
			return Ok();
		}

		[HttpPut("{id:int}")]
		public ActionResult<CountryResponse> UpdateCountry(int id, CountryRequest request)
		{
			return Ok();
		}

		[HttpDelete("{id:int}")]
		public ActionResult DeleteCountry(int id)
		{
			return Ok();
		}

	}
}
