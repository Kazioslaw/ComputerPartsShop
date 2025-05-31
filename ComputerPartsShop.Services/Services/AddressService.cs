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

		public AddressService(IAddressRepository addressRepository, ICountryRepository countryRepository, ICustomerRepository customerRepository)
		{
			_addressRepository = addressRepository;
			_countryRepository = countryRepository;
			_customerRepository = customerRepository;
		}


		public async Task<List<AddressResponse>> GetListAsync(CancellationToken ct)
		{
			var addressList = await _addressRepository.GetListAsync(ct);

			return addressList.Select(a => new AddressResponse(a.Id, a.Street, a.City, a.Region, a.ZipCode,
				a.Country == null ? "Empty" : a.Country.Alpha3)).ToList();
		}

		public async Task<AddressResponse> GetAsync(Guid id, CancellationToken ct)
		{
			var address = await _addressRepository.GetAsync(id, ct);

			if (address == null)
			{
				return null;
			}

			return new AddressResponse(address.Id, address.Street, address.City, address.Region, address.ZipCode,
				address.Country == null ? "Empty" : address.Country.Alpha3);
		}

		public async Task<AddressResponse> CreateAsync(AddressRequest entity, CancellationToken ct)
		{
			var country = await _countryRepository.GetByCountry3CodeAsync(entity.Country3Code, ct);

			var customer = await _customerRepository.GetByUsernameOrEmailAsync(entity.Username ?? entity.Email, ct);

			if (customer == null)
			{
				return null;
			}

			var newAddres = new Address()
			{
				Street = entity.Street,
				City = entity.City,
				Region = entity.Region,
				ZipCode = entity.ZipCode,
				CountryId = country.Id,
			};

			var createdAddress = await _addressRepository.CreateAsync(newAddres, customer, ct);

			return createdAddress == null ? null! : new AddressResponse(createdAddress.Id, entity.Street, entity.City, entity.Region, entity.ZipCode, entity.Country3Code);
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

			var newAddress = new Address()
			{
				Street = entity.newStreet,
				City = entity.newCity,
				Region = entity.newRegion,
				ZipCode = entity.newZipCode,
				CountryId = country.Id,
				Country = country
			};

			var existingAddress = await _addressRepository.GetAddressIDByFullDataAsync(newAddress, ct);

			if (existingAddress != Guid.Empty)
			{
				newAddress.Id = existingAddress;
			}

			await _addressRepository.UpdateAsync(id, newAddress, oldCustomer.Id, newCustomer.Id, ct);

			return new AddressResponse(newAddress.Id, newAddress.Street, newAddress.City, newAddress.Region, newAddress.ZipCode, newAddress.Country.Alpha3);
		}

		public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
		{
			return await _addressRepository.DeleteAsync(id, ct);
		}
	}
}
