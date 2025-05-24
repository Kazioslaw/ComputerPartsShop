using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class PaymentRepository : ICRUDRepository<Payment, int>
	{
		public List<Payment> GetList()
		{
			throw new NotImplementedException();
		}

		public Payment Get(int id)
		{
			throw new NotImplementedException();
		}

		public Payment Create(Payment request)
		{
			throw new NotImplementedException();
		}

		public Payment Update(int id, Payment request)
		{
			throw new NotImplementedException();
		}

		public void Delete(int id)
		{

		}
	}
}
