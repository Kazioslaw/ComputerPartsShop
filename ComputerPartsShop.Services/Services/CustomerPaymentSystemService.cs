using ComputerPartsShop.Domain.DTOs;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class CustomerPaymentSystemService : ICRUDService<CustomerPaymentSystemRequest, CustomerPaymentSystemResponse, DetailedCustomerPaymentSystemResponse, Guid>
	{
		private readonly CustomerPaymentSystemRepository _cpsRepository;
		private readonly CustomerRepository _customerRepository;
		private readonly PaymentProviderRepository _providerRepository;

		public CustomerPaymentSystemService(CustomerPaymentSystemRepository cpsRepository, CustomerRepository customerRepository, PaymentProviderRepository providerRepository)
		{
			_cpsRepository = cpsRepository;
			_customerRepository = customerRepository;
			_providerRepository = providerRepository;
		}

		public async Task<List<CustomerPaymentSystemResponse>> GetList()
		{
			var cpsList = await _cpsRepository.GetList();

			return cpsList.Select(cps => new CustomerPaymentSystemResponse(cps.ID, cps.Customer.Username, cps.Customer.Email, cps.Provider.Name, cps.PaymentReference)).ToList();
		}

		public async Task<DetailedCustomerPaymentSystemResponse> Get(Guid id)
		{
			var cps = await _cpsRepository.Get(id);
			var paymentsList = cps.Payments;

			return cps == null ? null! : new DetailedCustomerPaymentSystemResponse(cps.ID, cps.Customer.Username, cps.Customer.Email, cps.Provider.Name, cps.PaymentReference,
				paymentsList.Select(p => new PaymentInCustomerPaymentSystemResponse(p.ID, p.OrderID, p.Total, p.Method, p.Status, p.PaymentStartAt, p.PaidAt)).ToList());
		}

		public async Task<CustomerPaymentSystemResponse> Create(CustomerPaymentSystemRequest cps)
		{
			Customer customer = new();
			var provider = await _providerRepository.GetByName(cps.ProviderName);

			if (!string.IsNullOrWhiteSpace(cps.Username))
			{
				customer = await _customerRepository.GetByUsernameOrEmail(cps.Username);
			}

			if (!string.IsNullOrWhiteSpace(cps.Email) && string.IsNullOrWhiteSpace(cps.Username))
			{
				customer = await _customerRepository.GetByUsernameOrEmail(cps.Email);
			}


			var newCustomerPaymentSystem = new CustomerPaymentSystem()
			{
				CustomerID = customer.ID,
				Customer = customer,
				ProviderID = provider.ID,
				Provider = provider,
				PaymentReference = cps.PaymentReference
			};

			var createdCPSID = await _cpsRepository.Create(newCustomerPaymentSystem);

			return new CustomerPaymentSystemResponse(createdCPSID, cps.Username, cps.Email, cps.ProviderName, cps.PaymentReference);

		}

		public async Task<CustomerPaymentSystemResponse> Update(Guid id, CustomerPaymentSystemRequest updatedCPS)
		{
			var customer = await _customerRepository.GetByUsernameOrEmail(updatedCPS.Username! ?? updatedCPS.Email!);
			var provider = await _providerRepository.GetByName(updatedCPS.ProviderName);

			var cps = new CustomerPaymentSystem()
			{
				CustomerID = customer.ID,
				Customer = customer,
				ProviderID = provider.ID,
				Provider = provider,
				PaymentReference = updatedCPS.PaymentReference
			};

			await _cpsRepository.Update(id, cps);

			return new CustomerPaymentSystemResponse(id, updatedCPS.Username, updatedCPS.Email, updatedCPS.ProviderName, updatedCPS.PaymentReference);
		}

		public async Task Delete(Guid id)
		{
			await _cpsRepository.Delete(id);
		}
	}
}
