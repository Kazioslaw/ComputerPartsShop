using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public interface IOrderRepository
	{
		public Task<List<Order>> GetListAsync(CancellationToken ct);
		public Task<Order> GetAsync(int id, CancellationToken ct);
		public Task<Order> CreateAsync(Order request, CancellationToken ct);
		public Task<Order> UpdateStatusAsync(int id, Order request, CancellationToken ct);
		public Task DeleteAsync(int id, CancellationToken ct);
	}
}
