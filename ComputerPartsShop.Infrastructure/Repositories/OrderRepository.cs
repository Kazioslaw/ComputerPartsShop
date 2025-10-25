using ComputerPartsShop.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ComputerPartsShop.Infrastructure
{
	public class OrderRepository : IOrderRepository
	{
		private readonly DBContext _dbContext;

		public OrderRepository(DBContext dbContext, IPaymentRepository paymentRepository)
		{
			_dbContext = dbContext;
		}


		public async Task<List<Order>> GetListAsync(CancellationToken ct)
		{
			var query = "SELECT \"Order\".ID, \"Order\".UserID, \"Order\".Total, \"Order\".DeliveryAddressID, \"Order\".Status, " +
				"\"Order\".OrderedAt, \"Order\".SendAt, OrderProduct.ProductID, OrderProduct.Quantity, Product.Name, Product.UnitPrice, " +
				"Payment.ID FROM \"Order\" LEFT JOIN OrderProduct ON \"Order\".ID = OrderProduct.OrderID " +
				"LEFT JOIN Payment ON \"Order\".ID = Payment.OrderID LEFT JOIN Product ON Product.ID = OrderProduct.ProductID";

			using (var connection = await _dbContext.CreateConnection())
			{

				try
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
				catch (SqlException ex)
				{
					Console.WriteLine(ex.Message);

					throw;
				}
			}

		}

		public async Task<Order> GetAsync(int id, string username, CancellationToken ct)
		{
			var query = "SELECT \"Order\".ID, \"Order\".UserID, \"Order\".Total, \"Order\".DeliveryAddressID, \"Order\".Status, " +
				"\"Order\".OrderedAt, \"Order\".SendAt, OrderProduct.ProductID, OrderProduct.Quantity, Product.Name, Product.UnitPrice, " +
				"Payment.ID FROM \"Order\" LEFT JOIN OrderProduct ON \"Order\".ID = OrderProduct.OrderID " +
				"LEFT JOIN Payment ON \"Order\".ID = Payment.OrderID " +
				"LEFT JOIN Product ON Product.ID = OrderProduct.ProductID " +
				"JOIN ShopUser ON \"Order\".UserID = ShopUser.ID " +
				"WHERE \"Order\".ID = @Id AND ShopUser.Username = @Username";

			using (var connection = await _dbContext.CreateConnection())
			{
				try
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
					}, param: new { Id = id, Username = username },
					splitOn: "ProductID, Name, ID");

					return result.Distinct().FirstOrDefault();
				}
				catch (SqlException ex)
				{
					Console.WriteLine(ex.Message);

					throw;
				}
			}
		}

		public async Task<Order> CreateAsync(Order request, CancellationToken ct)
		{
			var createOrder = "INSERT INTO \"Order\" (UserID, DeliveryAddressID, Total, Status, OrderedAt, SendAt) " +
				"VALUES (@UserID, @DeliveryAddressID, @Total, @Status, @OrderedAt, @SendAt) " +
				"SELECT CAST(SCOPE_IDENTITY() AS int)";

			using (var connection = await _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						var orderParameters = new DynamicParameters();
						orderParameters.Add("UserID", request.UserId, DbType.Guid, ParameterDirection.Input);
						orderParameters.Add("DeliveryAddressID", request.DeliveryAddressId, DbType.Guid, ParameterDirection.Input);
						orderParameters.Add("Total", request.Total, DbType.Decimal, ParameterDirection.Input);
						orderParameters.Add("Status", request.Status.ToString(), DbType.String, ParameterDirection.Input);
						orderParameters.Add("OrderedAt", request.OrderedAt, DbType.DateTime2, ParameterDirection.Input);
						orderParameters.Add("SendAt", request.SendAt, DbType.DateTime2, ParameterDirection.Input);

						var orderID = await connection.QueryFirstOrDefaultAsync<int>(createOrder, orderParameters, transaction);

						var createOrderProduct = "INSERT INTO OrderProduct (OrderID, ProductID, Quantity) VALUES (@OrderID, @ProductID, @Quantity)";

						var orderProductList = new List<int>();

						foreach (var op in request.OrdersProducts)
						{
							var orderProduct = await connection.ExecuteAsync(createOrderProduct, new
							{
								OrderID = orderID,
								ProductID = op.ProductId,
								Quantity = op.Quantity
							}, transaction);
						}
						request.Id = orderID;

						transaction.Commit();

						return request;
					}
					catch (SqlException ex)
					{
						transaction.Rollback();
						Console.WriteLine(ex.Message);

						throw;
					}
				}
			}
		}

		public async Task<Order> UpdateStatusAsync(int id, Order request, CancellationToken ct)
		{
			var updateStatusQuery = "UPDATE \"Order\" SET Status = @Status, SendAt = @SendAt WHERE ID = @Id";

			var parameters = new DynamicParameters();
			parameters.Add("Id", request.Id, DbType.Int32, ParameterDirection.Input);
			parameters.Add("Status", request.Status.ToString(), DbType.String, ParameterDirection.Input);
			parameters.Add("SendAt", request.SendAt, DbType.DateTime2, ParameterDirection.Input);

			using (var connection = await _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						await connection.ExecuteAsync(updateStatusQuery, parameters, transaction);
						transaction.Commit();

						request.Id = id;

						return request;

					}
					catch (SqlException ex)
					{
						transaction.Rollback();
						Console.WriteLine(ex.Message);

						throw;
					}
				}
			}
		}

		public async Task DeleteAsync(int id, CancellationToken ct)
		{
			var query = "DELETE FROM \"Order\" WHERE ID = @Id";

			using (var connection = await _dbContext.CreateConnection())
			{
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						await connection.ExecuteAsync(query, new { Id = id }, transaction);
						transaction.Commit();
					}
					catch (SqlException ex)
					{
						transaction.Rollback();
						Console.WriteLine(ex.Message);

						throw;
					}
				}
			}
		}
	}
}
