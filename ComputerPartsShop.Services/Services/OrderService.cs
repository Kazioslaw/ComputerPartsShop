using AutoMapper;
using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Enums;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;
using Microsoft.Data.SqlClient;

namespace ComputerPartsShop.Services
{
	public class OrderService : IOrderService
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IShopUserRepository _userRepository;
		private readonly IProductRepository _productRepository;
		private readonly IAddressRepository _addressRepository;
		private readonly IMapper _mapper;

		public OrderService(IOrderRepository orderRepository, IShopUserRepository userRepository,
			IProductRepository productRepository, IAddressRepository addressRepository,
			IMapper mapper)
		{
			_orderRepository = orderRepository;
			_userRepository = userRepository;
			_productRepository = productRepository;
			_addressRepository = addressRepository;
			_mapper = mapper;
		}

		public async Task<List<OrderResponse>> GetListAsync(CancellationToken ct)
		{
			try
			{
				var result = await _orderRepository.GetListAsync(ct);

				var orderList = _mapper.Map<IEnumerable<OrderResponse>>(result);

				return orderList.ToList();
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<OrderResponse> GetAsync(int id, CancellationToken ct)
		{
			try
			{
				var result = await _orderRepository.GetAsync(id, ct);

				if (result == null)
				{
					throw new DataErrorException(404, "Order not found");
				}

				var order = _mapper.Map<OrderResponse>(result);

				return order;
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<OrderResponse> CreateAsync(OrderRequest entity, CancellationToken ct)
		{
			try
			{
				var user = await _userRepository.GetByUsernameOrEmailAsync(entity.Username! ?? entity.Email!, ct);

				if (user == null)
				{
					throw new DataErrorException(400, "Invalid or missing username or email");
				}

				var address = await _addressRepository.GetAsync(entity.AddressId, ct);

				if (address == null)
				{
					throw new DataErrorException(400, "That address does not exist or does not belong to the user");
				}

				var productOrderList = entity.Products;
				var productList = new List<OrderProduct>();
				foreach (var productOrder in productOrderList)
				{
					var product = await _productRepository.GetAsync(productOrder.Id, ct);

					if (product == null)
					{
						throw new DataErrorException(400, "That product does not exist");
					}

					var orderProduct = new OrderProduct()
					{
						ProductId = product.Id,
						Product = product,
						Quantity = productOrder.Quantity,
					};
					productList.Add(orderProduct);
				}

				var newOrder = _mapper.Map<Order>(entity);
				newOrder.UserId = user.Id;
				newOrder.User = user;
				newOrder.OrdersProducts = productList;
				newOrder.DeliveryAddressId = address.Id;
				newOrder.DeliveryAddress = address;
				newOrder.Payments = new List<Payment>();

				var result = await _orderRepository.CreateAsync(newOrder, ct);
				var order = _mapper.Map<OrderResponse>(result);

				return order;
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task<OrderResponse> UpdateStatusAsync(int id, UpdateOrderRequest entity, CancellationToken ct)
		{
			try
			{
				var existingOrder = await _orderRepository.GetAsync(id, ct);

				if (existingOrder == null)
				{
					throw new DataErrorException(404, "Order not found");
				}

				switch (entity.Status)
				{
					case DeliveryStatus.Pending:
					case DeliveryStatus.Processing:
						existingOrder.Status = (DeliveryStatus)entity.Status;
						break;
					case DeliveryStatus.Shipped:
						existingOrder.Status = (DeliveryStatus)entity.Status;
						existingOrder.SendAt = DateTime.Now;
						break;
					case DeliveryStatus.Delivered:
					case DeliveryStatus.Returned:
					case DeliveryStatus.Cancelled:
						existingOrder.Status = (DeliveryStatus)entity.Status;
						break;
				}

				var result = await _orderRepository.UpdateStatusAsync(id, existingOrder, ct);

				var updatedOrder = _mapper.Map<OrderResponse>(result);

				return updatedOrder;
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}

		public async Task DeleteAsync(int id, CancellationToken ct)
		{
			try
			{
				var order = await _orderRepository.GetAsync(id, ct);

				if (order == null)
				{
					throw new DataErrorException(404, "Order not found");
				}

				await _orderRepository.DeleteAsync(id, ct);
			}
			catch (SqlException)
			{
				throw new DataErrorException(500, "Database operation failed");
			}
		}
	}
}
