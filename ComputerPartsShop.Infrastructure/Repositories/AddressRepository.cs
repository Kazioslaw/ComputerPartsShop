using ComputerPartsShop.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ComputerPartsShop.Infrastructure
{
	public class AddressRepository : IAddressRepository
	{
		private readonly DBContext _dbContext;

		public AddressRepository(DBContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Address>> GetListAsync(string username, CancellationToken ct)
		{
			var query = "SELECT Address.ID, Address.Street, Address.City, Address.Region, Address.ZipCode, Country.Alpha3 FROM Address " +
				"JOIN Country ON Address.CountryID = Country.ID " +
				"JOIN UserAddress ON Address.ID = UserAddress.AddressID " +
				"JOIN ShopUser ON UserAddress.UserID = ShopUser.ID " +
				"WHERE Username = @username";

			using (var connection = await _dbContext.CreateConnection())
			{
				try
				{
					var result = await connection.QueryAsync<Address, Country, Address>(query, (address, country) =>
					{
						address.Country = country;

						return address;
					}, param: new { Username = username }, splitOn: "Alpha3");

					return result.ToList();
				}
				catch (SqlException ex)
				{
					Console.WriteLine(ex.Message);
					throw;
				}
			}
		}

		public async Task<Address> GetAsync(Guid id, string username, CancellationToken ct)
		{
			var query = "SELECT Address.ID, Address.Street, Address.City, Address.Region, Address.ZipCode, Country.Alpha3, " +
				"ShopUser.ID, ShopUser.FirstName, ShopUser.LastName, ShopUser.Username, ShopUser.Email " +
				"FROM Address " +
				"JOIN Country ON Address.CountryID = Country.ID " +
				"LEFT JOIN UserAddress ON Address.ID = UserAddress.AddressID " +
				"LEFT JOIN ShopUser ON UserAddress.UserID = ShopUser.ID WHERE Address.ID = @Id AND ShopUser.Username = @Username";

			var addressDictionary = new Dictionary<Guid, Address>();

			using (var connection = await _dbContext.CreateConnection())
			{
				try
				{
					var result = await connection.QueryAsync<Address, Country, ShopUser, Address>(query, (address, country, user) =>
					{
						if (!addressDictionary.TryGetValue(id, out var currentAddress))
						{
							currentAddress = address;
							currentAddress.Country = country;
							currentAddress.Users = new List<UserAddress>();
							addressDictionary.Add(address.Id, currentAddress);
						}

						if (user != null)
						{
							currentAddress.Users.Add(new UserAddress
							{
								UserId = user.Id,
								User = user,
								AddressId = address.Id,
							});
						}

						return currentAddress;
					}, param: new { Id = id, Username = username }, splitOn: "Alpha3, ID");

					return result.Distinct().FirstOrDefault();
				}
				catch (SqlException ex)
				{
					Console.WriteLine(ex.Message);
					throw;
				}
			}
		}

		public async Task<Guid> GetAddressIDByFullDataAsync(Address request, CancellationToken ct)
		{
			var query = "SELECT Address.ID FROM Address JOIN Country ON Country.ID = Address.CountryID" +
				" WHERE Address.Street = @Street AND Address.City = @City AND Address.Region = @Region AND Address.ZipCode = @ZipCode AND " +
				"Country.Alpha3 = @Alpha3";

			var parameters = new DynamicParameters();
			parameters.Add("Street", request.Street, DbType.String, ParameterDirection.Input);
			parameters.Add("City", request.City, DbType.String, ParameterDirection.Input);
			parameters.Add("Region", request.Region, DbType.String, ParameterDirection.Input);
			parameters.Add("ZipCode", request.ZipCode, DbType.String, ParameterDirection.Input);
			parameters.Add("Alpha3", request.Country.Alpha3, DbType.String, ParameterDirection.Input);

			using (var connection = await _dbContext.CreateConnection())
			{
				try
				{
					var result = await connection.QueryAsync<Guid>(query, parameters);

					return result.FirstOrDefault();
				}
				catch (SqlException ex)
				{
					Console.WriteLine(ex.Message);
					throw;
				}
			}
		}

		public async Task<Address> CreateAsync(Address addressRequest, ShopUser userRequest, CancellationToken ct)
		{
			addressRequest.Id = Guid.NewGuid();
			var addressQuery = "INSERT INTO Address (ID, Street, City, Region, ZipCode, CountryID) VALUES (@Id,@Street, @City, @Region, @ZipCode, @CountryID)";
			var userAddressQuery = "INSERT INTO UserAddress (AddressID, UserID) VALUES (@AddressID, @UserID)";

			var parameters = new DynamicParameters();
			parameters.Add("ID", addressRequest.Id, DbType.Guid, ParameterDirection.Input);
			parameters.Add("Street", addressRequest.Street, DbType.String, ParameterDirection.Input);
			parameters.Add("City", addressRequest.City, DbType.String, ParameterDirection.Input);
			parameters.Add("Region", addressRequest.Region, DbType.String, ParameterDirection.Input);
			parameters.Add("ZipCode", addressRequest.ZipCode, DbType.String, ParameterDirection.Input);
			parameters.Add("CountryID", addressRequest.CountryId, DbType.Int32, ParameterDirection.Input);

			var userAddressParameters = new DynamicParameters();
			userAddressParameters.Add("UserID", userRequest.Id, DbType.Guid, ParameterDirection.Input);
			userAddressParameters.Add("AddressID", addressRequest.Id, DbType.Guid, ParameterDirection.Input);


			using (var connection = await _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						await connection.ExecuteAsync(addressQuery, parameters, transaction);
						await connection.ExecuteAsync(userAddressQuery, userAddressParameters, transaction);
						transaction.Commit();

						return addressRequest;
					}
					catch (SqlException ex)
					{
						transaction.Rollback();
						Console.WriteLine(ex.Message);

						throw;
					}

				}
			}
		}

		public async Task<Address> UpdateAsync(Guid oldAddressId, Address request, Guid oldUserId, Guid newUserId, CancellationToken ct)
		{
			using (var connection = await _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						if (request.Id == Guid.Empty)
						{
							request.Id = Guid.NewGuid();
							var newAddressQuery = "INSERT INTO Address (ID, Street, City, Region, ZipCode, CountryID) VALUES (@ID, @Street, @City, @Region, @ZipCode, @CountryID)";

							var parametersToNewAddress = new DynamicParameters();
							parametersToNewAddress.Add("ID", request.Id, DbType.Guid, ParameterDirection.Input);
							parametersToNewAddress.Add("Street", request.Street, DbType.String, ParameterDirection.Input);
							parametersToNewAddress.Add("City", request.City, DbType.String, ParameterDirection.Input);
							parametersToNewAddress.Add("Region", request.Region, DbType.String, ParameterDirection.Input);
							parametersToNewAddress.Add("ZipCode", request.ZipCode, DbType.String, ParameterDirection.Input);
							parametersToNewAddress.Add("CountryID", request.CountryId, DbType.Int32, ParameterDirection.Input);

							await connection.ExecuteAsync(newAddressQuery, parametersToNewAddress, transaction);
						}

						var deleteQuery = "DELETE FROM UserAddress WHERE AddressID = @AddressID AND UserID = @UserID";

						var paramsToDelete = new DynamicParameters();
						paramsToDelete.Add("AddressID", oldAddressId, DbType.Guid, ParameterDirection.Input);
						paramsToDelete.Add("UserID", oldUserId, DbType.Guid, ParameterDirection.Input);

						var query = "INSERT INTO UserAddress (UserID, AddressID) VALUES (@UserID, @AddressID)";

						var parameters = new DynamicParameters();
						parameters.Add("UserID", newUserId, DbType.Guid, ParameterDirection.Input);
						parameters.Add("AddressID", request.Id, DbType.Guid, ParameterDirection.Input);

						await connection.ExecuteAsync(deleteQuery, paramsToDelete, transaction);
						await connection.ExecuteAsync(query, parameters, transaction);
						transaction.Commit();

						return request;
					}
					catch (SqlException ex)
					{
						transaction.Rollback();
						Console.WriteLine(ex.Message);

						throw;
					}
				}
			}
		}

		public async Task<int> DeleteAsync(Guid id, string username, CancellationToken ct)
		{
			var query = "DELETE FROM Address WHERE ID = @Id AND Username = @Username";

			using (var connection = await _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						var rowsAffected = await connection.ExecuteAsync(query, new { ID = id, Username = username }, transaction);
						transaction.Commit();

						return rowsAffected;
					}
					catch (SqlException ex)
					{
						transaction.Rollback();
						Console.WriteLine(ex.Message);

						throw;
					}
				}
			}
		}
	}
}
