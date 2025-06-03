using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface IOrderService
	{
		public Task<List<OrderResponse>> GetListAsync(CancellationToken ct);
		public Task<OrderResponse> GetAsync(int id, CancellationToken ct);
		public Task<OrderResponse> CreateAsync(OrderRequest request, CancellationToken ct);
		public Task<OrderResponse> UpdateStatusAsync(int id, UpdateOrderRequest request, CancellationToken ct);
		public Task<bool> DeleteAsync(int id, CancellationToken ct);
	}
}
