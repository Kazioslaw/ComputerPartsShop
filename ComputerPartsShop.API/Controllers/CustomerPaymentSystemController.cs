using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Infrastructure;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CustomerPaymentSystemController : ControllerBase
	{

		private readonly ICustomerPaymentSystemService _customerPaymentSystemService;
		private readonly IPaymentProviderRepository _providerRepository;
		private readonly ICustomerRepository _customerRepository;

		public CustomerPaymentSystemController(ICustomerPaymentSystemService customerPaymentSystemService,
			IPaymentProviderRepository providerRepository, ICustomerRepository customerRepository)
		{
			_customerPaymentSystemService = customerPaymentSystemService;
			_providerRepository = providerRepository;
			_customerRepository = customerRepository;
		}

		[HttpGet]
		public async Task<ActionResult<List<CustomerPaymentSystemResponse>>> GetCustomerPaymentSystemListAsync(CancellationToken ct)
		{
			try
			{
				var customerPaymentSystemList = await _customerPaymentSystemService.GetListAsync(ct);

				return Ok(customerPaymentSystemList);
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
				var customerPaymentSystem = await _customerPaymentSystemService.GetAsync(id, ct);

				if (customerPaymentSystem == null)
				{
					return NotFound();
				}

				return Ok(customerPaymentSystem);
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

				var customerPaymentSystem = await _customerPaymentSystemService.CreateAsync(request, ct);

				return CreatedAtAction(nameof(CreateCustomerPaymentSystemAsync), customerPaymentSystem);
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
				var customerPaymentSystem = await _customerPaymentSystemService.GetAsync(id, ct);
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

				if (customerPaymentSystem == null)
				{
					return NotFound();
				}

				var customerPaymentSystemUpdated = await _customerPaymentSystemService.UpdateAsync(id, request, ct);

				return Ok(customerPaymentSystemUpdated);
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
				var customerPaymentSystem = await _customerPaymentSystemService.GetAsync(id, ct);

				if (customerPaymentSystem == null)
				{
					return NotFound();
				}

				await _customerPaymentSystemService.DeleteAsync(id, ct);

				return Ok();
			}
			catch (OperationCanceledException)
			{
				return StatusCode(StatusCodes.Status499ClientClosedRequest);
			}
		}
	}
}
