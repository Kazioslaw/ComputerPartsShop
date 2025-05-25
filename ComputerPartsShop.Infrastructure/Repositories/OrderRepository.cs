using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class OrderRepository : ICRUDRepository<Order, int>
	{
		readonly TempData _dbContext;

		public OrderRepository(TempData dbContext)
		{
			_dbContext = dbContext;
		}


		public async Task<List<Order>> GetList()
		{
			return _dbContext.OrderList;
		}

		public async Task<Order> Get(int id)
		{
			var order = _dbContext.OrderList.FirstOrDefault(x => x.ID == id);

			return order;
		}

		public async Task<int> Create(Order request)
		{
			var last = _dbContext.OrderList.OrderBy(x => x.ID).LastOrDefault();

			if (last == null)
			{
				request.ID = 1;
			}
			else
			{
				request.ID = last.ID + 1;
			}

			_dbContext.OrderList.Add(request);

			return request.ID;
		}

		public async Task<Order> Update(int id, Order request)
		{
			var order = _dbContext.OrderList.FirstOrDefault(x => x.ID == id);

			if (order != null)
			{
				order.CustomerID = request.CustomerID;
				order.OrdersProducts = request.OrdersProducts;
				order.Total = request.Total;
				order.DeliveryAddress = request.DeliveryAddress;
				order.Status = request.Status;
				order.OrderedAt = request.OrderedAt;
				order.SendAt = request.SendAt;
				order.Payments = request.Payments;
			}

			return order;
		}

		public async Task Delete(int id)
		{
			var order = _dbContext.OrderList.FirstOrDefault(x => x.ID == id);

			if (order != null)
			{
				_dbContext.OrderList.Remove(order);
			}
		}
	}
}
