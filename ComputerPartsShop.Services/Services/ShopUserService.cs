using AutoMapper;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;
using Microsoft.Data.SqlClient;

namespace ComputerPartsShop.Services
{
	public class ShopUserService : IShopUserService
	{
		private readonly IShopUserRepository _userRepository;
		private readonly IMapper _mapper;

		public ShopUserService(IShopUserRepository userRepository, IMapper mapper)
		{
			_userRepository = userRepository;
			_mapper = mapper;
		}

		public async Task<List<ShopUserResponse>> GetListAsync(CancellationToken ct)
		{
			try
			{
				var result = await _userRepository.GetListAsync(ct);

				var userList = _mapper.Map<IEnumerable<ShopUserResponse>>(result);

				return userList.ToList();
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<DetailedShopUserResponse> GetAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var result = await _userRepository.GetAsync(id, ct);

				if (result == null)
				{
					throw new DataErrorException(404, "ShopUser not found");
				}

				var user = _mapper.Map<DetailedShopUserResponse>(result);

				return user;
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<ShopUserWithAddressResponse> GetByUsernameOrEmailAsync(string input, CancellationToken ct)
		{
			try
			{
				var result = await _userRepository.GetByUsernameOrEmailAsync(input, ct);

				if (result == null)
				{
					throw new DataErrorException(400, "Invalid or empty username or email");
				}

				var user = _mapper.Map<ShopUserWithAddressResponse>(result);

				return user;
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<ShopUserResponse> CreateAsync(ShopUserRequest entity, CancellationToken ct)
		{
			try
			{
				var existingUsername = await _userRepository.GetByUsernameOrEmailAsync(entity.Username, ct);
				var existingEmail = await _userRepository.GetByUsernameOrEmailAsync(entity.Email, ct);

				if (existingUsername != null || existingEmail != null)
				{
					throw new DataErrorException(400, "Failed to create user");
				}

				var newUser = _mapper.Map<ShopUser>(entity);

				var result = await _userRepository.CreateAsync(newUser, ct);

				var createdUser = _mapper.Map<ShopUserResponse>(result);

				return createdUser;
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<ShopUserResponse> UpdateAsync(Guid id, ShopUserRequest entity, CancellationToken ct)
		{
			try
			{
				var user = await _userRepository.GetAsync(id, ct);

				if (user == null)
				{
					throw new DataErrorException(404, "ShopUser not found");
				}

				var userToUpdate = _mapper.Map<ShopUser>(entity);

				var result = await _userRepository.UpdateAsync(id, userToUpdate, ct);

				var updatedUser = _mapper.Map<ShopUserResponse>(result);

				return updatedUser;
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
				var user = await _userRepository.GetAsync(id, ct);

				if (user == null)
				{
					throw new DataErrorException(404, "ShopUser not found");
				}

				await _userRepository.DeleteAsync(id, ct);
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}
	}
}
