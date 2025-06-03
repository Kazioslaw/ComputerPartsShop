using ComputerPartsShop.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ComputerPartsShop.Infrastructure
{
	public class CustomerPaymentSystemRepository : ICustomerPaymentSystemRepository
	{
		private readonly DBContext _dbContext;

		public CustomerPaymentSystemRepository(DBContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<CustomerPaymentSystem>> GetListAsync(CancellationToken ct)
		{
			var query = "SELECT CustomerPaymentSystem.ID, CustomerPaymentSystem.PaymentReference, Customer.Username, Customer.Email, PaymentProvider.Name FROM CustomerPaymentSystem " +
				"LEFT JOIN Customer ON Customer.ID = CustomerPaymentSystem.CustomerID " +
				"LEFT JOIN PaymentProvider ON PaymentProvider.ID = CustomerPaymentSystem.ProviderID";

			using (var connection = await _dbContext.CreateConnection())
			{
				var result = await connection.QueryAsync<CustomerPaymentSystem, Customer, PaymentProvider, CustomerPaymentSystem>(query,
				(customerPaymentSystem, customer, paymentProvider) =>
				{
					customerPaymentSystem.Customer = customer;
					customerPaymentSystem.Provider = paymentProvider;

					return customerPaymentSystem;
				}, splitOn: "Username, Name");

				return result.ToList();
			}
		}

		public async Task<CustomerPaymentSystem> GetAsync(Guid id, CancellationToken ct)
		{
			var query = "SELECT CustomerPaymentSystem.ID, CustomerPaymentSystem.PaymentReference, Customer.Username, Customer.Email, PaymentProvider.Name, Payment.ID, Payment.OrderID, " +
				"Payment.Total, Payment.Method, Payment.Status, Payment.PaymentStartAt, Payment.PaidAt FROM CustomerPaymentSystem " +
				"LEFT JOIN Customer ON Customer.ID = CustomerPaymentSystem.CustomerID " +
				"LEFT JOIN PaymentProvider ON PaymentProvider.ID = CustomerPaymentSystem.ProviderID " +
				"LEFT JOIN Payment ON Payment.CustomerPaymentSystemID = CustomerPaymentSystem.ID " +
				"WHERE CustomerPaymentSystem.ID = @ID";

			var customerPaymentSystemDictionary = new Dictionary<Guid, CustomerPaymentSystem>();

			using (var connection = await _dbContext.CreateConnection())
			{
				var result = await connection.QueryAsync<CustomerPaymentSystem, Customer, PaymentProvider, Payment, CustomerPaymentSystem>(query, (customerPaymentSystem, customer,
					paymentProvider, payment) =>
				{
					if (!customerPaymentSystemDictionary.TryGetValue(customerPaymentSystem.Id, out var cps))
					{
						cps = customerPaymentSystem;
						cps.Customer = customer;
						cps.Provider = paymentProvider;
						cps.Payments = new List<Payment>();
						customerPaymentSystemDictionary.Add(cps.Id, cps);
					}

					if (payment != null && payment.Id != Guid.Empty)
					{
						cps.Payments.Add(payment);
					}

					return cps;
				}, new { ID = id }, splitOn: "Username, Name, ID");

				return result.Distinct().FirstOrDefault();
			}
		}

		public async Task<CustomerPaymentSystem> CreateAsync(CustomerPaymentSystem request, CancellationToken ct)
		{
			request.Id = Guid.NewGuid();
			var query = "INSERT INTO CustomerPaymentSystem (ID, CustomerID, ProviderID, PaymentReference) VALUES (@ID, @CustomerID, @ProviderID, @PaymentReference)";

			var parameters = new DynamicParameters();
			parameters.Add("ID", request.Id, DbType.Guid, ParameterDirection.Input);
			parameters.Add("CustomerID", request.CustomerId, DbType.Guid, ParameterDirection.Input);
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
					catch (SqlException)
					{
						transaction.Rollback();

						return null;
					}
				}
			}
		}

		public async Task<CustomerPaymentSystem> UpdateAsync(Guid id, CustomerPaymentSystem request, CancellationToken ct)
		{
			request.Id = id;
			var query = "UPDATE CustomerPaymentSystem SET CustomerID = @CustomerID, ProviderID = @ProviderID, PaymentReference = @PaymentReference WHERE ID = @ID";

			var parameters = new DynamicParameters();
			parameters.Add("ID", request.Id, DbType.Guid, ParameterDirection.Input);
			parameters.Add("CustomerID", request.CustomerId, DbType.Guid, ParameterDirection.Input);
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
			var query = "DELETE FROM CustomerPaymentSystem WHERE ID = @Id";

			using (var connection = await _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						await connection.ExecuteAsync(query, new { ID = id }, transaction);
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
