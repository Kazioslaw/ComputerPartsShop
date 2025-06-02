using AutoMapper;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class CustomerService : ICustomerService
	{
		private readonly ICustomerRepository _customerRepository;
		private readonly IMapper _mapper;

		public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
		{
			_customerRepository = customerRepository;
			_mapper = mapper;
		}

		public async Task<List<CustomerResponse>> GetListAsync(CancellationToken ct)
		{
			var result = await _customerRepository.GetListAsync(ct);

			var customerList = _mapper.Map<IEnumerable<CustomerResponse>>(result);

			return customerList.ToList();
		}

		public async Task<DetailedCustomerResponse> GetAsync(Guid id, CancellationToken ct)
		{
			var result = await _customerRepository.GetAsync(id, ct);

			if (result == null)
			{
				return null;
			}

			var customer = _mapper.Map<DetailedCustomerResponse>(result);

			return customer;
		}

		public async Task<CustomerWithAddressResponse> GetByUsernameOrEmailAsync(string input, CancellationToken ct)
		{
			var result = await _customerRepository.GetByUsernameOrEmailAsync(input, ct);

			if (result == null)
			{
				return null;
			}

			var customer = _mapper.Map<CustomerWithAddressResponse>(result);

			return customer;
		}

		public async Task<CustomerResponse> CreateAsync(CustomerRequest entity, CancellationToken ct)
		{
			var newCustomer = _mapper.Map<Customer>(entity);

			var result = await _customerRepository.CreateAsync(newCustomer, ct);

			if (result == null)
			{
				return null;
			}

			var createdCustomer = _mapper.Map<CustomerResponse>(result);

			return createdCustomer;
		}

		public async Task<CustomerResponse> UpdateAsync(Guid id, CustomerRequest entity, CancellationToken ct)
		{
			var customerToUpdate = _mapper.Map<Customer>(entity);

			var result = await _customerRepository.UpdateAsync(id, customerToUpdate, ct);

			if (result == null)
			{
				return null;
			}

			var updatedCustomer = _mapper.Map<CustomerResponse>(result);

			return updatedCustomer;
		}

		public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
		{
			return await _customerRepository.DeleteAsync(id, ct);
		}
	}
}
