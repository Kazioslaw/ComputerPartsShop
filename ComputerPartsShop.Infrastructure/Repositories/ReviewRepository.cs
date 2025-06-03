using ComputerPartsShop.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ComputerPartsShop.Infrastructure
{
	public class ReviewRepository : IReviewRepository
	{
		private readonly DBContext _dbContext;

		public ReviewRepository(DBContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Review>> GetListAsync(CancellationToken ct)
		{
			var query = "SELECT Review.ID, Review.Rating, Review.Description, Customer.Username, Product.Name FROM Review " +
				"LEFT JOIN Customer ON Review.CustomerID = Customer.ID " +
				"LEFT JOIN Product ON Review.ProductID = Product.ID";

			using (var connection = await _dbContext.CreateConnection())
			{
				var result = await connection.QueryAsync<Review, Customer, Product, Review>(query, (review, customer, product) =>
				{
					review.Customer = customer;
					review.Product = product;
					return review;
				}, splitOn: "Username, Name");

				return result.ToList();
			}
		}

		public async Task<Review> GetAsync(int id, CancellationToken ct)
		{
			var query = "SELECT Review.ID, Customer.Username, Product.Name, Review.Rating, Review.Description FROM Review " +
				"LEFT JOIN Customer ON Review.CustomerID = Customer.ID " +
				"JOIN Product ON Review.ProductID = Product.ID WHERE Review.ID = @Id";

			using (var connection = await _dbContext.CreateConnection())
			{
				var result = await connection.QueryAsync<Review, Customer, Product, Review>(query, (review, customer, product) =>
				{
					review.Customer = customer;
					review.Product = product;
					return review;
				}, new { id }, splitOn: "Username, Name");

				return result.FirstOrDefault();
			}
		}

		public async Task<Review> CreateAsync(Review request, CancellationToken ct)
		{
			var query = "INSERT INTO Review (CustomerID, ProductID, Rating, Description) " +
				"VALUES (@CustomerID, @ProductID, @Rating, @Description); " +
				"SELECT CAST(SCOPE_IDENTITY() AS int)";

			var parameters = new DynamicParameters();
			parameters.Add("CustomerID", request.CustomerId, DbType.Guid, direction: ParameterDirection.Input);
			parameters.Add("ProductID", request.ProductId, DbType.Int32, ParameterDirection.Input);
			parameters.Add("Rating", request.Rating, DbType.Byte, ParameterDirection.Input);
			parameters.Add("Description", request.Description, DbType.String, direction: ParameterDirection.Input);
			parameters.Add("NewID", dbType: DbType.Int32, direction: ParameterDirection.Output);

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
					catch (SqlException)
					{
						transaction.Rollback();

						return null;
					}
				}
			}

		}

		public async Task<Review> UpdateAsync(int id, Review request, CancellationToken ct)
		{
			var query = "UPDATE Review SET CustomerID = @CustomerID, ProductID = @ProductID, Rating = @Rating, " +
				"Description = @Description WHERE ID = @Id";

			request.Id = id;

			var parameters = new DynamicParameters();
			parameters.Add("ID", request.Id, DbType.Int32, ParameterDirection.Input);
			parameters.Add("CustomerID", request.CustomerId, DbType.Guid, ParameterDirection.Input);
			parameters.Add("ProductID", request.ProductId, DbType.Int32, ParameterDirection.Input);
			parameters.Add("Rating", request.Rating, DbType.Byte, ParameterDirection.Input);
			parameters.Add("Description", request.Description, DbType.String, ParameterDirection.Input);

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
			var query = "DELETE FROM Review WHERE ID = @Id";

			using (var connection = await _dbContext.CreateConnection())
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
