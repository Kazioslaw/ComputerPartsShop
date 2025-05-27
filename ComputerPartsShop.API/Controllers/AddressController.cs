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
		private readonly ICountryRepository _countryRepository;

		public AddressController(IAddressService addressService, ICustomerRepository customerRepository, ICountryRepository countryRepository)
		{
			_addressService = addressService;
			_customerRepository = customerRepository;
			_countryRepository = countryRepository;
		}

		/// <summary>
		/// Asynchronously retrieves all addresses.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the list of addresses</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>List of addresses</returns>
		[HttpGet]
		public async Task<ActionResult<List<AddressResponse>>> GetAddressListAsync(CancellationToken ct)
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
		public async Task<ActionResult<AddressResponse>> GetAddressAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var address = await _addressService.GetAsync(id, ct);

				if (address == null)
				{
					return NotFound();
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
		/// <returns>Created address</returns>
		[HttpPost]
		public async Task<ActionResult<AddressResponse>> CreateAddressAsync(AddressRequest request, CancellationToken ct)
		{
			try
			{

				if (string.IsNullOrWhiteSpace(request.Username) && string.IsNullOrWhiteSpace(request.Email))
				{
					return BadRequest();
				}

				var customer = await _customerRepository.GetByUsernameOrEmailAsync(request.Username ?? request.Email, ct);

				if (customer == null)
				{
					return BadRequest();
				}

				var country = await _countryRepository.GetByCountry3CodeAsync(request.Country3Code, ct);

				if (country == null)
				{
					return BadRequest();
				}

				var address = await _addressService.CreateAsync(request, ct);

				return CreatedAtAction(nameof(CreateAddressAsync), address);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		/// <summary>
		/// Asynchronously updates an address by its ID.
		/// </summary>
		/// <param name="id">Address ID</param>
		/// <param name="request">Updated address model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the updated address</response>
		/// <response code="400">Returns if username, email or country3code was empty or invalid</response>
		/// <response code="404">Returns if the address was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Updated address</returns>
		[HttpPut("{id:guid}")]
		public async Task<ActionResult<AddressResponse>> UpdateAddressAsync(Guid id, AddressRequest request, CancellationToken ct)
		{
			try
			{
				var address = await _addressService.GetAsync(id, ct);

				if (address == null)
				{
					return NotFound();
				}

				if (string.IsNullOrWhiteSpace(request.Username) && string.IsNullOrWhiteSpace(request.Email))
				{
					return BadRequest();
				}

				var customer = await _customerRepository.GetByUsernameOrEmailAsync(request.Username ?? request.Email, ct);

				if (customer == null)
				{
					return BadRequest();
				}

				var country = await _countryRepository.GetByCountry3CodeAsync(request.Country3Code, ct);

				if (country == null)
				{
					return BadRequest();
				}

				var updatedAddress = await _addressService.UpdateAsync(id, request, ct);

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
		/// <response code="200">Returns confirmation of deletion</response>
		/// <response code="404">Returns if the address was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <returns>Deletion confirmation</returns>
		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> DeleteAddressAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var address = await _addressService.GetAsync(id, ct);

				if (address == null)
				{
					return NotFound();
				}

				await _addressService.DeleteAsync(id, ct);

				return Ok();
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}
	}
}
