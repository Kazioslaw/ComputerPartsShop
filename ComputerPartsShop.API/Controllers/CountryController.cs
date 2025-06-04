using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CountryController : ControllerBase
	{
		private readonly ICountryService _countryService;
		private readonly IValidator<CountryRequest> _countryValidator;

		public CountryController(ICountryService countryService, IValidator<CountryRequest> countryValidator)
		{
			_countryService = countryService;
			_countryValidator = countryValidator;
		}

		/// <summary>
		/// Asynchronously retrieves all countries.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the list of countries</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>List of countries</returns>
		[HttpGet]
		public async Task<IActionResult> GetCountryListAsync(CancellationToken ct)
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
			catch (DataErrorException ex)
			{
				return StatusCode(ex.StatusCode, ex.Message);
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
		public async Task<IActionResult> GetCountryAsync(int id, CancellationToken ct)
		{
			try
			{
				var country = await _countryService.GetAsync(id, ct);

				return Ok(country);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode(ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously retrieves an country by its Alpha3 code.
		/// </summary>
		/// <param name="alpha3">Country alpha3 code</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the country</response>
		/// <response code="404">Returns if the country was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Country</returns>
		[HttpGet("{alpha3}")]
		public async Task<IActionResult> GetCountryByAlpha3Code(string alpha3, CancellationToken ct)
		{
			try
			{
				var country = await _countryService.GetByAlpha3Async(alpha3, ct);

				return Ok(country);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode(ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously creates a new country.
		/// </summary>
		/// <param name="request">Country model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the created country</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Created country</returns>
		[HttpPost]
		public async Task<IActionResult> CreateCountryAsync(CountryRequest request, CancellationToken ct)
		{
			try
			{
				var validation = await _countryValidator.ValidateAsync(request);

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
					return BadRequest(errors);
				}

				var country = await _countryService.CreateAsync(request, ct);

				return Created(nameof(GetCountryAsync), country);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode(ex.StatusCode, ex.Message);
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
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Updated country</returns>
		[HttpPut("{id:int}")]
		public async Task<IActionResult> UpdateCountryAsync(int id, CountryRequest request, CancellationToken ct)
		{
			try
			{
				var validation = await _countryValidator.ValidateAsync(request);

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
					return BadRequest(errors);
				}

				var updatedCountry = await _countryService.UpdateAsync(id, request, ct);

				return Ok(updatedCountry);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode(ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously deletes an country by its ID.
		/// </summary>
		/// <param name="id">Country ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="204">Returns confirmation of deletion</response>
		/// <response code="404">Returns if the country was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Deletion confirmation</returns>
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteCountryAsync(int id, CancellationToken ct)
		{
			try
			{
				var country = await _countryService.GetAsync(id, ct);

				if (country == null)
				{
					return NotFound("Country not found");
				}

				await _countryService.DeleteAsync(id, ct);

				return NoContent();
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode(ex.StatusCode, ex.Message);
			}
		}
	}
}
