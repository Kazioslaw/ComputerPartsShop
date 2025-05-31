using ComputerPartsShop.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ComputerPartsShop.Infrastructure
{
	public class OrderRepository : IOrderRepository
	{
		private readonly DBContext _dbContext;

		public OrderRepository(DBContext dbContext)
		{
			_dbContext = dbContext;
		}


		public async Task<List<Order>> GetListAsync(CancellationToken ct)
		{
			var query = "SELECT \"Order\".ID, \"Order\".CustomerID, \"Order\".Total, \"Order\".DeliveryAddressID, \"Order\".Status, " +
				"\"Order\".OrderedAt, \"Order\".SendAt, OrderProduct.ProductID, OrderProduct.Quantity, Product.Name, Product.UnitPrice, " +
				"Payment.ID FROM \"Order\" LEFT JOIN OrderProduct ON \"Order\".ID = OrderProduct.OrderID " +
				"LEFT JOIN Payment ON \"Order\".ID = Payment.OrderID LEFT JOIN Product ON Product.ID = OrderProduct.ProductID";

			using (var connection = _dbContext.CreateConnection())
			{
				var orderDictionary = new Dictionary<int, Order>();
				var result = await connection.QueryAsync<Order, OrderProduct, Product, Payment, Order>(query, (order, orderProduct, product, payment) =>
				{
					if (!orderDictionary.TryGetValue(order.Id, out var currentOrder))
					{
						currentOrder = order;
						currentOrder.OrdersProducts = new List<OrderProduct>();
						currentOrder.Payments = new List<Payment>();
						orderDictionary.Add(order.Id, currentOrder);
					}

					if (orderProduct != null)
					{
						if (product == null)
						{
							orderProduct.Product = new Product();
						}
						else
						{
							orderProduct.Product = product;
						}


						if (!currentOrder.OrdersProducts.Any(op => op.ProductId == orderProduct.ProductId))
						{
							currentOrder.OrdersProducts.Add(orderProduct);
						}
					}

					if (payment != null && !currentOrder.Payments.Any(p => p.Id == payment.Id))
					{
						currentOrder.Payments.Add(payment);
					}

					return currentOrder;
				},
				splitOn: "ProductID, Name, ID");

				return result.Distinct().ToList();
			}

		}

		public async Task<Order> GetAsync(int id, CancellationToken ct)
		{
			var query = "SELECT \"Order\".ID, \"Order\".CustomerID, \"Order\".Total, \"Order\".DeliveryAddressID, \"Order\".Status, " +
				"\"Order\".OrderedAt, \"Order\".SendAt, OrderProduct.ProductID, OrderProduct.Quantity, Product.Name, Product.UnitPrice, " +
				"Payment.ID FROM \"Order\" LEFT JOIN OrderProduct ON \"Order\".ID = OrderProduct.OrderID " +
				"LEFT JOIN Payment ON \"Order\".ID = Payment.OrderID LEFT JOIN Product ON Product.ID = OrderProduct.ProductID";

			using (var connection = _dbContext.CreateConnection())
			{
				var orderDictionary = new Dictionary<int, Order>();
				var result = await connection.QueryAsync<Order, OrderProduct, Product, Payment, Order>(query, (order, orderProduct, product, payment) =>
				{
					if (!orderDictionary.TryGetValue(order.Id, out var currentOrder))
					{
						currentOrder = order;
						currentOrder.OrdersProducts = new List<OrderProduct>();
						currentOrder.Payments = new List<Payment>();
						orderDictionary.Add(order.Id, currentOrder);
					}

					if (orderProduct != null)
					{
						if (product == null)
						{
							orderProduct.Product = new Product();
						}
						else
						{
							orderProduct.Product = product;
						}


						if (!currentOrder.OrdersProducts.Any(op => op.ProductId == orderProduct.ProductId))
						{
							currentOrder.OrdersProducts.Add(orderProduct);
						}
					}

					if (payment != null && !currentOrder.Payments.Any(p => p.Id == payment.Id))
					{
						currentOrder.Payments.Add(payment);
					}

					return currentOrder;
				},
				splitOn: "ProductID, Name, ID");

				return result.Distinct().FirstOrDefault();
			}
		}

		public async Task<Order> CreateAsync(Order request, CancellationToken ct)
		{
			request.Id = 999;
			return request;
		}

		public async Task<Order> UpdateAsync(int id, Order request, CancellationToken ct)
		{
			request.Id = id;
			return request;
		}

		public async Task<bool> DeleteAsync(int id, CancellationToken ct)
		{
			var query = "DELETE FROM Order WHERE ID = @Id";

			using (var connection = _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						await connection.ExecuteAsync(query, new { id }, transaction);
						transaction.Commit();

						return true;
					}
					catch (SqlException)
					{
						transaction.Rollback();

						return false;
					}
				}
			}
		}
	}
}
