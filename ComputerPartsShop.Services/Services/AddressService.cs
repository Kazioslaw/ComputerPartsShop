using AutoMapper;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;
using Microsoft.Data.SqlClient;


namespace ComputerPartsShop.Services
{
	public class AddressService : IAddressService
	{
		private readonly IAddressRepository _addressRepository;
		private readonly ICountryRepository _countryRepository;
		private readonly IShopUserRepository _userRepository;
		private readonly IMapper _mapper;

		public AddressService(IAddressRepository addressRepository, ICountryRepository countryRepository, IShopUserRepository userRepository, IMapper mapper)
		{
			_addressRepository = addressRepository;
			_countryRepository = countryRepository;
			_userRepository = userRepository;
			_mapper = mapper;
		}


		public async Task<List<AddressResponse>> GetListAsync(CancellationToken ct)
		{
			try
			{
				var result = await _addressRepository.GetListAsync(ct);

				var addressList = _mapper.Map<IEnumerable<AddressResponse>>(result);

				return addressList.ToList();
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<DetailedAddressResponse> GetAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var result = await _addressRepository.GetAsync(id, ct);

				if (result == null)
				{
					throw new DataErrorException(404, "Address not found");
				}

				var address = _mapper.Map<DetailedAddressResponse>(result);

				return address;
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}



		public async Task<AddressResponse> CreateAsync(AddressRequest entity, CancellationToken ct)
		{
			try
			{
				var country = await _countryRepository.GetByCountry3CodeAsync(entity.Country3Code, ct);

				if (country == null)
				{
					throw new DataErrorException(400, "Invalid country code");
				}

				var user = await _userRepository.GetByUsernameOrEmailAsync(entity.Username ?? entity.Email, ct);

				if (user == null)
				{
					throw new DataErrorException(400, "Invalid or missing username or email");
				}

				var newAddress = _mapper.Map<Address>(entity);
				newAddress.CountryId = country.Id;
				newAddress.Country = country;

				var response = await _addressRepository.CreateAsync(newAddress, user, ct);

				return _mapper.Map<AddressResponse>(response);
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<AddressResponse> UpdateAsync(Guid oldAddressId, UpdateAddressRequest entity, CancellationToken ct)
		{

			try
			{
				var address = await _addressRepository.GetAsync(oldAddressId, ct);

				if (address == null)
				{
					throw new DataErrorException(404, "Address not exist");
				}

				var oldUser = await _userRepository.GetByUsernameOrEmailAsync(entity.oldUsername ?? entity.oldEmail, ct);

				if (oldUser == null)
				{
					throw new DataErrorException(400, "Invalid or missing oldUsername or oldEmail");
				}

				var newUser = await _userRepository.GetByUsernameOrEmailAsync(entity.newUsername ?? entity.newEmail, ct);

				if (newUser == null)
				{
					throw new DataErrorException(400, "Invalid or missing newUsername or newEmail");
				}

				var country = await _countryRepository.GetByCountry3CodeAsync(entity.newCountry3Code, ct);

				if (country == null)
				{
					throw new DataErrorException(400, "Invalid or missing country3Code");
				}

				var newAddress = _mapper.Map<Address>(entity);
				newAddress.Country = country;
				newAddress.CountryId = country.Id;

				var existingAddress = await _addressRepository.GetAddressIDByFullDataAsync(newAddress, ct);

				if (existingAddress != Guid.Empty)
				{
					newAddress.Id = existingAddress;
				}
				var updatedAddress = await _addressRepository.UpdateAsync(oldAddressId, newAddress, oldUser.Id, newUser.Id, ct);
				var result = _mapper.Map<AddressResponse>(updatedAddress);

				return result;
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task DeleteAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var existAddress = await _addressRepository.GetAsync(id, ct);

				if (existAddress == null)
				{
					throw new DataErrorException(404, "Address not found");
				}

				await _addressRepository.DeleteAsync(id, ct);
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}
	}
}
