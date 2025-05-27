using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface IOrderService
	{
		public Task<List<OrderResponse>> GetListAsync(CancellationToken ct);
		public Task<DetailedOrderResponse> GetAsync(int id, CancellationToken ct);
		public Task<OrderResponse> CreateAsync(OrderRequest entity, CancellationToken ct);
		public Task<OrderResponse> UpdateAsync(int id, OrderRequest entity, CancellationToken ct);
		public Task DeleteAsync(int id, CancellationToken ct);
	}
}
