using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class PaymentProviderController : ControllerBase
	{

		private readonly IPaymentProviderService _ppService;

		public PaymentProviderController(IPaymentProviderService ppService)
		{
			_ppService = ppService;
		}

		[HttpGet]
		public async Task<ActionResult<List<PaymentProviderResponse>>> GetPaymentProviderListAsync()
		{
			var paymentProviderList = await _ppService.GetListAsync();

			return Ok(paymentProviderList);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<DetailedPaymentProviderResponse>> GetPaymentProviderAsync(int id)
		{
			var paymentProvider = await _ppService.GetAsync(id);

			if (paymentProvider == null)
			{
				return NotFound();
			}

			return Ok(paymentProvider);
		}

		[HttpPost]
		public async Task<ActionResult<PaymentProviderResponse>> CreatePaymentProviderAsync(PaymentProviderRequest request)
		{
			var paymentProvider = await _ppService.CreateAsync(request);

			return CreatedAtAction(nameof(CreatePaymentProviderAsync), paymentProvider);
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<PaymentProviderResponse>> UpdatePaymentProviderAsync(int id, PaymentProviderRequest request)
		{
			var paymentProvider = await _ppService.GetAsync(id);

			if (paymentProvider == null)
			{
				return NotFound();
			}

			var updatedPaymentProvider = await _ppService.UpdateAsync(id, request);

			return Ok(updatedPaymentProvider);
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult> DeletePaymentProviderAsync(int id)
		{
			var paymentProvider = await _ppService.GetAsync(id);

			if (paymentProvider == null)
			{
				return NotFound();
			}

			await _ppService.DeleteAsync(id);

			return Ok();
		}
	}
}
