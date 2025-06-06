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
			var query = "SELECT Review.ID, Review.Rating, Review.Description, ShopUser.Username, Product.Name FROM Review " +
				"LEFT JOIN ShopUser ON Review.UserID = ShopUser.ID " +
				"LEFT JOIN Product ON Review.ProductID = Product.ID";

			using (var connection = await _dbContext.CreateConnection())
			{
				try
				{
					var result = await connection.QueryAsync<Review, ShopUser, Product, Review>(query, (review, customer, product) =>
					{
						review.User = customer;
						review.Product = product;
						return review;
					}, splitOn: "Username, Name");

					return result.ToList();
				}
				catch (SqlException ex)
				{
					Console.WriteLine(ex.Message);

					throw;
				}
			}
		}

		public async Task<Review> GetAsync(int id, CancellationToken ct)
		{
			var query = "SELECT Review.ID, Review.Rating, Review.Description, ShopUser.Username, Product.Name  FROM Review " +
				"LEFT JOIN ShopUser ON Review.UserID = ShopUser.ID " +
				"JOIN Product ON Review.ProductID = Product.ID WHERE Review.ID = @Id";

			using (var connection = await _dbContext.CreateConnection())
			{

				try
				{
					var result = await connection.QueryAsync<Review, ShopUser, Product, Review>(query, (review, customer, product) =>
					{
						review.User = customer;
						review.Product = product;
						return review;
					}, new { id }, splitOn: "Username, Name");

					return result.FirstOrDefault();
				}
				catch (SqlException ex)
				{
					Console.WriteLine(ex.Message);

					throw;
				}
			}
		}

		public async Task<Review> CreateAsync(Review request, CancellationToken ct)
		{
			var query = "INSERT INTO Review (UserID, ProductID, Rating, Description) " +
				"VALUES (@UserID, @ProductID, @Rating, @Description); " +
				"SELECT CAST(SCOPE_IDENTITY() AS int)";

			var parameters = new DynamicParameters();
			parameters.Add("UserID", request.UserId, DbType.Guid, direction: ParameterDirection.Input);
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
					catch (SqlException ex)
					{
						transaction.Rollback();
						Console.WriteLine(ex.Message);

						throw;
					}
				}
			}

		}

		public async Task<int> UpdateAsync(int id, Review request, CancellationToken ct)
		{
			var query = "UPDATE Review SET ProductID = @ProductID, Rating = @Rating, " +
				"Description = @Description WHERE ID = @Id AND UserID = @UserID";

			request.Id = id;

			var parameters = new DynamicParameters();
			parameters.Add("ID", request.Id, DbType.Int32, ParameterDirection.Input);
			parameters.Add("UserID", request.UserId, DbType.Guid, ParameterDirection.Input);
			parameters.Add("ProductID", request.ProductId, DbType.Int32, ParameterDirection.Input);
			parameters.Add("Rating", request.Rating, DbType.Byte, ParameterDirection.Input);
			parameters.Add("Description", request.Description, DbType.String, ParameterDirection.Input);

			using (var connection = await _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						var updatedRows = await connection.ExecuteAsync(query, parameters, transaction);

						transaction.Commit();

						return updatedRows;
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
			var query = "DELETE FROM Review WHERE ID = @Id";

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
