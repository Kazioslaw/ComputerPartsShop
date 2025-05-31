using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Enums;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class OrderService : IOrderService
	{
		private readonly IOrderRepository _orderRepository;
		private readonly ICustomerRepository _customerRepository;
		private readonly IProductRepository _productRepository;
		private readonly IAddressRepository _addressRepository;
		private readonly ICustomerPaymentSystemRepository _customerPaymentSystemRepository;

		public OrderService(IOrderRepository orderRepository, ICustomerRepository customerRepository,
			IProductRepository productRepository, IAddressRepository addressRepository,
			ICustomerPaymentSystemRepository customerPaymentSystemRepository)
		{
			_orderRepository = orderRepository;
			_customerRepository = customerRepository;
			_productRepository = productRepository;
			_addressRepository = addressRepository;
			_customerPaymentSystemRepository = customerPaymentSystemRepository;
		}

		public async Task<List<OrderResponse>> GetListAsync(CancellationToken ct)
		{
			var orderList = await _orderRepository.GetListAsync(ct);

			return orderList.Select(o => new OrderResponse(o.Id,
				o.CustomerId, o.OrdersProducts.Select(p => new ProductInOrderResponse(p.ProductId, p.Product.Name, p.Product.UnitPrice, p.Quantity)).ToList(),
				o.Total, o.DeliveryAddress == null ? Guid.NewGuid() : o.DeliveryAddress.Id,
				o.Status, o.OrderedAt, o.SendAt, o.Payments == null ? null! : o.Payments.Select(p => p.Id).ToList())).ToList();
		}

		public async Task<OrderResponse> GetAsync(int id, CancellationToken ct)
		{
			var order = await _orderRepository.GetAsync(id, ct);

			if (order == null)
			{
				return null;
			}

			var productList = order.OrdersProducts.Select(o => o.Product).ToList();
			var paymentList = order.Payments ?? new List<Payment>();

			return new OrderResponse(order.Id,
				order.CustomerId, order.OrdersProducts.Select(p => new ProductInOrderResponse(p.ProductId, p.Product.Name, p.Product.UnitPrice, p.Quantity)).ToList(),
				order.Total, order.DeliveryAddress == null ? Guid.NewGuid() : order.DeliveryAddress.Id,
				order.Status, order.OrderedAt, order.SendAt, order.Payments == null ? null! : order.Payments.Select(p => p.Id).ToList());
		}

		public async Task<OrderResponse> CreateAsync(OrderRequest entity, CancellationToken ct)
		{
			var customer = await _customerRepository.GetByUsernameOrEmailAsync(entity.Username! ?? entity.Email!, ct);
			var address = await _addressRepository.GetAsync(entity.AddressId, ct);
			var productOrderList = entity.Products;
			var productList = new List<OrderProduct>();
			foreach (var productOrder in productOrderList)
			{
				var product = await _productRepository.GetAsync(productOrder.Id, ct);
				var orderProduct = new OrderProduct()
				{
					ProductId = product.Id,
					Product = product,
					Quantity = productOrder.Quantity,
				};
				productList.Add(orderProduct);
			}

			var newOrder = new Order()
			{
				CustomerId = customer.Id,
				Customer = customer,
				OrdersProducts = productList,
				Total = entity.Total,
				DeliveryAddressId = entity.AddressId,
				DeliveryAddress = address,
				Status = DeliveryStatus.Pending,
				OrderedAt = DateTime.Now,
			};

			var order = await _orderRepository.CreateAsync(newOrder, ct);

			return new OrderResponse(order.Id, newOrder.CustomerId, newOrder.OrdersProducts.Select(x => new ProductInOrderResponse(x.ProductId, x.Product.Name, x.Product.UnitPrice, x.Quantity)).ToList(),
				entity.Total, entity.AddressId, newOrder.Status, newOrder.OrderedAt, null, newOrder.Payments.Select(p => p.Id).ToList());
		}

		public async Task<OrderResponse> UpdateAsync(int id, OrderRequest entity, CancellationToken ct)
		{
			var customer = await _customerRepository.GetByUsernameOrEmailAsync(entity.Username! ?? entity.Email!, ct);
			var address = await _addressRepository.GetAsync(entity.AddressId, ct);

			var productOrderList = entity.Products;
			var productList = new List<OrderProduct>();

			var customerPaymentSystem = await _customerPaymentSystemRepository.GetAsync(entity.CustomerPaymentSystemId, ct);

			var payments = customerPaymentSystem.Payments.Where(x => x.OrderId == id).ToList();

			foreach (var productOrder in productOrderList)
			{
				var product = await _productRepository.GetAsync(productOrder.Id, ct);
				var orderProduct = new OrderProduct()
				{
					ProductId = productOrder.Id,
					Product = product,
					Quantity = productOrder.Quantity
				};
				productList.Add(orderProduct);
			}

			Order order = null!;

			switch (entity.Status)
			{
				case DeliveryStatus.Pending:
				case DeliveryStatus.Processing:
					order = new Order()
					{
						CustomerId = customer.Id,
						Customer = customer,
						OrdersProducts = productList,
						Total = entity.Total,
						DeliveryAddressId = entity.AddressId,
						DeliveryAddress = address,
						Status = (DeliveryStatus)entity.Status,
						OrderedAt = (DateTime)entity.OrderedAt!,
						Payments = payments ?? new List<Payment>(),
					};
					break;
				case DeliveryStatus.Shipped:
					order = new Order()
					{
						CustomerId = customer.Id,
						Customer = customer,
						OrdersProducts = productList,
						Total = entity.Total,
						DeliveryAddressId = entity.AddressId,
						DeliveryAddress = address,
						Status = (DeliveryStatus)entity.Status,
						OrderedAt = (DateTime)entity.OrderedAt!,
						SendAt = DateTime.Now,
						Payments = payments ?? new List<Payment>()
					};
					break;
				case DeliveryStatus.Delivered:
				case DeliveryStatus.Returned:
				case DeliveryStatus.Cancelled:
					order = new Order()
					{
						CustomerId = customer.Id,
						Customer = customer,
						OrdersProducts = productList,
						Total = entity.Total,
						DeliveryAddressId = entity.AddressId,
						DeliveryAddress = address,
						Status = (DeliveryStatus)entity.Status,
						OrderedAt = (DateTime)entity.OrderedAt!,
						SendAt = (DateTime)entity.SendAt!,
						Payments = payments ?? new List<Payment>()
					};
					break;
			}
			if (order != null)
			{
				await _orderRepository.UpdateAsync(id, order, ct);
			}
			return new OrderResponse(id, customer.Id, order.OrdersProducts.Select(x => new ProductInOrderResponse(x.ProductId, x.Product.Name, x.Product.UnitPrice, x.Quantity)).ToList(),
				entity.Total, entity.AddressId, order.Status, entity.OrderedAt, entity.SendAt, payments?.Select(x => x.Id).ToList() ?? new List<int>());
		}

		public async Task<bool> DeleteAsync(int id, CancellationToken ct)
		{
			return await _orderRepository.DeleteAsync(id, ct);
		}
	}
}
