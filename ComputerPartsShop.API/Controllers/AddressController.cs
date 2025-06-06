using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Authorize]
	[Route("[controller]")]
	public class AddressController : ControllerBase
	{
		private readonly IAddressService _addressService;
		private readonly IValidator<AddressRequest> _addressValidator;
		private readonly IValidator<UpdateAddressRequest> _updateAddressValidator;


		public AddressController(IAddressService addressService,
			IValidator<AddressRequest> addressValidator,
			IValidator<UpdateAddressRequest> updateAddressValidator)
		{
			_addressValidator = addressValidator;
			_addressService = addressService;
			_updateAddressValidator = updateAddressValidator;
		}

		/// <summary>
		/// Asynchronously retrieves all addresses.
		/// </summary>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the list of addresses</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>List of addresses</returns>
		[HttpGet]
		[Authorize(Roles = "Admin")]
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
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously retrieves an address by its ID.
		/// </summary>
		/// <param name="id">Address ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the address</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the address was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Address</returns>
		[HttpGet("{id:Guid}")]
		public async Task<IActionResult> GetAddressAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var address = await _addressService.GetAsync(id, ct);

				return Ok(address);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously creates a new address.
		/// </summary>
		/// <param name="request">Address model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the created address</response>
		/// <response code="400">Returns if username, email or country3code was empty or invalid</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Created address</returns>
		[HttpPost]
		public async Task<IActionResult> CreateAddressAsync([FromBody] AddressRequest request, CancellationToken ct)
		{
			try
			{
				var validation = await _addressValidator.ValidateAsync(request);

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
					return BadRequest(errors);
				}

				var address = await _addressService.CreateAsync(request, ct);

				return Ok(address);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously updates an address by its ID.
		/// </summary>
		/// <param name="oldAddressId">Address ID</param>
		/// <param name="request">Updated address model</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="200">Returns the updated address</response>
		/// <response code="400">Returns if usernamename, email or country3code was empty or invalid</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the address was not found</response>		
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Updated address</returns>
		[HttpPut("{oldAddressId:guid}")]
		public async Task<IActionResult> UpdateAddressAsync(Guid oldAddressId, [FromBody] UpdateAddressRequest request, CancellationToken ct)
		{
			try
			{
				var validation = await _updateAddressValidator.ValidateAsync(request, ct);

				if (!validation.IsValid)
				{
					var errors = validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
					return BadRequest(errors);
				}

				var updatedAddress = await _addressService.UpdateAsync(oldAddressId, request, ct);

				return Ok(updatedAddress);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}

		/// <summary>
		/// Asynchronously deletes an address by its ID.
		/// </summary>
		/// <param name="id">Address ID</param>
		/// <param name="ct">Cancellation token</param>
		/// <response code="204">Returns confirmation of deletion</response>
		/// <response code="401">Returns if the user is unauthorized to access the resource</response>
		/// <response code="404">Returns if the address was not found</response>
		/// <response code="499">Returns if the client cancelled the operation</response>
		/// <response code="500">Returns if the database operation failed</response>
		/// <returns>Deletion confirmation</returns>
		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> DeleteAddressAsync(Guid id, CancellationToken ct)
		{
			try
			{
				await _addressService.DeleteAsync(id, ct);

				return NoContent();
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
			catch (DataErrorException ex)
			{
				return StatusCode((int)ex.StatusCode, ex.Message);
			}
		}
	}
}
