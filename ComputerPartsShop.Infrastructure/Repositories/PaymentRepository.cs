using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class PaymentRepository : ICRUDRepository<Payment, int>
	{
		public Task<List<Payment>> GetList()
		{
			throw new NotImplementedException();
		}

		public Task<Payment> Get(int id)
		{
			throw new NotImplementedException();
		}

		public Task<int> Create(Payment request)
		{
			throw new NotImplementedException();
		}

		public Task<Payment> Update(int id, Payment request)
		{
			throw new NotImplementedException();
		}

		public Task Delete(int id)
		{
			return Task.CompletedTask;
		}
	}
}
