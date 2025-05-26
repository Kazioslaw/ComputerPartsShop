using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface IAddressRepository : IRepository<Address, Guid>
	{
	}
}
