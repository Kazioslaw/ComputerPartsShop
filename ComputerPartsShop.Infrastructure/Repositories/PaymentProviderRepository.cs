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
			var query = "SELECT PaymentProvider.ID, Name, CustomerPaymentSystem.ID FROM PaymentProvider " +
				"LEFT JOIN CustomerPaymentSystem ON CustomerPaymentSystem.ProviderID = PaymentProvider.ID";

			var providerDictionary = new Dictionary<int, PaymentProvider>();

			using (var connection = await _dbContext.CreateConnection())
			{
				var result = await connection.QueryAsync<PaymentProvider, CustomerPaymentSystem, PaymentProvider>(query, (paymentProvider, customerPaymentSystem) =>
				{
					if (!providerDictionary.TryGetValue(paymentProvider.Id, out var pp))
					{
						pp = paymentProvider;
						pp.CustomerPayments = new List<CustomerPaymentSystem>();
						providerDictionary.Add(pp.Id, pp);
					}

					if (customerPaymentSystem != null)
					{
						pp.CustomerPayments.Add(customerPaymentSystem);
					}

					return pp;
				}, splitOn: "ID");

				return result.Distinct().ToList();
			}
		}

		public async Task<PaymentProvider> GetAsync(int id, CancellationToken ct)
		{
			var query = "SELECT PaymentProvider.ID, PaymentProvider.Name, CustomerPaymentSystem.ID, CustomerPaymentSystem.PaymentReference, Customer.Username, Customer.Email FROM PaymentProvider " +
				"LEFT JOIN CustomerPaymentSystem ON CustomerPaymentSystem.ProviderID = PaymentProvider.ID " +
				"LEFT JOIN Customer ON CustomerPaymentSystem.CustomerID = Customer.ID WHERE PaymentProvider.ID = @Id";

			using (var connection = await _dbContext.CreateConnection())
			{
				var providerDictionary = new Dictionary<int, PaymentProvider>();
				var result = await connection.QueryAsync<PaymentProvider, CustomerPaymentSystem, Customer, PaymentProvider>(query, (paymentProvider, customerPaymentSystem, customer) =>
				{
					if (!providerDictionary.TryGetValue(paymentProvider.Id, out var pp))
					{
						pp = paymentProvider;
						pp.CustomerPayments = new List<CustomerPaymentSystem>();
						providerDictionary.Add(pp.Id, pp);
					}

					if (customerPaymentSystem != null)
					{
						customerPaymentSystem.Customer = customer;
						pp.CustomerPayments.Add(customerPaymentSystem);
					}

					return pp;
				}, new { Id = id }, splitOn: "ID, Username");

				return result.Distinct().FirstOrDefault();
			}
		}

		public async Task<PaymentProvider> GetByNameAsync(string input, CancellationToken ct)
		{
			var query = "SELECT PaymentProvider.ID, PaymentProvider.Name, CustomerPaymentSystem.ID, CustomerPaymentSystem.PaymentReference, Customer.Username, Customer.Email FROM PaymentProvider " +
				"LEFT JOIN CustomerPaymentSystem ON CustomerPaymentSystem.ProviderID = PaymentProvider.ID " +
				"LEFT JOIN Customer ON CustomerPaymentSystem.CustomerID = Customer.ID WHERE PaymentProvider.Name = @Input";

			using (var connection = await _dbContext.CreateConnection())
			{
				var providerDictionary = new Dictionary<int, PaymentProvider>();
				var result = await connection.QueryAsync<PaymentProvider, CustomerPaymentSystem, Customer, PaymentProvider>(query, (paymentProvider, customerPaymentSystem, customer) =>
				{
					if (!providerDictionary.TryGetValue(paymentProvider.Id, out var pp))
					{
						pp = paymentProvider;
						pp.CustomerPayments = new List<CustomerPaymentSystem>();
						providerDictionary.Add(pp.Id, pp);
					}

					if (customerPaymentSystem != null)
					{
						customerPaymentSystem.Customer = customer;
						pp.CustomerPayments.Add(customerPaymentSystem);
					}

					return pp;
				}, new { Input = input }, splitOn: "ID, Username");

				return result.Distinct().FirstOrDefault();
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
					catch (SqlException)
					{
						transaction.Rollback();

						return null;
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
			var query = "DELETE FROM PaymentProvider WHERE ID = @Id";

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
