using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface ICustomerPaymentSystemRepository : IRepository<CustomerPaymentSystem, Guid>
	{
	}
}