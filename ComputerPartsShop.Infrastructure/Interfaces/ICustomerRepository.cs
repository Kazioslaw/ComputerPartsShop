using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface ICustomerRepository : IRepository<Customer, Guid>
	{
		public Task<Customer> GetByUsernameOrEmailAsync(string input, CancellationToken ct);
	}
}
