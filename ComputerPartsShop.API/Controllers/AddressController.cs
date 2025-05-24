using ComputerPartsShop.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class AddressController : ControllerBase
	{
		[HttpGet]
		public ActionResult<List<AddressResponse>> GetAddressList()
		{
			return Ok(new List<AddressResponse>());
		}

		[HttpGet("{id:Guid}")]
		public ActionResult<AddressResponse> GetAddress(Guid id)
		{
			return Ok();
		}

		[HttpPost]
		public ActionResult<AddressResponse> CreateAddress(AddressRequest request)
		{
			return Ok();
		}

		[HttpPut("{id:guid}")]
		public ActionResult<AddressResponse> UpdateAddress(Guid id, AddressRequest request)
		{
			return Ok();
		}

		[HttpDelete("{id:guid}")]
		public ActionResult DeleteAddress(Guid id)
		{
			return Ok();
		}
	}
}
