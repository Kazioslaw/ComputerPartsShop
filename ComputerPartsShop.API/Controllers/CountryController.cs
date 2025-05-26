using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class CountryController : ControllerBase
	{
		private readonly ICountryService _countryService;

		public CountryController(ICountryService countryService)
		{
			_countryService = countryService;
		}

		[HttpGet]
		public async Task<ActionResult<List<CountryResponse>>> GetCountryListAsync()
		{
			var countryList = await _countryService.GetListAsync();

			return Ok(countryList);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<DetailedCountryResponse>> GetCountry(int id)
		{
			var country = await _countryService.GetAsync(id);

			if (country == null)
			{
				return NotFound();
			}

			return Ok(country);
		}

		[HttpPost]
		public async Task<ActionResult<CountryResponse>> CreateCountryAsync(CountryRequest request)
		{
			var country = await _countryService.CreateAsync(request);

			return CreatedAtAction(nameof(CreateCountryAsync), country);
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<CountryResponse>> UpdateCountryAsync(int id, CountryRequest request)
		{
			var country = await _countryService.GetAsync(id);

			if (country == null)
			{
				return NotFound();
			}

			var updatedCountry = await _countryService.UpdateAsync(id, request);

			return Ok(updatedCountry);

		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult> DeleteCountryAsync(int id)
		{
			var country = await _countryService.GetAsync(id);

			if (country == null)
			{
				return NotFound();
			}

			await _countryService.DeleteAsync(id);

			return Ok(country);
		}

	}
}
