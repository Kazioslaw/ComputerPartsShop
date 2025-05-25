using ComputerPartsShop.Domain.DTOs;
using ComputerPartsShop.Infrastructure;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class CustomerPaymentSystemController : ControllerBase
	{
		private readonly CustomerPaymentSystemService _cpsService;
		private readonly PaymentProviderRepository _providerRepository;
		private readonly CustomerRepository _customerRepository;

		public CustomerPaymentSystemController(CustomerPaymentSystemService cpsService, PaymentProviderRepository providerRepository, CustomerRepository customerRepository)
		{
			_cpsService = cpsService;
			_providerRepository = providerRepository;
			_customerRepository = customerRepository;
		}

		[HttpGet]
		public async Task<ActionResult<List<CustomerPaymentSystemResponse>>> GetCustomerPaymentSystemList()
		{
			var cpsList = await _cpsService.GetList();

			return Ok(cpsList);
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult<DetailedCustomerPaymentSystemResponse>> GetCustomerPaymentSystem(Guid id)
		{
			var cps = await _cpsService.Get(id);

			if (cps == null)
			{
				return NotFound();
			}

			return Ok(cps);
		}

		[HttpPost]
		public async Task<ActionResult<CustomerPaymentSystemResponse>> CreateCustomerPaymentSystem(CustomerPaymentSystemRequest request)
		{

			if (string.IsNullOrWhiteSpace(request.Username) && string.IsNullOrWhiteSpace(request.Email))
			{
				return BadRequest();
			}

			var payment = await _providerRepository.GetByName(request.ProviderName);
			var customer = await _customerRepository.GetByUsernameOrEmail(request.Username! ?? request.Email!);

			if (payment == null)
			{
				return BadRequest();
			}

			if (customer == null)
			{
				return BadRequest();
			}

			var cps = await _cpsService.Create(request);

			return CreatedAtAction(nameof(CreateCustomerPaymentSystem), cps);
		}

		[HttpPut("{id:guid}")]
		public async Task<ActionResult<CustomerPaymentSystemResponse>> UpdateCustomerPaymentSystem(Guid id, CustomerPaymentSystemRequest request)
		{
			var cps = await _cpsService.Get(id);
			var payment = await _providerRepository.GetByName(request.ProviderName);
			var customer = await _customerRepository.GetByUsernameOrEmail(request.Username! ?? request.Email!);

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

			var cpsUpdated = await _cpsService.Update(id, request);

			return Ok(cpsUpdated);
		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> DeleteCustomerPaymentSystem(Guid id)
		{
			var cps = await _cpsService.Get(id);

			if (cps == null)
			{
				return NotFound();
			}

			await _cpsService.Delete(id);

			return Ok();
		}
	}
}
