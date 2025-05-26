using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Infrastructure;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class CustomerPaymentSystemController : ControllerBase
	{

		private readonly ICustomerPaymentSystemService _cpsService;
		private readonly IPaymentProviderRepository _providerRepository;
		private readonly ICustomerRepository _customerRepository;

		public CustomerPaymentSystemController(ICustomerPaymentSystemService cpsService,
			IPaymentProviderRepository providerRepository, ICustomerRepository customerRepository)
		{
			_cpsService = cpsService;
			_providerRepository = providerRepository;
			_customerRepository = customerRepository;
		}

		[HttpGet]
		public async Task<ActionResult<List<CustomerPaymentSystemResponse>>> GetCustomerPaymentSystemListAsync(CancellationToken ct)
		{
			try
			{
				var cpsList = await _cpsService.GetListAsync(ct);

				return Ok(cpsList);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult<DetailedCustomerPaymentSystemResponse>> GetCustomerPaymentSystemAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var cps = await _cpsService.GetAsync(id, ct);

				if (cps == null)
				{
					return NotFound();
				}

				return Ok(cps);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		[HttpPost]
		public async Task<ActionResult<CustomerPaymentSystemResponse>> CreateCustomerPaymentSystemAsync(CustomerPaymentSystemRequest request, CancellationToken ct)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(request.Username) && string.IsNullOrWhiteSpace(request.Email))
				{
					return BadRequest();
				}

				var payment = await _providerRepository.GetByNameAsync(request.ProviderName, ct);
				var customer = await _customerRepository.GetByUsernameOrEmailAsync(request.Username! ?? request.Email!, ct);

				if (payment == null)
				{
					return BadRequest();
				}

				if (customer == null)
				{
					return BadRequest();
				}

				var cps = await _cpsService.CreateAsync(request, ct);

				return CreatedAtAction(nameof(CreateCustomerPaymentSystemAsync), cps);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		[HttpPut("{id:guid}")]
		public async Task<ActionResult<CustomerPaymentSystemResponse>> UpdateCustomerPaymentSystemAsync(Guid id, CustomerPaymentSystemRequest request, CancellationToken ct)
		{
			try
			{
				var cps = await _cpsService.GetAsync(id, ct);
				var payment = await _providerRepository.GetByNameAsync(request.ProviderName, ct);
				var customer = await _customerRepository.GetByUsernameOrEmailAsync(request.Username! ?? request.Email!, ct);

				if (payment == null)
				{
					return BadRequest();

				}

				if (customer == null)
				{
					return BadRequest();
				}

				if (cps == null)
				{
					return NotFound();
				}

				var cpsUpdated = await _cpsService.UpdateAsync(id, request, ct);

				return Ok(cpsUpdated);
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> DeleteCustomerPaymentSystemAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var cps = await _cpsService.GetAsync(id, ct);

				if (cps == null)
				{
					return NotFound();
				}

				await _cpsService.DeleteAsync(id, ct);

				return Ok();
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}
	}
}
