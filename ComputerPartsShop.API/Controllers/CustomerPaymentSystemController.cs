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
		public async Task<ActionResult<List<CustomerPaymentSystemResponse>>> GetCustomerPaymentSystemListAsync()
		{
			var cpsList = await _cpsService.GetListAsync();

			return Ok(cpsList);
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult<DetailedCustomerPaymentSystemResponse>> GetCustomerPaymentSystemAsync(Guid id)
		{
			var cps = await _cpsService.GetAsync(id);

			if (cps == null)
			{
				return NotFound();
			}

			return Ok(cps);
		}

		[HttpPost]
		public async Task<ActionResult<CustomerPaymentSystemResponse>> CreateCustomerPaymentSystemAsync(CustomerPaymentSystemRequest request)
		{

			if (string.IsNullOrWhiteSpace(request.Username) && string.IsNullOrWhiteSpace(request.Email))
			{
				return BadRequest();
			}

			var payment = await _providerRepository.GetByNameAsync(request.ProviderName);
			var customer = await _customerRepository.GetByUsernameOrEmailAsync(request.Username! ?? request.Email!);

			if (payment == null)
			{
				return BadRequest();
			}

			if (customer == null)
			{
				return BadRequest();
			}

			var cps = await _cpsService.CreateAsync(request);

			return CreatedAtAction(nameof(CreateCustomerPaymentSystemAsync), cps);
		}

		[HttpPut("{id:guid}")]
		public async Task<ActionResult<CustomerPaymentSystemResponse>> UpdateCustomerPaymentSystemAsync(Guid id, CustomerPaymentSystemRequest request)
		{
			var cps = await _cpsService.GetAsync(id);
			var payment = await _providerRepository.GetByNameAsync(request.ProviderName);
			var customer = await _customerRepository.GetByUsernameOrEmailAsync(request.Username! ?? request.Email!);

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

			var cpsUpdated = await _cpsService.UpdateAsync(id, request);

			return Ok(cpsUpdated);
		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> DeleteCustomerPaymentSystemAsync(Guid id)
		{
			var cps = await _cpsService.GetAsync(id);

			if (cps == null)
			{
				return NotFound();
			}

			await _cpsService.DeleteAsync(id);

			return Ok();
		}
	}
}
