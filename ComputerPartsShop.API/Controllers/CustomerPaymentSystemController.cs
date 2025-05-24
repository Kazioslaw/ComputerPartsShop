using ComputerPartsShop.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class CustomerPaymentSystemController : ControllerBase
	{
		[HttpGet]
		public ActionResult<List<CustomerPaymentSystemResponse>> GetCustomerPaymentServiceList()
		{
			return Ok();
		}

		[HttpGet("{id:guid}")]
		public ActionResult<DetailedCustomerPaymentSystemResponse> GetCustomerPaymentService(Guid id)
		{
			return Ok();
		}

		[HttpPost]
		public ActionResult<CustomerPaymentSystemResponse> CreateCustomerPaymentService(CustomerPaymentSystemRequest request)
		{
			return Ok();
		}

		[HttpPut("{id:guid}")]
		public ActionResult<CustomerPaymentSystemResponse> UpdateCustomerPaymentService(Guid id, CustomerPaymentSystemRequest request)
		{
			return Ok();
		}
	}
}
