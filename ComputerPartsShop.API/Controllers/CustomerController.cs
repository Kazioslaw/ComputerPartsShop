using ComputerPartsShop.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class CustomerController : ControllerBase
	{
		[HttpGet]
		public ActionResult<List<CustomerResponse>> GetCustomerList()
		{
			return Ok();
		}

		[HttpGet("{id:guid}")]
		public ActionResult<DetailedCustomerResponse> GetCustomer(Guid id)
		{
			return Ok();
		}

		[HttpPost]
		public ActionResult<CustomerResponse> CreateCustomer(CustomerRequest request)
		{
			return Ok();
		}

		[HttpPut("{id:guid}")]
		public ActionResult<CustomerResponse> UpdateCustomer(Guid id, CustomerResponse request)
		{
			return Ok();
		}

		[HttpDelete("{id:guid}")]
		public ActionResult DeleteCustomer(Guid id)
		{
			return Ok();
		}
	}
}
