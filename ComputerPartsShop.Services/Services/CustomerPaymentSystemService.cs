using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class CustomerPaymentSystemService : IService<CustomerPaymentSystemRequest, CustomerPaymentSystemResponse, DetailedCustomerPaymentSystemResponse, Guid>
	{
		private readonly IRepository<CustomerPaymentSystem, Guid> _cpsRepository;
		private readonly ICustomerRepository _customerRepository;
		private readonly IPaymentProviderRepository _providerRepository;

		public CustomerPaymentSystemService(IRepository<CustomerPaymentSystem, Guid> cpsRepository,
			ICustomerRepository customerRepository, IPaymentProviderRepository providerRepository)
		{
			_cpsRepository = cpsRepository;
			_customerRepository = customerRepository;
			_providerRepository = providerRepository;
		}

		public async Task<List<CustomerPaymentSystemResponse>> GetListAsync()
		{
			var cpsList = await _cpsRepository.GetListAsync();

			return cpsList.Select(cps => new CustomerPaymentSystemResponse(cps.ID, cps.Customer.Username, cps.Customer.Email, cps.Provider.Name, cps.PaymentReference)).ToList();
		}

		public async Task<DetailedCustomerPaymentSystemResponse> GetAsync(Guid id)
		{
			var cps = await _cpsRepository.GetAsync(id);
			var paymentsList = cps.Payments;

			return cps == null ? null! : new DetailedCustomerPaymentSystemResponse(cps.ID, cps.Customer.Username, cps.Customer.Email, cps.Provider.Name, cps.PaymentReference,
				paymentsList.Select(p => new PaymentInCustomerPaymentSystemResponse(p.ID, p.OrderID, p.Total, p.Method, p.Status, p.PaymentStartAt, p.PaidAt)).ToList());
		}

		public async Task<CustomerPaymentSystemResponse> CreateAsync(CustomerPaymentSystemRequest cps)
		{
			Customer customer = new();
			var provider = await _providerRepository.GetByNameAsync(cps.ProviderName);

			if (!string.IsNullOrWhiteSpace(cps.Username))
			{
				customer = await _customerRepository.GetByUsernameOrEmailAsync(cps.Username);
			}

			if (!string.IsNullOrWhiteSpace(cps.Email) && string.IsNullOrWhiteSpace(cps.Username))
			{
				customer = await _customerRepository.GetByUsernameOrEmailAsync(cps.Email);
			}


			var newCustomerPaymentSystem = new CustomerPaymentSystem()
			{
				CustomerID = customer.ID,
				Customer = customer,
				ProviderID = provider.ID,
				Provider = provider,
				PaymentReference = cps.PaymentReference
			};

			var createdCPSID = await _cpsRepository.CreateAsync(newCustomerPaymentSystem);

			return new CustomerPaymentSystemResponse(createdCPSID, cps.Username, cps.Email, cps.ProviderName, cps.PaymentReference);

		}

		public async Task<CustomerPaymentSystemResponse> UpdateAsync(Guid id, CustomerPaymentSystemRequest updatedCPS)
		{
			var customer = await _customerRepository.GetByUsernameOrEmailAsync(updatedCPS.Username! ?? updatedCPS.Email!);
			var provider = await _providerRepository.GetByNameAsync(updatedCPS.ProviderName);

			var cps = new CustomerPaymentSystem()
			{
				CustomerID = customer.ID,
				Customer = customer,
				ProviderID = provider.ID,
				Provider = provider,
				PaymentReference = updatedCPS.PaymentReference
			};

			await _cpsRepository.UpdateAsync(id, cps);

			return new CustomerPaymentSystemResponse(id, updatedCPS.Username, updatedCPS.Email, updatedCPS.ProviderName, updatedCPS.PaymentReference);
		}

		public async Task DeleteAsync(Guid id)
		{
			await _cpsRepository.DeleteAsync(id);
		}
	}
}
