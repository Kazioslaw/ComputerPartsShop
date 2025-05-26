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

			return customerList.Select(c => new CustomerResponse(c.ID, c.FirstName, c.LastName, c.Username, c.Email, c.PhoneNumber)).ToList();
		}

		public async Task<DetailedCustomerResponse> GetAsync(Guid id, CancellationToken ct)
		{
			var customer = await _customerRepository.GetAsync(id, ct);
			var addressLsit = customer.CustomersAddresses.Select(c => c.Address);
			var cpsList = customer.PaymentInfoList;
			var reviewList = customer.Reviews;

			return customer == null ? null! : new DetailedCustomerResponse(customer.ID, customer.FirstName, customer.LastName, customer.Username, customer.Email, customer.PhoneNumber,
				addressLsit.Select(a => new AddressResponse(a.ID, a.Street, a.City, a.Region, a.ZipCode, a.Country.Alpha3)).ToList(),
				cpsList.Select(cps => new PaymentInfoInCustomerResponse(cps.ID, cps.Provider.Name, cps.PaymentReference)).ToList(),
				reviewList!.Select(r => new ReviewInCustomerResponse(r.ID, r.Product.Name, r.Rating, r.Description)).ToList() ?? new List<ReviewInCustomerResponse>());
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

			var createdCustomerID = await _customerRepository.CreateAsync(newCustomer, ct);

			return new CustomerResponse(createdCustomerID, entity.FirstName, entity.LastName, entity.Username, entity.Email, entity.PhoneNumber);
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
