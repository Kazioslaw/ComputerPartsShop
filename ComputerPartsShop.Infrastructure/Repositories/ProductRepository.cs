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
			using (var connection = await _dbContext.CreateConnection())
			{

				try
				{
					var productList = await connection.QueryAsync<Product, Category, Product>(query, (product, category) =>
					{
						product.Category = category;
						return product;
					}, splitOn: "Name");

					return productList.ToList();
				}
				catch (SqlException ex)
				{
					Console.WriteLine(ex.Message);

					throw;
				}
			}
		}

		public async Task<Product> GetAsync(int id, CancellationToken ct)
		{
			var query = "SELECT Product.ID, Product.Name, Product.Description, Product.UnitPrice, Product.Stock, Product.InternalCode, Category.Name " +
				"FROM Product JOIN Category ON Product.CategoryID = Category.ID WHERE Product.ID = @Id";
			using (var connection = await _dbContext.CreateConnection())
			{
				try
				{
					var product = await connection.QueryAsync<Product, Category, Product>(query, (product, category) =>
					{
						product.Category = category;
						return product;
					}, new { Id = id }, splitOn: "Name");

					return product.FirstOrDefault();
				}
				catch (SqlException ex)
				{
					Console.WriteLine(ex.Message);

					throw;
				}
			}
		}

		public async Task<Product> CreateAsync(Product product, CancellationToken ct)
		{
			var query = "INSERT INTO Product (Name, Description, UnitPrice, Stock, CategoryID, InternalCode) VALUES (@Name, @Description, @UnitPrice, @Stock, @CategoryID, @InternalCode); " +
				"SELECT CAST(SCOPE_IDENTITY() AS int)";

			var parameters = new DynamicParameters();
			parameters.Add("Name", product.Name, DbType.String, ParameterDirection.Input);
			parameters.Add("Description", product.Description, DbType.String, ParameterDirection.Input);
			parameters.Add("UnitPrice", product.UnitPrice, DbType.Decimal, ParameterDirection.Input);
			parameters.Add("Stock", product.Stock, DbType.Int32, ParameterDirection.Input);
			parameters.Add("CategoryID", product.CategoryId, DbType.Int32, ParameterDirection.Input);
			parameters.Add("InternalCode", product.InternalCode, DbType.String, ParameterDirection.Input);

			using (var connection = await _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						product.Id = await connection.QuerySingleAsync<int>(query, parameters, transaction);
						transaction.Commit();

						return product;
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
			var productQuery = "DELETE FROM Product WHERE ID = @Id";

			using (var connection = await _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						await connection.ExecuteAsync(productQuery, new { id }, transaction);
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
