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

		public async Task<List<Address>> GetListAsync(CancellationToken ct)
		{
			var query = "SELECT Address.ID, Address.Street, Address.City, Address.Region, Address.ZipCode, Country.Alpha3 FROM Address " +
				"JOIN Country on Address.CountryID = Country.ID;";

			using (var connection = _dbContext.CreateConnection())
			{
				var result = await connection.QueryAsync<Address, Country, Address>(query, (address, country) =>
				{
					address.Country = country;

					return address;
				}, splitOn: "Alpha3");

				return result.ToList();
			}
		}

		public async Task<Address> GetAsync(Guid id, CancellationToken ct)
		{
			var query = "SELECT Address.ID, Address.Street, Address.City, Address.Region, Address.ZipCode, Country.Alpha3 " +
				"FROM Address JOIN Country ON Address.CountryID = Country.ID WHERE Address.ID = @Id";

			using (var connection = _dbContext.CreateConnection())
			{
				var result = await connection.QueryAsync<Address, Country, Address>(query, (address, country) =>
				{
					address.Country = country;

					return address;
				}, new { Id = id }, splitOn: "Alpha3");

				return result.FirstOrDefault();
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

			using (var connection = _dbContext.CreateConnection())
			{
				var result = await connection.QueryAsync<Guid>(query, parameters);

				return result.FirstOrDefault();
			}
		}

		public async Task<Address> CreateAsync(Address request, Customer customer, CancellationToken ct)
		{
			request.Id = Guid.NewGuid();
			var addressQuery = "INSERT INTO Address (ID, Street, City, Region, ZipCode, CountryID) VALUES (@Id,@Street, @City, @Region, @ZipCode, @CountryID)";
			var customerAddressQuery = "INSERT INTO CustomerAddress (AddressID, CustomerID) VALUES (@AddressID, @CustomerID)";

			var parameters = new DynamicParameters();
			parameters.Add("ID", request.Id, DbType.Guid, ParameterDirection.Input);
			parameters.Add("Street", request.Street, DbType.String, ParameterDirection.Input);
			parameters.Add("City", request.City, DbType.String, ParameterDirection.Input);
			parameters.Add("Region", request.Region, DbType.String, ParameterDirection.Input);
			parameters.Add("ZipCode", request.ZipCode, DbType.String, ParameterDirection.Input);
			parameters.Add("CountryID", request.CountryId, DbType.Int32, ParameterDirection.Input);

			var customerAddressParameters = new DynamicParameters();
			customerAddressParameters.Add("CustomerID", customer.Id, DbType.Guid, ParameterDirection.Input);
			customerAddressParameters.Add("AddressID", request.Id, DbType.Guid, ParameterDirection.Input);


			using (var connection = _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						await connection.ExecuteAsync(addressQuery, parameters, transaction);
						await connection.ExecuteAsync(customerAddressQuery, customerAddressParameters, transaction);
						transaction.Commit();

						return request;
					}
					catch (SqlException)
					{
						transaction.Rollback();

						return null;
					}

				}
			}
		}

		public async Task<Address> UpdateAsync(Guid oldAddressId, Address request, Guid oldCustomerId, Guid newCustomerId, CancellationToken ct)
		{
			using (var connection = _dbContext.CreateConnection())
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

						var deleteQuery = "DELETE FROM CustomerAddress WHERE AddressID = @AddressID AND CustomerID = @CustomerID";

						var paramsToDelete = new DynamicParameters();
						paramsToDelete.Add("AddressID", oldAddressId, DbType.Guid, ParameterDirection.Input);
						paramsToDelete.Add("CustomerID", oldCustomerId, DbType.Guid, ParameterDirection.Input);

						var query = "INSERT INTO CustomerAddress (CustomerID, AddressID) VALUES (@CustomerID, @AddressID)";

						var parameters = new DynamicParameters();
						parameters.Add("CustomerID", newCustomerId, DbType.Guid, ParameterDirection.Input);
						parameters.Add("AddressID", request.Id, DbType.Guid, ParameterDirection.Input);

						await connection.ExecuteAsync(deleteQuery, paramsToDelete, transaction);
						await connection.ExecuteAsync(query, parameters, transaction);
						transaction.Commit();

						return request;
					}
					catch (SqlException)
					{
						transaction.Rollback();

						return null;
					}
				}
			}
		}

		public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
		{
			var query = "DELETE FROM Address WHERE ID = @Id";

			using (var connection = _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						await connection.ExecuteAsync(query, new { id }, transaction);
						transaction.Commit();

						return true;
					}
					catch (SqlException)
					{
						transaction.Rollback();

						return false;
					}
				}
			}
		}
	}
}
