using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;


namespace ComputerPartsShop.Services
{
	public class AddressService : IAddressService
	{
		private readonly IAddressRepository _addressRepository;
		private readonly ICountryRepository _countryRepository;

		public AddressService(IAddressRepository addressRepository, ICountryRepository countryRepository)
		{
			_addressRepository = addressRepository;
			_countryRepository = countryRepository;
		}


		public async Task<List<AddressResponse>> GetListAsync(CancellationToken ct)
		{
			var addressList = await _addressRepository.GetListAsync(ct);

			return addressList.Select(a => new AddressResponse(a.Id, a.Street, a.City, a.Region, a.ZipCode, a.Country.Alpha3)).ToList();
		}

		public async Task<AddressResponse> GetAsync(Guid id, CancellationToken ct)
		{
			var address = await _addressRepository.GetAsync(id, ct);

			return address == null ? null! : new AddressResponse(address.Id, address.Street, address.City, address.Region, address.ZipCode, address.Country.Alpha3);
		}

		public async Task<AddressResponse> CreateAsync(AddressRequest entity, CancellationToken ct)
		{
			var country = await _countryRepository.GetByCountry3CodeAsync(entity.Country3Code, ct);

			var newAddres = new Address()
			{
				Street = entity.Street,
				City = entity.City,
				Region = entity.Region,
				ZipCode = entity.ZipCode,
				CountryId = country.Id,
				Country = country
			};

			var createdAddressId = await _addressRepository.CreateAsync(newAddres, ct);

			return new AddressResponse(createdAddressId, entity.Street, entity.City, entity.Region, entity.ZipCode, entity.Country3Code);
		}

		public async Task<AddressResponse> UpdateAsync(Guid id, AddressRequest entity, CancellationToken ct)
		{
			var country = await _countryRepository.GetByCountry3CodeAsync(entity.Country3Code, ct);

			var address = new Address()
			{
				Street = entity.Street,
				City = entity.City,
				Region = entity.Region,
				ZipCode = entity.ZipCode,
				CountryId = country.Id,
				Country = country
			};

			await _addressRepository.UpdateAsync(id, address, ct);

			return new AddressResponse(id, entity.Street, entity.City, entity.Region, entity.ZipCode, entity.Country3Code);
		}

		public async Task DeleteAsync(Guid id, CancellationToken ct)
		{
			await _addressRepository.DeleteAsync(id, ct);
		}
	}
}
