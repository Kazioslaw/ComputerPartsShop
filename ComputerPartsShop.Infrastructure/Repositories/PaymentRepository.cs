using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class PaymentRepository : IPaymentRepository
	{
		private readonly TempData _dbContext;

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
			var payment = _dbContext.PaymentList.FirstOrDefault(x => x.Id == id);

			return payment!;
		}

		public async Task<int> CreateAsync(Payment request, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var last = _dbContext.PaymentList.OrderBy(x => x.Id).FirstOrDefault();

			if (last == null)
			{
				request.Id = 1;
			}
			else
			{
				request.Id = last.Id + 1;
			}

			_dbContext.PaymentList.Add(request);

			return request.Id;
		}

		public async Task<Payment> UpdateAsync(int id, Payment request, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var payment = _dbContext.PaymentList.FirstOrDefault(x => x.Id == id);

			if (payment != null)
			{
				payment.CustomerPaymentSystemId = request.CustomerPaymentSystemId;
				payment.OrderId = request.OrderId;
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
			var payment = _dbContext.PaymentList.FirstOrDefault(x => x.Id == id);

			if (payment != null)
			{
				_dbContext.PaymentList.Remove(payment);
			}
		}
	}
}
