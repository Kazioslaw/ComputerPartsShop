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
		public async Task<ActionResult<List<PaymentProviderResponse>>> GetPaymentProviderListAsync(CancellationToken ct)
		{
			try
			{
				var paymentProviderList = await _ppService.GetListAsync(ct);

				return Ok(paymentProviderList);
			}
			catch (OperationCanceledException)
			{
				return BadRequest();
			}
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<DetailedPaymentProviderResponse>> GetPaymentProviderAsync(int id, CancellationToken ct)
		{
			try
			{
				var paymentProvider = await _ppService.GetAsync(id, ct);

				if (paymentProvider == null)
				{
					return NotFound();
				}

				return Ok(paymentProvider);
			}
			catch (OperationCanceledException)
			{
				return BadRequest();
			}
		}

		[HttpPost]
		public async Task<ActionResult<PaymentProviderResponse>> CreatePaymentProviderAsync(PaymentProviderRequest request, CancellationToken ct)
		{
			try
			{
				var paymentProvider = await _ppService.CreateAsync(request, ct);

				return CreatedAtAction(nameof(CreatePaymentProviderAsync), paymentProvider);
			}
			catch (OperationCanceledException)
			{
				return BadRequest();
			}
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<PaymentProviderResponse>> UpdatePaymentProviderAsync(int id, PaymentProviderRequest request, CancellationToken ct)
		{
			try
			{
				var paymentProvider = await _ppService.GetAsync(id, ct);

				if (paymentProvider == null)
				{
					return NotFound();
				}

				var updatedPaymentProvider = await _ppService.UpdateAsync(id, request, ct);

				return Ok(updatedPaymentProvider);
			}
			catch (OperationCanceledException)
			{
				return BadRequest();
			}
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult> DeletePaymentProviderAsync(int id, CancellationToken ct)
		{
			try
			{
				var paymentProvider = await _ppService.GetAsync(id, ct);

				if (paymentProvider == null)
				{
					return NotFound();
				}

				await _ppService.DeleteAsync(id, ct);

				return Ok();
			}
			catch (OperationCanceledException)
			{
				return BadRequest();
			}
		}
	}
}
