using AutoMapper;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;
using Microsoft.Data.SqlClient;

namespace ComputerPartsShop.Services
{
	public class UserPaymentSystemService : IUserPaymentSystemService
	{
		private readonly IUserPaymentSystemRepository _userPaymentSystemRepository;
		private readonly IShopUserRepository _userRepository;
		private readonly IPaymentProviderRepository _providerRepository;
		private readonly IMapper _mapper;

		public UserPaymentSystemService(IUserPaymentSystemRepository userPaymentSystemRepository, IShopUserRepository
			userRepository, IPaymentProviderRepository providerRepository, IMapper mapper)
		{
			_userPaymentSystemRepository = userPaymentSystemRepository;
			_userRepository = userRepository;
			_providerRepository = providerRepository;
			_mapper = mapper;
		}

		public async Task<List<UserPaymentSystemResponse>> GetListAsync(CancellationToken ct)
		{
			try
			{
				var result = await _userPaymentSystemRepository.GetListAsync(ct);

				var userPaymentSystemList = _mapper.Map<IEnumerable<UserPaymentSystemResponse>>(result);

				return userPaymentSystemList.ToList();
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<DetailedUserPaymentSystemResponse> GetAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var result = await _userPaymentSystemRepository.GetAsync(id, ct);

				if (result == null)
				{
					throw new DataErrorException(404, "ShopUser Payment System Not Found");
				}

				var userPaymentSystem = _mapper.Map<DetailedUserPaymentSystemResponse>(result);

				return userPaymentSystem;
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<UserPaymentSystemResponse> CreateAsync(UserPaymentSystemRequest entity, CancellationToken ct)
		{
			try
			{
				var provider = await _providerRepository.GetByNameAsync(entity.ProviderName, ct);

				if (provider == null)
				{
					throw new DataErrorException(400, "Invalid provider name");
				}

				var user = await _userRepository.GetByUsernameOrEmailAsync(entity.Username ?? entity.Email, ct);

				if (user == null)
				{
					throw new DataErrorException(400, "Invalid or missing username or email");
				}

				var newUserPaymentSystem = _mapper.Map<UserPaymentSystem>(entity);
				newUserPaymentSystem.UserId = user.Id;
				newUserPaymentSystem.User = user;
				newUserPaymentSystem.ProviderId = provider.Id;
				newUserPaymentSystem.Provider = provider;

				var result = await _userPaymentSystemRepository.CreateAsync(newUserPaymentSystem, ct);

				var createdCPS = _mapper.Map<UserPaymentSystemResponse>(result);

				return createdCPS;
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<UserPaymentSystemResponse> UpdateAsync(Guid id, UserPaymentSystemRequest entity, CancellationToken ct)
		{
			try
			{
				var userPaymentSystem = await _userPaymentSystemRepository.GetAsync(id, ct);

				if (userPaymentSystem == null)
				{
					throw new DataErrorException(404, "ShopUser Payment System not found");
				}

				var user = await _userRepository.GetByUsernameOrEmailAsync(entity.Username! ?? entity.Email!, ct);

				if (user == null)
				{
					throw new DataErrorException(400, "Invalid or missing username or email");
				}

				var provider = await _providerRepository.GetByNameAsync(entity.ProviderName, ct);

				if (provider == null)
				{
					throw new DataErrorException(400, "Invalid provider name");
				}

				var userPaymentSystemToUpdate = _mapper.Map<UserPaymentSystem>(entity);
				userPaymentSystemToUpdate.ProviderId = provider.Id;
				userPaymentSystemToUpdate.Provider = provider;
				userPaymentSystemToUpdate.UserId = user.Id;
				userPaymentSystemToUpdate.User = user;

				var result = await _userPaymentSystemRepository.UpdateAsync(id, userPaymentSystemToUpdate, ct);

				var updatedUserPaymentSystem = _mapper.Map<UserPaymentSystemResponse>(result);

				return updatedUserPaymentSystem;
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
				var userPaymentSystem = await _userPaymentSystemRepository.GetAsync(id, ct);

				if (userPaymentSystem == null)
				{
					throw new DataErrorException(404, "ShopUser payment system not found");
				}

				await _userPaymentSystemRepository.DeleteAsync(id, ct);
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}
	}
}
