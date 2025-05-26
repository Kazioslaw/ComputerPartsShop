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
		public async Task<ActionResult<List<AddressResponse>>> GetAddressListAsync()
		{
			var addressList = await _addressService.GetListAsync();

			return Ok(addressList);

		}

		[HttpGet("{id:Guid}")]
		public async Task<ActionResult<AddressResponse>> GetAddressAsync(Guid id)
		{
			var address = await _addressService.GetAsync(id);

			if (address == null)
			{
				return NotFound();
			}

			return Ok(address);
		}

		[HttpPost]
		public async Task<ActionResult<AddressResponse>> CreateAddressAsync(AddressRequest request)
		{
			var address = await _addressService.CreateAsync(request);

			return CreatedAtAction(nameof(CreateAddressAsync), address);
		}

		[HttpPut("{id:guid}")]
		public async Task<ActionResult<AddressResponse>> UpdateAddressAsync(Guid id, AddressRequest request)
		{
			var address = await _addressService.GetAsync(id);

			if (address == null)
			{
				return NotFound();
			}

			var updatedAddress = await _addressService.UpdateAsync(id, request);

			return Ok(updatedAddress);

		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> DeleteAddressAsync(Guid id)
		{
			var address = await _addressService.GetAsync(id);

			if (address == null)
			{
				return NotFound();
			}

			await _addressService.DeleteAsync(id);

			return Ok();
		}
	}
}
