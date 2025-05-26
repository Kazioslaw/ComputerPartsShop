using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class CustomerService : ICustomerService
	{
		private readonly ICustomerRepository _customerRepository;

		public CustomerService(ICustomerRepository customerRepository)
		{
			_customerRepository = customerRepository;
		}

		public async Task<List<CustomerResponse>> GetListAsync(CancellationToken ct)
		{
			var customerList = await _customerRepository.GetListAsync(ct);

			return customerList.Select(c => new CustomerResponse(c.Id, c.FirstName, c.LastName, c.Username, c.Email, c.PhoneNumber)).ToList();
		}

		public async Task<DetailedCustomerResponse> GetAsync(Guid id, CancellationToken ct)
		{
			var customer = await _customerRepository.GetAsync(id, ct);
			var addressLsit = customer.CustomersAddresses.Select(c => c.Address);
			var customerPaymentSystemList = customer.PaymentInfoList;
			var reviewList = customer.Reviews;

			return customer == null ? null! : new DetailedCustomerResponse(customer.Id, customer.FirstName, customer.LastName,
				customer.Username, customer.Email, customer.PhoneNumber,
				addressLsit.Select(a => new AddressResponse(a.Id, a.Street, a.City, a.Region, a.ZipCode, a.Country.Alpha3)).ToList(),
				customerPaymentSystemList.Select(customerPaymentSystem => new PaymentInfoInCustomerResponse(customerPaymentSystem.Id,
				customerPaymentSystem.Provider.Name, customerPaymentSystem.PaymentReference)).ToList(),
				reviewList!.Select(r => new ReviewInCustomerResponse(r.Id, r.Product.Name, r.Rating, r.Description)).ToList() ?? new List<ReviewInCustomerResponse>());
		}

		public async Task<CustomerResponse> CreateAsync(CustomerRequest entity, CancellationToken ct)
		{
			var newCustomer = new Customer()
			{
				FirstName = entity.FirstName,
				LastName = entity.LastName,
				Username = entity.Username,
				Email = entity.Email,
				PhoneNumber = entity.PhoneNumber,
			};

			var createdCustomerId = await _customerRepository.CreateAsync(newCustomer, ct);

			return new CustomerResponse(createdCustomerId, entity.FirstName, entity.LastName, entity.Username, entity.Email, entity.PhoneNumber);
		}

		public async Task<CustomerResponse> UpdateAsync(Guid id, CustomerRequest entity, CancellationToken ct)
		{

			var customer = new Customer()
			{
				FirstName = entity.FirstName,
				LastName = entity.LastName,
				Username = entity.Username,
				Email = entity.Email,
				PhoneNumber = entity.PhoneNumber,
			};

			await _customerRepository.UpdateAsync(id, customer, ct);

			return new CustomerResponse(id, customer.FirstName, customer.LastName, customer.Username, customer.Email, customer.PhoneNumber);
		}

		public async Task DeleteAsync(Guid id, CancellationToken ct)
		{
			await _customerRepository.DeleteAsync(id, ct);
		}
	}
}
