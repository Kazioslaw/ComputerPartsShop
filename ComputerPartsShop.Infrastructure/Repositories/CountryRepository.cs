using ComputerPartsShop.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ComputerPartsShop.Infrastructure
{
	public class CountryRepository : ICountryRepository
	{
		private readonly DBContext _dbContext;

		public CountryRepository(DBContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Country>> GetListAsync(CancellationToken ct)
		{
			var query = "SELECT ID, Alpha2, Alpha3, Name FROM Country";

			using (var connection = await _dbContext.CreateConnection())
			{
				try
				{
					var result = await connection.QueryAsync<Country>(query);

					return result.ToList();
				}
				catch (SqlException ex)
				{
					Console.WriteLine(ex.Message);

					throw;
				}
			}
		}

		public async Task<Country> GetAsync(int id, CancellationToken ct)
		{
			var query = "SELECT Country.ID, Country.Alpha2, Country.Alpha3, Country.Name, Address.ID, Address.Street, Address.City, Address.Region, Address.ZipCode FROM Country " +
				"LEFT JOIN Address ON Address.CountryID = Country.ID WHERE Country.ID = @Id";
			using (var connection = await _dbContext.CreateConnection())
			{
				try
				{
					var countryDictionary = new Dictionary<int, Country>();

					var response = await connection.QueryAsync<Country, Address, Country>(query, (country, address) =>
					{
						Country currentCountry;
						if (!countryDictionary.TryGetValue(country.Id, out currentCountry))
						{
							currentCountry = country;
							currentCountry.Addresses = new List<Address>();
							countryDictionary.Add(currentCountry.Id, currentCountry);
						}

						if (address != null && address.Id != Guid.Empty && !currentCountry.Addresses.Any(a => a.Id == address.Id))
						{
							currentCountry.Addresses.Add(address);
						}

						return currentCountry;
					}, splitOn: "ID", param: new { id });

					return response.Distinct().FirstOrDefault();
				}
				catch (SqlException ex)
				{
					Console.WriteLine(ex.Message);

					throw;
				}

			}
		}

		public async Task<Country> GetByCountry3CodeAsync(string alpha3, CancellationToken ct)
		{
			var query = "SELECT ID, Alpha2, Alpha3, Name FROM Country WHERE Alpha3 = @Alpha3";

			var parameter = new DynamicParameters();
			parameter.Add("Alpha3", alpha3, DbType.String, ParameterDirection.Input);

			using (var connection = await _dbContext.CreateConnection())
			{
				try
				{
					var result = await connection.QueryFirstOrDefaultAsync<Country>(query, parameter);

					return result;
				}
				catch (SqlException ex)
				{
					Console.WriteLine(ex.Message);

					throw;
				}
			}
		}

		public async Task<Country> CreateAsync(Country request, CancellationToken ct)
		{
			var query = "INSERT INTO Country (Alpha2, Alpha3, Name) VALUES (@Alpha2, @Alpha3, @Name); " +
				"SELECT CAST(SCOPE_IDENTITY() AS int)";
			var parameters = new DynamicParameters();
			parameters.Add("@Alpha2", request.Alpha2, dbType: DbType.String, direction: ParameterDirection.Input);
			parameters.Add("@Alpha3", request.Alpha3, dbType: DbType.String, direction: ParameterDirection.Input);
			parameters.Add("@Name", request.Name, dbType: DbType.String, direction: ParameterDirection.Input);

			using (var connection = await _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						request.Id = await connection.QuerySingleAsync<int>(query, parameters, transaction);
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

		public async Task<Country> UpdateAsync(int id, Country request, CancellationToken ct)
		{
			var query = "UPDATE Country SET Alpha2 = @Alpha2, Alpha3 = @Alpha3, Name = @Name WHERE ID = @Id";
			request.Id = id;

			var parameters = new DynamicParameters();
			parameters.Add("Id", request.Id, DbType.Int32, ParameterDirection.Input);
			parameters.Add("Alpha2", request.Alpha2, DbType.String, ParameterDirection.Input);
			parameters.Add("Alpha3", request.Alpha3, DbType.String, ParameterDirection.Input);
			parameters.Add("Name", request.Name, DbType.String, ParameterDirection.Input);

			using (var connection = await _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
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

		public async Task DeleteAsync(int id, CancellationToken ct)
		{
			var query = "DELETE FROM Country WHERE ID = @Id";

			using (var connection = await _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						await connection.ExecuteAsync(query, new { id }, transaction);
						transaction.Commit();
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
