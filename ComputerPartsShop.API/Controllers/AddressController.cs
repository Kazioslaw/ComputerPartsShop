using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class AddressController : ControllerBase
	{
		private readonly IService<AddressRequest, AddressResponse, AddressResponse, Guid> _addressService;

		public AddressController(IService<AddressRequest, AddressResponse, AddressResponse, Guid> addressService)
		{
			_addressService = addressService;
		}

		[HttpGet]
		public async Task<ActionResult<List<AddressResponse>>> GetAddressList()
		{
			var addressList = await _addressService.GetList();

			return Ok(addressList);

		}

		[HttpGet("{id:Guid}")]
		public async Task<ActionResult<AddressResponse>> GetAddress(Guid id)
		{
			var address = await _addressService.Get(id);

			if (address == null)
			{
				return NotFound();
			}

			return Ok(address);
		}

		[HttpPost]
		public async Task<ActionResult<AddressResponse>> CreateAddress(AddressRequest request)
		{
			var address = await _addressService.Create(request);

			return CreatedAtAction(nameof(CreateAddress), address);
		}

		[HttpPut("{id:guid}")]
		public async Task<ActionResult<AddressResponse>> UpdateAddress(Guid id, AddressRequest request)
		{
			var address = await _addressService.Get(id);

			if (address == null)
			{
				return NotFound();
			}

			var updatedAddress = await _addressService.Update(id, request);

			return Ok(updatedAddress);

		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> DeleteAddress(Guid id)
		{
			var address = await _addressService.Get(id);

			if (address == null)
			{
				return NotFound();
			}

			await _addressService.Delete(id);

			return Ok();
		}
	}
}
