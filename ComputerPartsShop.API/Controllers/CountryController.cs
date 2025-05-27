using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CountryController : ControllerBase
	{
		private readonly ICountryService _countryService;

		public CountryController(ICountryService countryService)
		{
			_countryService = countryService;
		}

		/// <summary>
		/// Asynchronously retrieves all countries.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <response code = "200">Returns the list of countries</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>List of countries</returns>
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
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously retrieves an country by its ID.
		/// </summary>
		/// <param name="id">Country ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the country</response>
		/// <response code="404">Returns if the country was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Country</returns>
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
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously creates a new country.
		/// </summary>
		/// <param name="request">Country model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the created country</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Created country</returns>
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
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously updates an country by its ID.
		/// </summary>
		/// <param name="id">Country ID</param>
		/// <param name="request">Updated country model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the updated country</response>
		/// <response code="404">Returns if the country was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Updated country</returns>
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
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}

		}

		/// <summary>
		/// Asynchronously deletes an country by its ID.
		/// </summary>
		/// <param name="id">Country ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns confirmation of deletion</response>
		/// <response code="404">Returns if the country was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Deletion confirmation</returns>
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
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

	}
}
