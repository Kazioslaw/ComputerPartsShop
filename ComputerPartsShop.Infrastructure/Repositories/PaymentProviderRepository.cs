using ComputerPartsShop.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ComputerPartsShop.Infrastructure
{
	public class PaymentProviderRepository : IPaymentProviderRepository
	{
		private readonly DBContext _dbContext;

		public PaymentProviderRepository(DBContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<PaymentProvider>> GetListAsync(CancellationToken ct)
		{
			var query = "SELECT PaymentProvider.ID, Name, UserPaymentSystem.ID FROM PaymentProvider " +
				"LEFT JOIN UserPaymentSystem ON UserPaymentSystem.ProviderID = PaymentProvider.ID";

			var providerDictionary = new Dictionary<int, PaymentProvider>();

			using (var connection = await _dbContext.CreateConnection())
			{
				try
				{
					var result = await connection.QueryAsync<PaymentProvider, UserPaymentSystem, PaymentProvider>(query, (paymentProvider, userPaymentSystem) =>
					{
						if (!providerDictionary.TryGetValue(paymentProvider.Id, out var pp))
						{
							pp = paymentProvider;
							pp.UserPayments = new List<UserPaymentSystem>();
							providerDictionary.Add(pp.Id, pp);
						}

						if (userPaymentSystem != null)
						{
							pp.UserPayments.Add(userPaymentSystem);
						}

						return pp;
					}, splitOn: "ID");

					return result.Distinct().ToList();
				}
				catch (SqlException ex)
				{
					Console.WriteLine(ex.Message);

					throw;
				}
			}
		}

		public async Task<PaymentProvider> GetAsync(int id, CancellationToken ct)
		{
			var query = "SELECT PaymentProvider.ID, PaymentProvider.Name, UserPaymentSystem.ID, UserPaymentSystem.PaymentReference, ShopUser.Username, ShopUser.Email FROM PaymentProvider " +
				"LEFT JOIN UserPaymentSystem ON UserPaymentSystem.ProviderID = PaymentProvider.ID " +
				"LEFT JOIN ShopUser ON UserPaymentSystem.UserID = ShopUser.ID WHERE PaymentProvider.ID = @Id";

			using (var connection = await _dbContext.CreateConnection())
			{
				try
				{
					var providerDictionary = new Dictionary<int, PaymentProvider>();
					var result = await connection.QueryAsync<PaymentProvider, UserPaymentSystem, ShopUser, PaymentProvider>(query, (paymentProvider, userPaymentSystem, user) =>
					{
						if (!providerDictionary.TryGetValue(paymentProvider.Id, out var pp))
						{
							pp = paymentProvider;
							pp.UserPayments = new List<UserPaymentSystem>();
							providerDictionary.Add(pp.Id, pp);
						}

						if (userPaymentSystem != null)
						{
							userPaymentSystem.User = user;
							pp.UserPayments.Add(userPaymentSystem);
						}

						return pp;
					}, new { Id = id }, splitOn: "ID, Username");

					return result.Distinct().FirstOrDefault();
				}
				catch (SqlException ex)
				{
					Console.WriteLine(ex.Message);

					throw;
				}
			}
		}

		public async Task<PaymentProvider> GetByNameAsync(string input, CancellationToken ct)
		{
			var query = "SELECT PaymentProvider.ID, PaymentProvider.Name, UserPaymentSystem.ID, UserPaymentSystem.PaymentReference, ShopUser.Username, ShopUser.Email FROM PaymentProvider " +
				"LEFT JOIN UserPaymentSystem ON UserPaymentSystem.ProviderID = PaymentProvider.ID " +
				"LEFT JOIN ShopUser ON UserPaymentSystem.UserID = ShopUser.ID WHERE PaymentProvider.Name = @Input";

			using (var connection = await _dbContext.CreateConnection())
			{
				try
				{
					var providerDictionary = new Dictionary<int, PaymentProvider>();
					var result = await connection.QueryAsync<PaymentProvider, UserPaymentSystem, ShopUser, PaymentProvider>(query, (paymentProvider, userPaymentSystem, user) =>
					{
						if (!providerDictionary.TryGetValue(paymentProvider.Id, out var currentPaymentProvider))
						{
							currentPaymentProvider = paymentProvider;
							currentPaymentProvider.UserPayments = new List<UserPaymentSystem>();
							providerDictionary.Add(currentPaymentProvider.Id, currentPaymentProvider);
						}

						if (userPaymentSystem != null)
						{
							userPaymentSystem.User = user;
							currentPaymentProvider.UserPayments.Add(userPaymentSystem);
						}

						return currentPaymentProvider;
					}, new { Input = input }, splitOn: "ID, Username");

					return result.Distinct().FirstOrDefault();
				}
				catch (SqlException ex)
				{
					Console.WriteLine(ex.Message);

					throw;
				}
			}
		}

		public async Task<PaymentProvider> CreateAsync(PaymentProvider request, CancellationToken ct)
		{
			var query = "INSERT INTO PaymentProvider (Name) VALUES (@Name); SELECT CAST(SCOPE_IDENTITY() AS int)";

			var parameters = new DynamicParameters();
			parameters.Add("Name", request.Name, DbType.String, ParameterDirection.Input);

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

		public async Task<PaymentProvider> UpdateAsync(int id, PaymentProvider request, CancellationToken ct)
		{
			var query = "UPDATE PaymentProvider SET Name = @Name WHERE ID = @Id";

			var parameters = new DynamicParameters();
			parameters.Add("Name", request.Name, DbType.String, ParameterDirection.Input);
			parameters.Add("Id", request.Id, DbType.Int32, ParameterDirection.Input);

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
			var query = "DELETE FROM PaymentProvider WHERE ID = @Id";

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
