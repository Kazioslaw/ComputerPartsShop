using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class PaymentRepository : IPaymentRepository
	{
		readonly TempData _dbContext;

		public PaymentRepository(TempData dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Payment>> GetListAsync(CancellationToken ct)
		{
			await Task.Delay(500, ct);
			return _dbContext.PaymentList;
		}

		public async Task<Payment> GetAsync(int id, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var payment = _dbContext.PaymentList.FirstOrDefault(x => x.ID == id);

			return payment!;
		}

		public async Task<int> CreateAsync(Payment request, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var last = _dbContext.PaymentList.OrderBy(x => x.ID).FirstOrDefault();

			if (last == null)
			{
				request.ID = 1;
			}
			else
			{
				request.ID = last.ID + 1;
			}

			_dbContext.PaymentList.Add(request);

			return request.ID;
		}

		public async Task<Payment> UpdateAsync(int id, Payment request, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var payment = _dbContext.PaymentList.FirstOrDefault(x => x.ID == id);

			if (payment != null)
			{
				payment.CustomerPaymentSystemID = request.CustomerPaymentSystemID;
				payment.OrderID = request.OrderID;
				payment.Total = request.Total;
				payment.Method = request.Method;
				payment.Status = request.Status;
				payment.PaymentStartAt = request.PaymentStartAt;
				payment.PaidAt = request.PaidAt;
			}

			return payment!;
		}

		public async Task DeleteAsync(int id, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var payment = _dbContext.PaymentList.FirstOrDefault(x => x.ID == id);

			if (payment != null)
			{
				_dbContext.PaymentList.Remove(payment);
			}
		}
	}
}
