using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class CountryController : ControllerBase
	{
		private readonly IService<CountryRequest, CountryResponse, DetailedCountryResponse, int> _countryService;

		public CountryController(IService<CountryRequest, CountryResponse, DetailedCountryResponse, int> countryService)
		{
			_countryService = countryService;
		}

		[HttpGet]
		public async Task<ActionResult<List<CountryResponse>>> GetCountryList()
		{
			var countryList = await _countryService.GetList();

			return Ok(countryList);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<DetailedCountryResponse>> GetCountry(int id)
		{
			var country = await _countryService.Get(id);

			if (country == null)
			{
				return NotFound();
			}

			return Ok(country);
		}

		[HttpPost]
		public async Task<ActionResult<CountryResponse>> CreateCountry(CountryRequest request)
		{
			var country = await _countryService.Create(request);

			return CreatedAtAction(nameof(CreateCountry), country);
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<CountryResponse>> UpdateCountry(int id, CountryRequest request)
		{
			var country = await _countryService.Get(id);

			if (country == null)
			{
				return NotFound();
			}

			var updatedCountry = await _countryService.Update(id, request);

			return Ok(updatedCountry);

		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult> DeleteCountry(int id)
		{
			var country = await _countryService.Get(id);

			if (country == null)
			{
				return NotFound();
			}

			return Ok(country);
		}

	}
}
