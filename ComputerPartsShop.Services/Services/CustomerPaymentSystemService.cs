using AutoMapper;
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
		private readonly IMapper _mapper;

		public CustomerPaymentSystemService(ICustomerPaymentSystemRepository customerPaymentSystemRepository, ICustomerRepository
			customerRepository, IPaymentProviderRepository providerRepository, IMapper mapper)
		{
			_customerPaymentSystemRepository = customerPaymentSystemRepository;
			_customerRepository = customerRepository;
			_providerRepository = providerRepository;
			_mapper = mapper;
		}

		public async Task<List<CustomerPaymentSystemResponse>> GetListAsync(CancellationToken ct)
		{
			var result = await _customerPaymentSystemRepository.GetListAsync(ct);

			var customerPaymentSystemList = _mapper.Map<IEnumerable<CustomerPaymentSystemResponse>>(result);

			return customerPaymentSystemList.ToList();
		}

		public async Task<DetailedCustomerPaymentSystemResponse> GetAsync(Guid id, CancellationToken ct)
		{
			var result = await _customerPaymentSystemRepository.GetAsync(id, ct);

			if (result == null)
			{
				return null;
			}

			var customerPaymentSystem = _mapper.Map<DetailedCustomerPaymentSystemResponse>(result);

			return customerPaymentSystem;
		}

		public async Task<CustomerPaymentSystemResponse> CreateAsync(CustomerPaymentSystemRequest entity, CancellationToken ct)
		{
			var provider = await _providerRepository.GetByNameAsync(entity.ProviderName, ct);
			var customer = await _customerRepository.GetByUsernameOrEmailAsync(entity.Username ?? entity.Email, ct);

			if (provider == null || customer == null)
			{
				return null;
			}

			var newCustomerPaymentSystem = _mapper.Map<CustomerPaymentSystem>(entity);
			newCustomerPaymentSystem.CustomerId = customer.Id;
			newCustomerPaymentSystem.Customer = customer;
			newCustomerPaymentSystem.ProviderId = provider.Id;
			newCustomerPaymentSystem.Provider = provider;

			var result = await _customerPaymentSystemRepository.CreateAsync(newCustomerPaymentSystem, ct);

			if (result == null)
			{
				return null;
			}

			var createdCPS = _mapper.Map<CustomerPaymentSystemResponse>(result);

			return createdCPS;

		}

		public async Task<CustomerPaymentSystemResponse> UpdateAsync(Guid id, CustomerPaymentSystemRequest entity, CancellationToken ct)
		{
			var customer = await _customerRepository.GetByUsernameOrEmailAsync(entity.Username! ?? entity.Email!, ct);
			var provider = await _providerRepository.GetByNameAsync(entity.ProviderName, ct);

			if (provider == null || customer == null)
			{
				return null;
			}

			var customerPaymentSystemToUpdate = _mapper.Map<CustomerPaymentSystem>(entity);
			customerPaymentSystemToUpdate.ProviderId = provider.Id;
			customerPaymentSystemToUpdate.Provider = provider;
			customerPaymentSystemToUpdate.CustomerId = customer.Id;
			customerPaymentSystemToUpdate.Customer = customer;

			var result = await _customerPaymentSystemRepository.UpdateAsync(id, customerPaymentSystemToUpdate, ct);

			if (result == null)
			{
				return null;
			}

			var updatedCustomerPaymentSystem = _mapper.Map<CustomerPaymentSystemResponse>(result);

			return updatedCustomerPaymentSystem;
		}

		public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
		{
			return await _customerPaymentSystemRepository.DeleteAsync(id, ct);
		}
	}
}
