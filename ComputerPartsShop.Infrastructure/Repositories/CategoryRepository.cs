using ComputerPartsShop.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ComputerPartsShop.Infrastructure
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly DBContext _dbContext;
		public CategoryRepository(DBContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Category>> GetListAsync(CancellationToken ct)
		{
			var query = "SELECT ID, Name, Description FROM Category";

			using (var connection = _dbContext.CreateConnection())
			{
				var result = await connection.QueryAsync<Category>(query);

				return result.ToList();
			}
		}

		public async Task<Category> GetAsync(int id, CancellationToken ct)
		{
			var query = "SELECT ID, Name, Description FROM Category WHERE ID = @Id";

			using (var connection = _dbContext.CreateConnection())
			{
				var result = await connection.QueryFirstOrDefaultAsync<Category>(query, new { id });

				return result;
			}
		}

		public async Task<Category> GetByNameAsync(string name, CancellationToken ct)
		{
			var query = "SELECT ID, Name, Description FROM Category WHERE Name = @Name";

			using (var connection = _dbContext.CreateConnection())
			{
				var result = await connection.QueryFirstOrDefaultAsync<Category>(query, new { name });

				return result;
			}
		}

		public async Task<Category> CreateAsync(Category request, CancellationToken ct)
		{
			var query = "INSERT INTO Category (Name, Description) VALUES (@Name, @Description); " +
						"SELECT CAST(SCOPE_IDENTITY() AS int)";

			var parameters = new DynamicParameters();
			parameters.Add("Name", request.Name, dbType: DbType.String, direction: ParameterDirection.Input);
			parameters.Add("Description", request.Description, dbType: DbType.String, direction: ParameterDirection.Input);

			using (var connection = _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						request.Id = await connection.QuerySingleAsync<int>(query, parameters, transaction);
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

		public async Task<Category> UpdateAsync(int id, Category request, CancellationToken ct)
		{
			var query = "UPDATE Category SET Name = @Name, Description = @Description WHERE ID = @Id";

			var parameters = new DynamicParameters();
			parameters.Add("Id", id, dbType: DbType.Int32, direction: ParameterDirection.Input);
			parameters.Add("Name", request.Name, dbType: DbType.String, direction: ParameterDirection.Input);
			parameters.Add("Description", request.Description, dbType: DbType.String, direction: ParameterDirection.Input);

			using (var connection = _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						await connection.ExecuteAsync(query, parameters, transaction: transaction);
						transaction.Commit();
						request.Id = id;

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

		public async Task<bool> DeleteAsync(int id, CancellationToken ct)
		{
			var query = "DELETE FROM Category WHERE ID = @Id";

			using (var connection = _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{

					try
					{
						await connection.ExecuteAsync(query, new { id }, transaction: transaction);
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
