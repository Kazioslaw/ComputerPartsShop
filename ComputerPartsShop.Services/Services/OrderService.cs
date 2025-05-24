using ComputerPartsShop.Domain.DTOs;

namespace ComputerPartsShop.Services
{
	internal class OrderService : ICRUDService<OrderRequest, OrderResponse, DetailedOrderResponse, int>
	{
		public List<OrderResponse> GetList()
		{
			throw new NotImplementedException();
		}

		public DetailedOrderResponse Get(int id)
		{
			throw new NotImplementedException();
		}

		public OrderResponse Create(OrderRequest request)
		{
			throw new NotImplementedException();
		}

		public OrderResponse Update(int id, OrderRequest request)
		{
			throw new NotImplementedException();
		}

		public void Delete(int id)
		{
			throw new NotImplementedException();
		}
	}
}
