using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Infrastructure;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AddressController : ControllerBase
	{
		private readonly IAddressService _addressService;
		private readonly ICustomerRepository _customerRepository;
		private readonly ICountryService _countryService;

		public AddressController(IAddressService addressService, ICustomerRepository customerRepository, ICountryService countryService)
		{
			_addressService = addressService;
			_customerRepository = customerRepository;
			_countryService = countryService;
		}

		/// <summary>
		/// Asynchronously retrieves all addresses.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the list of addresses</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>List of addresses</returns>
		[HttpGet]
		public async Task<IActionResult> GetAddressListAsync(CancellationToken ct)
		{
			try
			{
				var addressList = await _addressService.GetListAsync(ct);

				return Ok(addressList);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously retrieves an address by its ID.
		/// </summary>
		/// <param name="id">Address ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the address</response>
		/// <response code="404">Returns if the address was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Address</returns>
		[HttpGet("{id:Guid}")]
		public async Task<IActionResult> GetAddressAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var address = await _addressService.GetAsync(id, ct);

				if (address == null)
				{
					return NotFound("Address not found");
				}

				return Ok(address);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously creates a new address.
		/// </summary>
		/// <param name="request">Address model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the created address</response>
		/// <response code="400">Returns if username, email or country3code was empty or invalid</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the address could not be created</response>
		/// <returns>Created address</returns>
		[HttpPost]
		public async Task<IActionResult> CreateAddressAsync([FromBody] AddressRequest request, CancellationToken ct)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(request.Username) && string.IsNullOrWhiteSpace(request.Email))
				{
					return BadRequest("Invalid or missing username or email");
				}

				var customer = await _customerRepository.GetByUsernameOrEmailAsync(request.Username ?? request.Email, ct);

				if (customer == null)
				{
					return BadRequest("Invalid or missing username or email");
				}

				var country = await _countryService.GetByAlpha3Async(request.Country3Code, ct);

				if (country == null)
				{
					return BadRequest("Invalid country code");
				}

				var address = await _addressService.CreateAsync(request, ct);

				if (address == null)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create address");
				}

				return Ok(address);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously updates an address by its ID.
		/// </summary>
		/// <param name="oldAddressId">Address ID</param>
		/// <param name="request">Updated address model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the updated address</response>
		/// <response code="400">Returns if username, email or country3code was empty or invalid</response>
		/// <response code="404">Returns if the address was not found</response>		
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the address could not be updated</response>
		/// <returns>Updated address</returns>
		[HttpPut("{oldAddressId:guid}")]
		public async Task<IActionResult> UpdateAddressAsync(Guid oldAddressId, [FromBody] UpdateAddressRequest request, CancellationToken ct)
		{
			try
			{
				var address = await _addressService.GetAsync(oldAddressId, ct);

				if (address == null)
				{
					return NotFound("Address not found");
				}

				if (string.IsNullOrWhiteSpace(request.oldUsername) && string.IsNullOrWhiteSpace(request.oldEmail))
				{
					return BadRequest("Invalid or missing oldUsername or oldEmail");
				}

				if (string.IsNullOrWhiteSpace(request.newUsername) && string.IsNullOrWhiteSpace(request.newEmail))
				{
					return BadRequest("Invalid or missing newUsername or newEmail");
				}

				var oldCustomer = await _customerRepository.GetByUsernameOrEmailAsync(request.oldUsername ?? request.oldEmail, ct);

				if (oldCustomer == null)
				{
					return BadRequest("Invalid or missing username or email");
				}

				var newCustomer = await _customerRepository.GetByUsernameOrEmailAsync(request.newUsername ?? request.newEmail, ct);

				if (newCustomer == null)
				{
					return BadRequest("Invalid or missing username or email");
				}

				var country = await _countryService.GetByAlpha3Async(request.newCountry3Code, ct);

				if (country == null)
				{
					return BadRequest("Country with that alpha3 code was not found");
				}

				var updatedAddress = await _addressService.UpdateAsync(oldAddressId, request, ct);

				if (updatedAddress == null)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Update failed");
				}

				return Ok(updatedAddress);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}

		}

		/// <summary>
		/// Asynchronously deletes an address by its ID.
		/// </summary>
		/// <param name="id">Address ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="204">Returns confirmation of deletion</response>
		/// <response code="404">Returns if the address was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Deletion confirmation</returns>
		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> DeleteAddressAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var address = await _addressService.GetAsync(id, ct);

				if (address == null)
				{
					return NotFound("Address not found");
				}

				var isDeleted = await _addressService.DeleteAsync(id, ct);

				if (!isDeleted)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Delete failed");
				}


				return NoContent();
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}
	}
}
