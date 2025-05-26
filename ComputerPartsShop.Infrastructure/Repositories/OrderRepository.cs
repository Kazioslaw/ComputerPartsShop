using ComputerPartsShop.Domain.Models;

namespace ComputerPartsShop.Infrastructure
{
	public class OrderRepository : IOrderRepository
	{
		readonly TempData _dbContext;

		public OrderRepository(TempData dbContext)
		{
			_dbContext = dbContext;
		}


		public async Task<List<Order>> GetListAsync(CancellationToken ct)
		{
			await Task.Delay(500, ct);
			return _dbContext.OrderList;
		}

		public async Task<Order> GetAsync(int id, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var order = _dbContext.OrderList.FirstOrDefault(x => x.Id == id);

			return order!;
		}

		public async Task<int> CreateAsync(Order request, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var last = _dbContext.OrderList.OrderBy(x => x.Id).LastOrDefault();

			if (last == null)
			{
				request.Id = 1;
			}
			else
			{
				request.Id = last.Id + 1;
			}

			_dbContext.OrderList.Add(request);

			return request.Id;
		}

		public async Task<Order> UpdateAsync(int id, Order request, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var order = _dbContext.OrderList.FirstOrDefault(x => x.Id == id);

			if (order != null)
			{
				order.CustomerId = request.CustomerId;
				order.OrdersProducts = request.OrdersProducts;
				order.Total = request.Total;
				order.DeliveryAddress = request.DeliveryAddress;
				order.Status = request.Status;
				order.OrderedAt = request.OrderedAt;
				order.SendAt = request.SendAt;
				order.Payments = request.Payments;
			}

			return order!;
		}

		public async Task DeleteAsync(int id, CancellationToken ct)
		{
			await Task.Delay(500, ct);
			var order = _dbContext.OrderList.FirstOrDefault(x => x.Id == id);

			if (order != null)
			{
				_dbContext.OrderList.Remove(order);
			}
		}
	}
}
