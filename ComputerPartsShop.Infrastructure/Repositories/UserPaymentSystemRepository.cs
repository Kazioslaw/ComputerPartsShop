using ComputerPartsShop.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ComputerPartsShop.Infrastructure
{
	public class UserPaymentSystemRepository : IUserPaymentSystemRepository
	{
		private readonly DBContext _dbContext;

		public UserPaymentSystemRepository(DBContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<UserPaymentSystem>> GetListAsync(CancellationToken ct)
		{
			var query = "SELECT UserPaymentSystem.ID, UserPaymentSystem.PaymentReference, ShopUser.Username, ShopUser.Email, PaymentProvider.Name FROM UserPaymentSystem " +
				"LEFT JOIN ShopUser ON ShopUser.ID = UserPaymentSystem.UserID " +
				"LEFT JOIN PaymentProvider ON PaymentProvider.ID = UserPaymentSystem.ProviderID";

			using (var connection = await _dbContext.CreateConnection())
			{
				try
				{
					var result = await connection.QueryAsync<UserPaymentSystem, ShopUser, PaymentProvider, UserPaymentSystem>(query,
					(userPaymentSystem, user, paymentProvider) =>
					{
						userPaymentSystem.User = user;
						userPaymentSystem.Provider = paymentProvider;

						return userPaymentSystem;
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

		public async Task<UserPaymentSystem> GetAsync(Guid id, string username, CancellationToken ct)
		{
			var query = "SELECT UserPaymentSystem.ID, UserPaymentSystem.PaymentReference, ShopUser.Username, ShopUser.Email, PaymentProvider.Name, Payment.ID, Payment.OrderID, " +
				"Payment.Total, Payment.Method, Payment.Status, Payment.PaymentStartAt, Payment.PaidAt FROM UserPaymentSystem " +
				"LEFT JOIN ShopUser ON ShopUser.ID = UserPaymentSystem.UserID " +
				"LEFT JOIN PaymentProvider ON PaymentProvider.ID = UserPaymentSystem.ProviderID " +
				"LEFT JOIN Payment ON Payment.UserPaymentSystemID = UserPaymentSystem.ID " +
				"WHERE UserPaymentSystem.ID = @ID AND ShopUser.Username = @Username";

			var userPaymentSystemDictionary = new Dictionary<Guid, UserPaymentSystem>();

			using (var connection = await _dbContext.CreateConnection())
			{
				try
				{
					var result = await connection.QueryAsync<UserPaymentSystem, ShopUser, PaymentProvider, Payment, UserPaymentSystem>(query, (userPaymentSystem, user,
						paymentProvider, payment) =>
					{
						if (!userPaymentSystemDictionary.TryGetValue(userPaymentSystem.Id, out var cps))
						{
							cps = userPaymentSystem;
							cps.User = user;
							cps.Provider = paymentProvider;
							cps.Payments = new List<Payment>();
							userPaymentSystemDictionary.Add(cps.Id, cps);
						}

						if (payment != null && payment.Id != Guid.Empty)
						{
							cps.Payments.Add(payment);
						}

						return cps;
					}, new { ID = id, Username = username }, splitOn: "Username, Name, ID");

					return result.Distinct().FirstOrDefault();
				}
				catch (SqlException ex)
				{
					Console.WriteLine(ex.Message);

					throw;
				}
			}
		}

		public async Task<UserPaymentSystem> CreateAsync(UserPaymentSystem request, CancellationToken ct)
		{
			request.Id = Guid.NewGuid();
			var query = "INSERT INTO UserPaymentSystem (ID, UserID, ProviderID, PaymentReference) VALUES (@ID, @UserID, @ProviderID, @PaymentReference)";

			var parameters = new DynamicParameters();
			parameters.Add("ID", request.Id, DbType.Guid, ParameterDirection.Input);
			parameters.Add("UserID", request.UserId, DbType.Guid, ParameterDirection.Input);
			parameters.Add("ProviderID", request.ProviderId, DbType.Int32, ParameterDirection.Input);
			parameters.Add("PaymentReference", request.PaymentReference, DbType.String, ParameterDirection.Input);

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

		public async Task<int> UpdateAsync(Guid id, string username, UserPaymentSystem request, CancellationToken ct)
		{
			request.Id = id;
			var query = "UPDATE UserPaymentSystem SET UserID = @UserID, ProviderID = @ProviderID, PaymentReference = @PaymentReference " +
				"JOIN ShopUser ON ShopUser.ID = UserPaymentSystem.UserID WHERE UserPaymentSystem.ID = @ID AND Username = @Username";

			var parameters = new DynamicParameters();
			parameters.Add("ID", request.Id, DbType.Guid, ParameterDirection.Input);
			parameters.Add("UserID", request.UserId, DbType.Guid, ParameterDirection.Input);
			parameters.Add("ProviderID", request.ProviderId, DbType.Int32, ParameterDirection.Input);
			parameters.Add("PaymentReference", request.PaymentReference, DbType.String, ParameterDirection.Input);
			parameters.Add("Username", username, DbType.String, ParameterDirection.Input);

			using (var connection = await _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						var rowsAffected = await connection.ExecuteAsync(query, parameters, transaction);
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

		public async Task<bool> DeleteAsync(Guid id, string username, CancellationToken ct)
		{
			var query = "DELETE FROM UserPaymentSystem JOIN ShopUser ON ShopUser.ID = UserPaymentSystem.UserID WHERE ID = @Id AND ShopUser.Username = @Username";

			using (var connection = await _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						int rowsAffected = await connection.ExecuteAsync(query, new { ID = id }, transaction);
						transaction.Commit();

						return rowsAffected > 0;
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
