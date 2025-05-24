using ComputerPartsShop.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class PaymentProviderController : ControllerBase
	{
		[HttpGet]
		public ActionResult<List<PaymentProviderResponse>> GetPaymentProviderList()
		{
			return Ok();
		}

		[HttpGet("{id:int}")]
		public ActionResult<DetailedPaymentProviderResponse> GetPaymentProvider(int id)
		{
			return Ok();
		}

		[HttpPost]
		public ActionResult<PaymentProviderResponse> CreatePaymentProvider(PaymentProviderRequest request)
		{
			return Ok();
		}

		[HttpPut("{id:int}")]
		public ActionResult<PaymentProviderResponse> UpdatePaymentProvider(int id, PaymentProviderRequest request)
		{
			return Ok();
		}

		[HttpDelete("{id:int}")]
		public ActionResult DeletePaymentProvider(int id)
		{
			return Ok();
		}
	}
}
