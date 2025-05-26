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
		public async Task<ActionResult<List<CountryResponse>>> GetCountryListAsync(CancellationToken ct)
		{
			try
			{
				var countryList = await _countryService.GetListAsync(ct);

				return Ok(countryList);
			}
			catch (OperationCanceledException)
			{
				return BadRequest();
			}
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<DetailedCountryResponse>> GetCountry(int id, CancellationToken ct)
		{
			try
			{
				var country = await _countryService.GetAsync(id, ct);

				if (country == null)
				{
					return NotFound();
				}

				return Ok(country);
			}
			catch (OperationCanceledException)
			{
				return BadRequest();
			}
		}

		[HttpPost]
		public async Task<ActionResult<CountryResponse>> CreateCountryAsync(CountryRequest request, CancellationToken ct)
		{
			try
			{
				var country = await _countryService.CreateAsync(request, ct);

				return CreatedAtAction(nameof(CreateCountryAsync), country);
			}
			catch (OperationCanceledException)
			{
				return BadRequest();
			}
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<CountryResponse>> UpdateCountryAsync(int id, CountryRequest request, CancellationToken ct)
		{
			try
			{
				var country = await _countryService.GetAsync(id, ct);

				if (country == null)
				{
					return NotFound();
				}

				var updatedCountry = await _countryService.UpdateAsync(id, request, ct);

				return Ok(updatedCountry);
			}
			catch (OperationCanceledException)
			{
				return BadRequest();
			}

		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult> DeleteCountryAsync(int id, CancellationToken ct)
		{
			try
			{
				var country = await _countryService.GetAsync(id, ct);

				if (country == null)
				{
					return NotFound();
				}

				await _countryService.DeleteAsync(id, ct);

				return Ok(country);
			}
			catch (OperationCanceledException)
			{
				return BadRequest();
			}
		}

	}
}
