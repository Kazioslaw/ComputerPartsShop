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

			if (customer == null)
			{
				return null;
			}

			return new DetailedCustomerResponse(customer.Id, customer.FirstName, customer.LastName, customer.Username, customer.Email, customer.PhoneNumber,
				customer.CustomersAddresses.Select(x => new AddressResponse(x.AddressId, x.Address.Street, x.Address.City, x.Address.Region, x.Address.ZipCode,
				x.Address.Country.Alpha3)).ToList(), customer.PaymentInfoList.Select(x => new PaymentInfoInCustomerResponse(x.Id, x.Provider.Name, x.PaymentReference)).ToList(),
				customer.Reviews.Select(x => new ReviewInCustomerResponse(x.Id, x.Product.Name, x.Rating, x.Description)).ToList());
		}

		public async Task<CustomerResponse> GetByUsernameOrEmailAsync(string usernameOrEmail, CancellationToken ct)
		{
			var customer = await _customerRepository.GetByUsernameOrEmailAsync(usernameOrEmail, ct);

			if (customer == null)
			{
				return null;
			}

			return new CustomerResponse(customer.Id, customer.FirstName, customer.LastName, customer.Username, customer.Email, customer.PhoneNumber);
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

			var createdCustomer = await _customerRepository.CreateAsync(newCustomer, ct);

			return new CustomerResponse(createdCustomer.Id, entity.FirstName, entity.LastName, entity.Username, entity.Email, entity.PhoneNumber);
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

			var result = await _customerRepository.UpdateAsync(id, customer, ct);

			return new CustomerResponse(id, result.FirstName, result.LastName, result.Username, result.Email, result.PhoneNumber);
		}

		public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
		{
			return await _customerRepository.DeleteAsync(id, ct);
		}
	}
}
