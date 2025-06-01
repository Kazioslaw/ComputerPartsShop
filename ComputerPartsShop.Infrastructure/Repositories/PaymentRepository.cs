using ComputerPartsShop.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

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
			var query = "SELECT Payment.ID, Payment.CustomerPaymentSystemID, Payment.OrderID, Payment.Total, Payment.Method, Payment.Status, Payment.PaymentStartAt, Payment.PaidAt FROM Payment";
			using (var connection = await _dbContext.CreateConnection())
			{
				var paymentList = await connection.QueryAsync<Payment>(query);
				return paymentList.ToList();
			}
		}

		public async Task<Payment> GetAsync(Guid id, CancellationToken ct)
		{
			var query = "SELECT Payment.ID, Payment.CustomerPaymentSystemID, Payment.OrderID, Payment.Total, Payment.Method, Payment.Status, Payment.PaymentStartAt, Payment.PaidAt " +
				"FROM Payment WHERE Payment.ID = @ID";

			using (var connection = await _dbContext.CreateConnection())
			{
				var payment = await connection.QueryFirstOrDefaultAsync<Payment>(query, new { ID = id });

				return payment;
			}
		}

		public async Task<Payment> CreateAsync(Payment request, CancellationToken ct)
		{
			var query = "INSERT INTO Payment (ID,CustomerPaymentSystemID, OrderID, Total, Method, Status, PaymentStartAt, PaidAt) " +
				"VALUES (@ID,@CustomerPaymentSystemID, @OrderID, @Total, @Method, @Status, @PaymentStartAt, @PaidAt) " +
				"SELECT CAST(SCOPE_IDENTITY() AS int)";

			request.Id = Guid.NewGuid();

			var parameters = new DynamicParameters();
			parameters.Add("ID", request.Id, DbType.Guid, ParameterDirection.Input);
			parameters.Add("CustomerPaymentSystemID", request.CustomerPaymentSystemId, DbType.Guid, ParameterDirection.Input);
			parameters.Add("OrderID", request.OrderId, DbType.Int32, ParameterDirection.Input);
			parameters.Add("Total", request.Total, DbType.Decimal, ParameterDirection.Input);
			parameters.Add("Method", request.Method.ToString(), DbType.String, ParameterDirection.Input);
			parameters.Add("Status", request.Status.ToString(), DbType.String, ParameterDirection.Input);
			parameters.Add("PaymentStartAt", request.PaymentStartAt, DbType.DateTime2, ParameterDirection.Input);
			parameters.Add("PaidAt", request.PaidAt, DbType.DateTime2, ParameterDirection.Input);

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

		public async Task<Payment> UpdateStatusAsync(Guid id, Payment request, CancellationToken ct)
		{
			var query = "UPDATE Payment SET Payment.Status = @Status, Payment.PaidAt = @PaidAt WHERE Payment.ID = @ID";

			var parameters = new DynamicParameters();
			parameters.Add("ID", id, DbType.Guid, ParameterDirection.Input);
			parameters.Add("Status", request.Status.ToString(), DbType.String, ParameterDirection.Input);
			parameters.Add("PaidAt", request.PaidAt, DbType.DateTime2, ParameterDirection.Input);

			using (var connection = await _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						await connection.ExecuteAsync(query, parameters, transaction);

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

		public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
		{
			var query = "DELETE FROM Payment WHERE ID = @Id";

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
