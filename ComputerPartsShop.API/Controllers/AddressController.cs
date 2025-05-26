using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class AddressController : ControllerBase
	{
		private readonly IAddressService _addressService;

		public AddressController(IAddressService addressService)
		{
			_addressService = addressService;
		}

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
				return BadRequest();
			}
		}

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
				return BadRequest();
			}
		}

		[HttpPost]
		public async Task<ActionResult<AddressResponse>> CreateAddressAsync(AddressRequest request, CancellationToken ct)
		{
			try
			{
				var address = await _addressService.CreateAsync(request, ct);

				return CreatedAtAction(nameof(CreateAddressAsync), address);
			}
			catch (OperationCanceledException)
			{
				return BadRequest();
			}
		}

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

				var updatedAddress = await _addressService.UpdateAsync(id, request, ct);

				return Ok(updatedAddress);
			}
			catch (OperationCanceledException)
			{
				return BadRequest();
			}

		}

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
				return BadRequest();
			}
		}
	}
}
