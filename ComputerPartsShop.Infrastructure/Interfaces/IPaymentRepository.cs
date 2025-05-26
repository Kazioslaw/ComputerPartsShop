using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface IPaymentRepository : IRepository<Payment, int>
	{
	}
}
