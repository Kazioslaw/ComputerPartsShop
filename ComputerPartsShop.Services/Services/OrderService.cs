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

			return orderList.Select(o => new OrderResponse(o.Id, o.CustomerId, o.OrdersProducts.Select(p => p.ProductId).ToList(), o.Total,
				o.DeliveryAddress == null ? Guid.NewGuid() : o.DeliveryAddress.Id,
				o.Status, o.OrderedAt, o.SendAt, o.Payments.Select(p => p.Id).ToList())).ToList();
		}

		public async Task<DetailedOrderResponse> GetAsync(int id, CancellationToken ct)
		{
			var order = await _orderRepository.GetAsync(id, ct);
			var productList = order.OrdersProducts.Select(o => o.Product).ToList();
			var paymentList = order.Payments;

			return order == null ? null! : new DetailedOrderResponse(
				order.Id,
				new CustomerResponse(order.Customer.Id, order.Customer.FirstName, order.Customer.LastName, order.Customer.Username, order.Customer.Email,
				order.Customer.PhoneNumber), productList.Select(p => new ProductInOrderResponse(p.Id, p.Name, p.UnitPrice,
				order.OrdersProducts.Where(c => c.ProductId == p.Id).Sum(c => c.Quantity))).ToList(), order.Total,
				new AddressResponse(order.DeliveryAddress.Id, order.DeliveryAddress.Street, order.DeliveryAddress.City,
				order.DeliveryAddress.Region, order.DeliveryAddress.ZipCode, order.DeliveryAddress.Country.Alpha3),
				order.Status, order.OrderedAt, order.SendAt, paymentList.Select(p => new PaymentInOrderResponse(p.Id,
				new CustomerPaymentSystemResponse(p.CustomerPaymentSystem.Id, p.CustomerPaymentSystem.Customer.Username, p.CustomerPaymentSystem.Customer.Email,
				p.CustomerPaymentSystem.Provider.Name, p.CustomerPaymentSystem.PaymentReference),
				p.Total, p.Method, p.Status, p.PaymentStartAt, p.PaidAt)).ToList());
		}

		public async Task<OrderResponse> CreateAsync(OrderRequest entity, CancellationToken ct)
		{
			var customer = await _customerRepository.GetByUsernameOrEmailAsync(entity.Username! ?? entity.Email!, ct);
			var address = await _addressRepository.GetAsync(entity.AddressId, ct);
			var productsId = entity.ProductIdList;
			var productsIdDistinct = productsId.Distinct();
			var productList = new List<OrderProduct>();
			foreach (var productId in productsIdDistinct)
			{
				var product = await _productRepository.GetAsync(productId, ct);
				var orderProduct = new OrderProduct()
				{
					ProductId = productId,
					Product = product,
					Quantity = entity.ProductIdList.Count(id => id == productId),
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

			var orderId = await _orderRepository.CreateAsync(newOrder, ct);

			return new OrderResponse(orderId, newOrder.CustomerId, entity.ProductIdList, entity.Total, entity.AddressId, newOrder.Status,
				newOrder.OrderedAt, null, newOrder.Payments.Select(p => p.Id).ToList());
		}

		public async Task<OrderResponse> UpdateAsync(int id, OrderRequest entity, CancellationToken ct)
		{
			var customer = await _customerRepository.GetByUsernameOrEmailAsync(entity.Username! ?? entity.Email!, ct);
			var address = await _addressRepository.GetAsync(entity.AddressId, ct);

			var productIdList = entity.ProductIdList;
			var productsIdDistinct = productIdList.Distinct();
			var productList = new List<OrderProduct>();

			var customerPaymentSystem = await _customerPaymentSystemRepository.GetAsync(entity.CustomerPaymentSystemId, ct);

			var payments = customerPaymentSystem.Payments.Where(x => x.OrderId == id).ToList();

			foreach (var productId in productsIdDistinct)
			{
				var product = await _productRepository.GetAsync(productId, ct);
				var orderProduct = new OrderProduct()
				{
					ProductId = productId,
					Product = product,
					Quantity = entity.ProductIdList.Count(id => id == productId),
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
			return new OrderResponse(id, customer.Id, entity.ProductIdList, entity.Total, entity.AddressId,
				order.Status, entity.OrderedAt, entity.SendAt, payments?.Select(x => x.Id).ToList() ?? new List<int>());
		}

		public async Task DeleteAsync(int id, CancellationToken ct)
		{
			await _orderRepository.DeleteAsync(id, ct);
		}
	}
}
