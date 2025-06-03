using AutoMapper;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;


namespace ComputerPartsShop.Services
{
	public class AddressService : IAddressService
	{
		private readonly IAddressRepository _addressRepository;
		private readonly ICountryRepository _countryRepository;
		private readonly ICustomerRepository _customerRepository;
		private readonly IMapper _mapper;

		public AddressService(IAddressRepository addressRepository, ICountryRepository countryRepository, ICustomerRepository customerRepository, IMapper mapper)
		{
			_addressRepository = addressRepository;
			_countryRepository = countryRepository;
			_customerRepository = customerRepository;
			_mapper = mapper;
		}


		public async Task<List<AddressResponse>> GetListAsync(CancellationToken ct)
		{
			var result = await _addressRepository.GetListAsync(ct);

			var addressList = _mapper.Map<IEnumerable<AddressResponse>>(result);

			return addressList.ToList();
		}

		public async Task<DetailedAddressResponse> GetAsync(Guid id, CancellationToken ct)
		{
			var result = await _addressRepository.GetAsync(id, ct);

			if (result == null)
			{
				return null;
			}

			var address = _mapper.Map<DetailedAddressResponse>(result);

			return address;
		}



		public async Task<AddressResponse> CreateAsync(AddressRequest entity, CancellationToken ct)
		{
			var country = await _countryRepository.GetByCountry3CodeAsync(entity.Country3Code, ct);

			var customer = await _customerRepository.GetByUsernameOrEmailAsync(entity.Username ?? entity.Email, ct);

			if (customer == null)
			{
				return null;
			}

			var newAddress = _mapper.Map<Address>(entity);
			newAddress.CountryId = country.Id;
			newAddress.Country = country;

			var response = await _addressRepository.CreateAsync(newAddress, customer, ct);

			if (response == null)
			{
				return null;
			}

			return _mapper.Map<AddressResponse>(response);
		}

		public async Task<AddressResponse> UpdateAsync(Guid id, UpdateAddressRequest entity, CancellationToken ct)
		{
			var oldCustomer = await _customerRepository.GetByUsernameOrEmailAsync(entity.oldUsername ?? entity.oldEmail, ct);

			if (oldCustomer == null)
			{
				return null;
			}

			var newCustomer = await _customerRepository.GetByUsernameOrEmailAsync(entity.newUsername ?? entity.newEmail, ct);

			if (newCustomer == null)
			{
				return null;
			}

			var country = await _countryRepository.GetByCountry3CodeAsync(entity.newCountry3Code, ct);

			var newAddress = _mapper.Map<Address>(entity);
			newAddress.Country = country;
			newAddress.CountryId = country.Id;

			var existingAddress = await _addressRepository.GetAddressIDByFullDataAsync(newAddress, ct);

			if (existingAddress != Guid.Empty)
			{
				newAddress.Id = existingAddress;
			}

			await _addressRepository.UpdateAsync(id, newAddress, oldCustomer.Id, newCustomer.Id, ct);

			var result = _mapper.Map<AddressResponse>(newAddress);

			return result;
		}

		public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
		{
			return await _addressRepository.DeleteAsync(id, ct);
		}
	}
}
