using AutoMapper;
using ComputerPartsShop.Domain;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;
using Microsoft.Data.SqlClient;
using System.Net;

namespace ComputerPartsShop.Services
{
	public class ShopUserService : IShopUserService
	{
		private readonly IShopUserRepository _userRepository;
		private readonly IMapper _mapper;
		private readonly IPasswordHasher _passwordHasher;
		private readonly IAuthTokenProcessor _authTokenProcessor;

		public ShopUserService(IShopUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher, IAuthTokenProcessor authTokenProcessor)
		{
			_authTokenProcessor = authTokenProcessor;
			_passwordHasher = passwordHasher;
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
				throw new DataErrorException(HttpStatusCode.InternalServerError, "Database operation failed");
			}
		}

		public async Task<DetailedShopUserResponse> GetAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var result = await _userRepository.GetAsync(id, ct);

				if (result == null)
				{
					throw new DataErrorException(HttpStatusCode.NotFound, "ShopUser not found");
				}

				var user = _mapper.Map<DetailedShopUserResponse>(result);

				return user;
			}
			catch (SqlException)
			{
				throw new DataErrorException(HttpStatusCode.InternalServerError, "Database operation failed");
			}
		}

		public async Task<ShopUserWithAddressResponse> GetByUsernameOrEmailAsync(string input, CancellationToken ct)
		{
			try
			{
				var result = await _userRepository.GetByUsernameOrEmailAsync(input, ct);

				if (result == null)
				{
					throw new DataErrorException(HttpStatusCode.BadRequest, "Invalid or empty username or email");
				}

				var user = _mapper.Map<ShopUserWithAddressResponse>(result);

				return user;
			}
			catch (SqlException)
			{
				throw new DataErrorException(HttpStatusCode.InternalServerError, "Database operation failed");
			}
		}

		public async Task<ShopUserResponse> CreateAsync(ShopUserRequest request, CancellationToken ct)
		{
			try
			{
				var existingUser = await _userRepository.CheckIfUserExists(request.Username, request.Email, ct);

				if (existingUser)
				{
					throw new DataErrorException(HttpStatusCode.BadRequest, $"User with this: {request.Username} or {request.Email} already exist");
				}


				var newUser = _mapper.Map<ShopUser>(request);
				newUser.PasswordHash = _passwordHasher.Hash(request.Password);
				newUser.Role = UserRole.Customer;

				var result = await _userRepository.CreateAsync(newUser, ct);

				var createdUser = _mapper.Map<ShopUserResponse>(result);

				return createdUser;
			}
			catch (SqlException)
			{
				throw new DataErrorException(HttpStatusCode.InternalServerError, "Database operation failed");
			}
		}

		public async Task<string> LoginAsync(LoginRequest request, CancellationToken ct)
		{
			try
			{
				var user = await _userRepository.GetByUsernameOrEmailAsync(request.Username ?? request.Email, ct);

				if (user == null || !_passwordHasher.Verify(user.PasswordHash, request.Password))
				{
					throw new DataErrorException(HttpStatusCode.Unauthorized, "Username or password is not correct.");
				}

				var token = _authTokenProcessor.GenerateJwtToken(user);
				var refreshTokenValue = _authTokenProcessor.GenerateRefreshToken();

				var refreshTokenExiprationDateInUtc = DateTime.UtcNow.AddDays(7);

				user.RefreshToken = refreshTokenValue;
				user.RefreshTokenExpiresAtUtc = refreshTokenExiprationDateInUtc;

				await _userRepository.UpdateAsync(user.Id, user, ct);

				_authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("REFRESH_TOKEN", user.RefreshToken, refreshTokenExiprationDateInUtc);

				return token.jwtToken;
			}
			catch (SqlException)
			{
				throw new DataErrorException(HttpStatusCode.InternalServerError, "Database operation failed");
			}
		}

		public async Task<string> RefreshTokenAsync(string? refreshToken, CancellationToken ct)
		{
			if (string.IsNullOrEmpty(refreshToken))
			{
				throw new DataErrorException(HttpStatusCode.BadRequest, "Refresh token is missing");
			}

			var user = await _userRepository.GetByRefreshTokenAsync(refreshToken, ct);

			if (user == null)
			{
				throw new DataErrorException(HttpStatusCode.BadRequest, "Unable to retrieve user for refresh token");
			}

			if (user.RefreshTokenExpiresAtUtc < DateTime.UtcNow)
			{
				throw new DataErrorException(HttpStatusCode.BadRequest, "Refresh token is expired");
			}

			var token = _authTokenProcessor.GenerateJwtToken(user);
			var refreshTokenValue = _authTokenProcessor.GenerateRefreshToken();

			var refreshTokenExiprationDateInUtc = DateTime.UtcNow.AddDays(7);

			user.RefreshToken = refreshTokenValue;
			user.RefreshTokenExpiresAtUtc = refreshTokenExiprationDateInUtc;


			await _userRepository.UpdateAsync(user.Id, user, ct);

			_authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie("REFRESH_TOKEN", user.RefreshToken, refreshTokenExiprationDateInUtc);

			return token.jwtToken;
		}

		public async Task<ShopUserResponse> UpdateAsync(Guid id, ShopUserRequest request, CancellationToken ct)
		{
			try
			{
				var user = await _userRepository.GetAsync(id, ct);

				if (user == null)
				{
					throw new DataErrorException(HttpStatusCode.NotFound, "ShopUser not found");
				}

				var userToUpdate = _mapper.Map<ShopUser>(request);

				var result = await _userRepository.UpdateAsync(id, userToUpdate, ct);

				var updatedUser = _mapper.Map<ShopUserResponse>(result);

				return updatedUser;
			}
			catch (SqlException)
			{
				throw new DataErrorException(HttpStatusCode.InternalServerError, "Database operation failed");
			}
		}

		public async Task DeleteAsync(Guid id, CancellationToken ct)
		{
			try
			{
				var user = await _userRepository.GetAsync(id, ct);

				if (user == null)
				{
					throw new DataErrorException(HttpStatusCode.NotFound, "ShopUser not found");
				}

				await _userRepository.DeleteAsync(id, ct);
			}
			catch (SqlException)
			{
				throw new DataErrorException(HttpStatusCode.InternalServerError, "Database operation failed");
			}
		}
	}
}
