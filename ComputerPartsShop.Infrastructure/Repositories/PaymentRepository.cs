using ComputerPartsShop.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ComputerPartsShop.Infrastructure
{
	public class PaymentRepository : IPaymentRepository
	{
		private readonly DBContext _dbContext;

		public PaymentRepository(DBContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Payment>> GetListAsync(CancellationToken ct)
		{
			var query = "SELECT Payment.ID, Payment.CustomerPaymentID, Payment.OrderID, Payment.Total, Payment.Method, Payment.Status, Payment.PaymentStartAt, Payment.PaidAt FROM Payment";
			using (var connection = _dbContext.CreateConnection())
			{
				var paymentList = await connection.QueryAsync<Payment>(query);
				return paymentList.ToList();
			}
		}

		public async Task<Payment> GetAsync(int id, CancellationToken ct)
		{
			var query = "SELECT Payment.ID, CustomerPaymentSystem.ID, CustomerPaymentSystem.Username, CustomerPaymentSystem.Email," +
				"CustomerPaymentSystem.ProviderName, CustomerPaymentSystem.PaymentReference, " +
				"Order.Id, Order.Username, Order.Email, ";

			return new Payment();
		}

		public async Task<Payment> CreateAsync(Payment request, CancellationToken ct)
		{
			request.Id = 999;
			return request;
		}

		public async Task<Payment> UpdateAsync(int id, Payment request, CancellationToken ct)
		{
			request.Id = id;
			return request;
		}

		public async Task<bool> DeleteAsync(int id, CancellationToken ct)
		{
			var query = "DELETE FROM Payment WHERE ID = @Id";

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
