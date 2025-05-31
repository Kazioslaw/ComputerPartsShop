using ComputerPartsShop.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ComputerPartsShop.Infrastructure
{
	public class ProductRepository : IProductRepository
	{
		private readonly DBContext _dbContext;

		public ProductRepository(DBContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Product>> GetListAsync(CancellationToken ct)
		{
			var query = "SELECT Product.ID, Product.Name, Product.Description, Product.UnitPrice, Product.Stock, Product.InternalCode, Category.Name " +
				"FROM Product JOIN Category ON Product.CategoryID = Category.ID";
			using (var connection = _dbContext.CreateConnection())
			{
				var productList = await connection.QueryAsync<Product, Category, Product>(query, (product, category) =>
				{
					product.Category = category;
					return product;
				}, splitOn: "Name");

				return productList.ToList();
			}
		}

		public async Task<Product> GetAsync(int id, CancellationToken ct)
		{
			var query = "SELECT Product.ID, Product.Name, Product.Description, Product.UnitPrice, Product.Stock, Product.InternalCode, Category.Name " +
				"FROM Product JOIN Category ON Product.CategoryID = Category.ID WHERE Product.ID = @Id";
			using (var connection = _dbContext.CreateConnection())
			{
				var product = await connection.QueryAsync<Product, Category, Product>(query, (product, category) =>
				{
					product.Category = category;
					return product;
				}, new { Id = id }, splitOn: "Name");

				return product.FirstOrDefault();
			}
		}

		public async Task<Product> CreateAsync(Product request, CancellationToken ct)
		{
			var query = "INSERT INTO Product (Name, Description, UnitPrice, Stock, CategoryID, InternalCode) VALUES (@Name, @Description, @UnitPrice, @Stock, @CategoryID, @InternalCode); " +
				"SELECT CAST(SCOPE_IDENTITY() AS int)";

			var parameters = new DynamicParameters();
			parameters.Add("Name", request.Name, DbType.String, ParameterDirection.Input);
			parameters.Add("Description", request.Description, DbType.String, ParameterDirection.Input);
			parameters.Add("UnitPrice", request.UnitPrice, DbType.Decimal, ParameterDirection.Input);
			parameters.Add("Stock", request.Stock, DbType.Int32, ParameterDirection.Input);
			parameters.Add("CategoryID", request.CategoryId, DbType.Int32, ParameterDirection.Input);
			parameters.Add("InternalCode", request.InternalCode, DbType.String, ParameterDirection.Input);

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

		public async Task<Product> UpdateAsync(int id, Product request, CancellationToken ct)
		{

			var query = "UPDATE Product SET Name = @Name, Description = @Description, " +
				"UnitPrice = @UnitPrice, Stock = @Stock, CategoryID = @CategoryID, InternalCode = @InternalCode WHERE ID = @ID";
			var parameters = new DynamicParameters();
			parameters.Add("Name", request.Name, DbType.String, ParameterDirection.Input);
			parameters.Add("Description", request.Description, DbType.String, ParameterDirection.Input);
			parameters.Add("UnitPrice", request.UnitPrice, DbType.Decimal, ParameterDirection.Input);
			parameters.Add("Stock", request.Stock, DbType.Int32, ParameterDirection.Input);
			parameters.Add("CategoryID", request.CategoryId, DbType.Int32, ParameterDirection.Input);
			parameters.Add("InternalCode", request.InternalCode, DbType.String, ParameterDirection.Input);
			parameters.Add("ID", request.Id, DbType.String, ParameterDirection.Input);

			using (var connection = _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
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

		public async Task<bool> DeleteAsync(int id, CancellationToken ct)
		{
			var query = "DELETE FROM Product WHERE ID = @Id";

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
