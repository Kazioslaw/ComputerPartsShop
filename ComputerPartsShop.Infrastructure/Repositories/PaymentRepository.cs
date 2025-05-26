using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class PaymentRepository : IRepository<Payment, int>
	{
		readonly TempData _dbContext;

		public PaymentRepository(TempData dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Payment>> GetList()
		{
			return _dbContext.PaymentList;
		}

		public async Task<Payment> Get(int id)
		{
			var payment = _dbContext.PaymentList.FirstOrDefault(x => x.ID == id);

			return payment;
		}

		public async Task<int> Create(Payment request)
		{
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

		public async Task<Payment> Update(int id, Payment request)
		{
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

			return payment;
		}

		public async Task Delete(int id)
		{
			var payment = _dbContext.PaymentList.FirstOrDefault(x => x.ID == id);

			if (payment != null)
			{
				_dbContext.PaymentList.Remove(payment);
			}
		}
	}
}
