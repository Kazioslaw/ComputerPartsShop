using ComputerPartsShop.Domain.DTOs;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class PaymentProviderController : ControllerBase
	{

		private readonly PaymentProviderService _ppService;

		public PaymentProviderController(PaymentProviderService ppService)
		{
			_ppService = ppService;
		}

		[HttpGet]
		public async Task<ActionResult<List<PaymentProviderResponse>>> GetPaymentProviderList()
		{
			var paymentProviderList = await _ppService.GetList();

			return Ok(paymentProviderList);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<DetailedPaymentProviderResponse>> GetPaymentProvider(int id)
		{
			var paymentProvider = await _ppService.Get(id);

			if (paymentProvider == null)
			{
				return NotFound();
			}

			return Ok(paymentProvider);
		}

		[HttpPost]
		public async Task<ActionResult<PaymentProviderResponse>> CreatePaymentProvider(PaymentProviderRequest request)
		{
			var paymentProvider = await _ppService.Create(request);

			return CreatedAtAction(nameof(CreatePaymentProvider), paymentProvider);
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<PaymentProviderResponse>> UpdatePaymentProvider(int id, PaymentProviderRequest request)
		{
			var paymentProvider = await _ppService.Get(id);

			if (paymentProvider == null)
			{
				return NotFound();
			}

			var updatedPaymentProvider = await _ppService.Update(id, request);

			return Ok(updatedPaymentProvider);
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult> DeletePaymentProvider(int id)
		{
			var paymentProvider = await _ppService.Get(id);

			if (paymentProvider == null)
			{
				return NotFound();
			}

			await _ppService.Delete(id);

			return Ok();
		}
	}
}
