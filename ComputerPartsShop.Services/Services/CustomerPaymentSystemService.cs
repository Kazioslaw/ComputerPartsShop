using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class CustomerPaymentSystemService : ICustomerPaymentSystemService
	{
		private readonly ICustomerPaymentSystemRepository _cpsRepository;
		private readonly ICustomerRepository _customerRepository;
		private readonly IPaymentProviderRepository _providerRepository;

		public CustomerPaymentSystemService(ICustomerPaymentSystemRepository cpsRepository, ICustomerRepository customerRepository, IPaymentProviderRepository providerRepository)
		{
			_cpsRepository = cpsRepository;
			_customerRepository = customerRepository;
			_providerRepository = providerRepository;
		}

		public async Task<List<CustomerPaymentSystemResponse>> GetListAsync(CancellationToken ct)
		{
			var cpsList = await _cpsRepository.GetListAsync(ct);

			return cpsList.Select(cps => new CustomerPaymentSystemResponse(cps.ID, cps.Customer.Username, cps.Customer.Email, cps.Provider.Name, cps.PaymentReference)).ToList();
		}

		public async Task<DetailedCustomerPaymentSystemResponse> GetAsync(Guid id, CancellationToken ct)
		{
			var cps = await _cpsRepository.GetAsync(id, ct);
			var paymentsList = cps.Payments;

			return cps == null ? null! : new DetailedCustomerPaymentSystemResponse(cps.ID, cps.Customer.Username, cps.Customer.Email, cps.Provider.Name, cps.PaymentReference,
				paymentsList.Select(p => new PaymentInCustomerPaymentSystemResponse(p.ID, p.OrderID, p.Total, p.Method, p.Status, p.PaymentStartAt, p.PaidAt)).ToList());
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
				CustomerID = customer.ID,
				Customer = customer,
				ProviderID = provider.ID,
				Provider = provider,
				PaymentReference = entity.PaymentReference
			};

			var createdCPSID = await _cpsRepository.CreateAsync(newCustomerPaymentSystem, ct);

			return new CustomerPaymentSystemResponse(createdCPSID, entity.Username, entity.Email, entity.ProviderName, entity.PaymentReference);

		}

		public async Task<CustomerPaymentSystemResponse> UpdateAsync(Guid id, CustomerPaymentSystemRequest entity, CancellationToken ct)
		{
			var customer = await _customerRepository.GetByUsernameOrEmailAsync(entity.Username! ?? entity.Email!, ct);
			var provider = await _providerRepository.GetByNameAsync(entity.ProviderName, ct);

			var cps = new CustomerPaymentSystem()
			{
				CustomerID = customer.ID,
				Customer = customer,
				ProviderID = provider.ID,
				Provider = provider,
				PaymentReference = entity.PaymentReference
			};

			await _cpsRepository.UpdateAsync(id, cps, ct);

			return new CustomerPaymentSystemResponse(id, entity.Username, entity.Email, entity.ProviderName, entity.PaymentReference);
		}

		public async Task DeleteAsync(Guid id, CancellationToken ct)
		{
			await _cpsRepository.DeleteAsync(id, ct);
		}
	}
}
