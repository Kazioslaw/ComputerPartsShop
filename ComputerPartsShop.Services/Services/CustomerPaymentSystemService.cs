using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class CustomerPaymentSystemService : ICustomerPaymentSystemService
	{
		private readonly ICustomerPaymentSystemRepository _customerPaymentSystemRepository;
		private readonly ICustomerRepository _customerRepository;
		private readonly IPaymentProviderRepository _providerRepository;

		public CustomerPaymentSystemService(ICustomerPaymentSystemRepository customerPaymentSystemRepository, ICustomerRepository
			customerRepository, IPaymentProviderRepository providerRepository)
		{
			_customerPaymentSystemRepository = customerPaymentSystemRepository;
			_customerRepository = customerRepository;
			_providerRepository = providerRepository;
		}

		public async Task<List<CustomerPaymentSystemResponse>> GetListAsync(CancellationToken ct)
		{
			var customerPaymentSystemList = await _customerPaymentSystemRepository.GetListAsync(ct);

			return customerPaymentSystemList.Select(customerPaymentSystem => new CustomerPaymentSystemResponse(customerPaymentSystem.Id,
				customerPaymentSystem.Customer.Username, customerPaymentSystem.Customer.Email, customerPaymentSystem.Provider.Name,
				customerPaymentSystem.PaymentReference)).ToList();
		}

		public async Task<DetailedCustomerPaymentSystemResponse> GetAsync(Guid id, CancellationToken ct)
		{
			var customerPaymentSystem = await _customerPaymentSystemRepository.GetAsync(id, ct);
			var paymentsList = customerPaymentSystem.Payments;

			return customerPaymentSystem == null ? null! : new DetailedCustomerPaymentSystemResponse(customerPaymentSystem.Id, customerPaymentSystem.Customer.Username,
				customerPaymentSystem.Customer.Email, customerPaymentSystem.Provider.Name, customerPaymentSystem.PaymentReference,
				paymentsList.Select(p => new PaymentInCustomerPaymentSystemResponse(p.Id, p.OrderId, p.Total, p.Method, p.Status, p.PaymentStartAt, p.PaidAt)).ToList());
		}

		public async Task<CustomerPaymentSystemResponse> CreateAsync(CustomerPaymentSystemRequest entity, CancellationToken ct)
		{
			Customer customer = new();
			var provider = await _providerRepository.GetByNameAsync(entity.ProviderName, ct);

			if (!string.IsNullOrWhiteSpace(entity.Username))
			{
				customer = await _customerRepository.GetByUsernameOrEmailAsync(entity.Username, ct);
			}

			if (!string.IsNullOrWhiteSpace(entity.Email) && string.IsNullOrWhiteSpace(entity.Username))
			{
				customer = await _customerRepository.GetByUsernameOrEmailAsync(entity.Email, ct);
			}


			var newCustomerPaymentSystem = new CustomerPaymentSystem()
			{
				CustomerId = customer.Id,
				Customer = customer,
				ProviderId = provider.Id,
				Provider = provider,
				PaymentReference = entity.PaymentReference
			};

			var createdCPSId = await _customerPaymentSystemRepository.CreateAsync(newCustomerPaymentSystem, ct);

			return new CustomerPaymentSystemResponse(createdCPSId, entity.Username, entity.Email, entity.ProviderName, entity.PaymentReference);

		}

		public async Task<CustomerPaymentSystemResponse> UpdateAsync(Guid id, CustomerPaymentSystemRequest entity, CancellationToken ct)
		{
			var customer = await _customerRepository.GetByUsernameOrEmailAsync(entity.Username! ?? entity.Email!, ct);
			var provider = await _providerRepository.GetByNameAsync(entity.ProviderName, ct);

			var customerPaymentSystem = new CustomerPaymentSystem()
			{
				CustomerId = customer.Id,
				Customer = customer,
				ProviderId = provider.Id,
				Provider = provider,
				PaymentReference = entity.PaymentReference
			};

			await _customerPaymentSystemRepository.UpdateAsync(id, customerPaymentSystem, ct);

			return new CustomerPaymentSystemResponse(id, entity.Username, entity.Email, entity.ProviderName, entity.PaymentReference);
		}

		public async Task DeleteAsync(Guid id, CancellationToken ct)
		{
			await _customerPaymentSystemRepository.DeleteAsync(id, ct);
		}
	}
}
