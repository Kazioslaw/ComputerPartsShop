using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class CustomerService : IService<CustomerRequest, CustomerResponse, DetailedCustomerResponse, Guid>
	{
		private readonly ICustomerRepository _customerRepository;

		public CustomerService(ICustomerRepository customerRepository)
		{
			_customerRepository = customerRepository;
		}

		public async Task<List<CustomerResponse>> GetListAsync()
		{
			var customerList = await _customerRepository.GetListAsync();

			return customerList.Select(c => new CustomerResponse(c.ID, c.FirstName, c.LastName, c.Username, c.Email, c.PhoneNumber)).ToList();
		}

		public async Task<DetailedCustomerResponse> GetAsync(Guid id)
		{
			var customer = await _customerRepository.GetAsync(id);
			var addressLsit = customer.CustomersAddresses.Select(c => c.Address);
			var cpsList = customer.PaymentInfoList;
			var reviewList = customer.Reviews;

			return customer == null ? null! : new DetailedCustomerResponse(customer.ID, customer.FirstName, customer.LastName, customer.Username, customer.Email, customer.PhoneNumber,
				addressLsit.Select(a => new AddressResponse(a.ID, a.Street, a.City, a.Region, a.ZipCode, a.Country.Alpha3)).ToList(),
				cpsList.Select(cps => new PaymentInfoInCustomerResponse(cps.ID, cps.Provider.Name, cps.PaymentReference)).ToList(),
				reviewList!.Select(r => new ReviewInCustomerResponse(r.ID, r.Product.Name, r.Rating, r.Description)).ToList() ?? new List<ReviewInCustomerResponse>());
		}

		public async Task<CustomerResponse> CreateAsync(CustomerRequest customer)
		{
			var newCustomer = new Customer()
			{
				FirstName = customer.FirstName,
				LastName = customer.LastName,
				Username = customer.Username,
				Email = customer.Email,
				PhoneNumber = customer.PhoneNumber,
			};

			var createdCustomerID = await _customerRepository.CreateAsync(newCustomer);

			return new CustomerResponse(createdCustomerID, customer.FirstName, customer.LastName, customer.Username, customer.Email, customer.PhoneNumber);
		}

		public async Task<CustomerResponse> UpdateAsync(Guid id, CustomerRequest updatedCustomer)
		{

			var customer = new Customer()
			{
				FirstName = updatedCustomer.FirstName,
				LastName = updatedCustomer.LastName,
				Username = updatedCustomer.Username,
				Email = updatedCustomer.Email,
				PhoneNumber = updatedCustomer.PhoneNumber,
			};

			await _customerRepository.UpdateAsync(id, customer);

			return new CustomerResponse(id, customer.FirstName, customer.LastName, customer.Username, customer.Email, customer.PhoneNumber);
		}

		public async Task DeleteAsync(Guid id)
		{
			await _customerRepository.DeleteAsync(id);
		}
	}
}
