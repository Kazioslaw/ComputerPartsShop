using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;


namespace ComputerPartsShop.Services
{
	public class AddressService : IService<AddressRequest, AddressResponse, AddressResponse, Guid>
	{
		private readonly IRepository<Address, Guid> _addressRepository;
		private readonly ICountryRepository _countryRepository;

		public AddressService(IRepository<Address, Guid> addressRepository, ICountryRepository countryRepository)
		{
			_addressRepository = addressRepository;
			_countryRepository = countryRepository;
		}


		public async Task<List<AddressResponse>> GetListAsync()
		{
			var addressList = await _addressRepository.GetListAsync();

			return addressList.Select(a => new AddressResponse(a.ID, a.Street, a.City, a.Region, a.ZipCode, a.Country.Alpha3)).ToList();
		}

		public async Task<AddressResponse> GetAsync(Guid id)
		{
			var address = await _addressRepository.GetAsync(id);

			return address == null ? null! : new AddressResponse(address.ID, address.Street, address.City, address.Region, address.ZipCode, address.Country.Alpha3);
		}

		public async Task<AddressResponse> CreateAsync(AddressRequest address)
		{
			var country = await _countryRepository.GetByCountry3CodeAsync(address.Country3Code);

			var newAddres = new Address()
			{
				Street = address.Street,
				City = address.City,
				Region = address.Region,
				ZipCode = address.ZipCode,
				CountryID = country.ID,
				Country = country
			};

			var createdAddressID = await _addressRepository.CreateAsync(newAddres);

			return new AddressResponse(createdAddressID, address.Street, address.City, address.Region, address.ZipCode, address.Country3Code);
		}

		public async Task<AddressResponse> UpdateAsync(Guid id, AddressRequest updatedAddress)
		{
			var country = await _countryRepository.GetByCountry3CodeAsync(updatedAddress.Country3Code);

			var address = new Address()
			{
				Street = updatedAddress.Street,
				City = updatedAddress.City,
				Region = updatedAddress.Region,
				ZipCode = updatedAddress.ZipCode,
				CountryID = country.ID,
				Country = country
			};

			await _addressRepository.UpdateAsync(id, address);

			return new AddressResponse(id, updatedAddress.Street, updatedAddress.City, updatedAddress.Region, updatedAddress.ZipCode, updatedAddress.Country3Code);
		}

		public async Task DeleteAsync(Guid id)
		{
			await _addressRepository.DeleteAsync(id);
		}
	}
}
