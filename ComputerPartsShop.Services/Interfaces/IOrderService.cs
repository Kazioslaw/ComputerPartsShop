using ComputerPartsShop.Domain.DTO;

namespace ComputerPartsShop.Services
{
	public interface IOrderService : IService<OrderRequest, OrderResponse, DetailedOrderResponse, int>
	{
	}
}
