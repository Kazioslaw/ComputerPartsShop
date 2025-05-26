using ComputerPartsShop.Domain.DTO;
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
		private readonly ICustomerPaymentSystemRepository _cpsRepository;

		public OrderService(IOrderRepository orderRepository, ICustomerRepository customerRepository,
			IProductRepository productRepository, IAddressRepository addressRepository,
			ICustomerPaymentSystemRepository cpsRepository)
		{
			_orderRepository = orderRepository;
			_customerRepository = customerRepository;
			_productRepository = productRepository;
			_addressRepository = addressRepository;
			_cpsRepository = cpsRepository;
		}

		public async Task<List<OrderResponse>> GetListAsync(CancellationToken ct)
		{
			var orderList = await _orderRepository.GetListAsync(ct);

			return orderList.Select(o => new OrderResponse(o.ID, o.CustomerID, o.OrdersProducts.Select(p => p.ProductID).ToList(), o.Total, o.DeliveryAddress.ID,
				o.Status, o.OrderedAt, o.SendAt, o.Payments.Select(p => p.ID).ToList())).ToList();
		}

		public async Task<DetailedOrderResponse> GetAsync(int id, CancellationToken ct)
		{
			var order = await _orderRepository.GetAsync(id, ct);
			var productList = order.OrdersProducts.Select(o => o.Product).ToList();
			var paymentList = order.Payments;

			return order == null ? null! : new DetailedOrderResponse(
				order.ID,
				new CustomerResponse(order.Customer.ID, order.Customer.FirstName, order.Customer.LastName, order.Customer.Username, order.Customer.Email, order.Customer.PhoneNumber),
				productList.Select(p => new ProductInOrderResponse(p.ID, p.Name, p.UnitPrice, order.OrdersProducts.Where(c => c.ProductID == p.ID).Sum(c => c.Quantity))).ToList(),
				order.Total,
				new AddressResponse(order.DeliveryAddress.ID, order.DeliveryAddress.Street, order.DeliveryAddress.City,
				order.DeliveryAddress.Region, order.DeliveryAddress.ZipCode, order.DeliveryAddress.Country.Alpha3),
				order.Status, order.OrderedAt, order.SendAt,
				paymentList.Select(p => new PaymentInOrderResponse(p.ID, new CustomerPaymentSystemResponse(p.CustomerPaymentSystem.ID,
				p.CustomerPaymentSystem.Customer.Username, p.CustomerPaymentSystem.Customer.Email, p.CustomerPaymentSystem.Provider.Name, p.CustomerPaymentSystem.PaymentReference),
				p.Total, p.Method, p.Status, p.PaymentStartAt, p.PaidAt)).ToList());
		}

		public async Task<OrderResponse> CreateAsync(OrderRequest entity, CancellationToken ct)
		{
			var customer = await _customerRepository.GetByUsernameOrEmailAsync(entity.Username! ?? entity.Email!, ct);
			var address = await _addressRepository.GetAsync(entity.AddressID, ct);
			var productsID = entity.ProductIDList;
			var productsIDDistinct = productsID.Distinct();
			var productList = new List<OrderProduct>();
			foreach (var productID in productsIDDistinct)
			{
				var product = await _productRepository.GetAsync(productID, ct);
				var orderProduct = new OrderProduct()
				{
					ProductID = productID,
					Product = product,
					Quantity = entity.ProductIDList.Count(id => id == productID),
				};
				productList.Add(orderProduct);
			}

			var newOrder = new Order()
			{
				CustomerID = customer.ID,
				Customer = customer,
				OrdersProducts = productList,
				Total = entity.Total,
				DeliveryAddressID = entity.AddressID,
				DeliveryAddress = address,
				Status = "Pending",
				OrderedAt = DateTime.Now,
			};

			var orderID = await _orderRepository.CreateAsync(newOrder, ct);

			return new OrderResponse(orderID, newOrder.CustomerID, entity.ProductIDList, entity.Total, entity.AddressID, newOrder.Status, newOrder.OrderedAt, null, newOrder.Payments.Select(p => p.ID).ToList());
		}

		public async Task<OrderResponse> UpdateAsync(int id, OrderRequest entity, CancellationToken ct)
		{
			var customer = await _customerRepository.GetByUsernameOrEmailAsync(entity.Username! ?? entity.Email!, ct);
			var address = await _addressRepository.GetAsync(entity.AddressID, ct);

			var productIDList = entity.ProductIDList;
			var productsIDDistinct = productIDList.Distinct();
			var productList = new List<OrderProduct>();

			var cps = await _cpsRepository.GetAsync(entity.CustomerPaymentSystemID, ct);

			var payments = cps.Payments.Where(x => x.OrderID == id).ToList();

			foreach (var productID in productsIDDistinct)
			{
				var product = await _productRepository.GetAsync(productID, ct);
				var orderProduct = new OrderProduct()
				{
					ProductID = productID,
					Product = product,
					Quantity = entity.ProductIDList.Count(id => id == productID),
				};
				productList.Add(orderProduct);
			}

			Order order = null!;

			switch (entity.Status)
			{
				case "Pending":
				case "Processing":
					order = new Order()
					{
						CustomerID = customer.ID,
						Customer = customer,
						OrdersProducts = productList,
						Total = entity.Total,
						DeliveryAddressID = entity.AddressID,
						DeliveryAddress = address,
						Status = entity.Status!,
						OrderedAt = (DateTime)entity.OrderedAt!,
						Payments = payments ?? new List<Payment>(),
					};
					break;
				case "Shipped":
					order = new Order()
					{
						CustomerID = customer.ID,
						Customer = customer,
						OrdersProducts = productList,
						Total = entity.Total,
						DeliveryAddressID = entity.AddressID,
						DeliveryAddress = address,
						Status = entity.Status!,
						OrderedAt = (DateTime)entity.OrderedAt!,
						SendAt = DateTime.Now,
						Payments = payments ?? new List<Payment>()
					};
					break;
				case "Delivered":
				case "Returned":
				case "Cancelled":
					order = new Order()
					{
						CustomerID = customer.ID,
						Customer = customer,
						OrdersProducts = productList,
						Total = entity.Total,
						DeliveryAddressID = entity.AddressID,
						DeliveryAddress = address,
						Status = entity.Status!,
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
			return new OrderResponse(id, customer.ID, entity.ProductIDList, entity.Total, entity.AddressID,
				entity.Status!, entity.OrderedAt, entity.SendAt, payments?.Select(x => x.ID).ToList() ?? new List<int>());
		}

		public async Task DeleteAsync(int id, CancellationToken ct)
		{
			await _orderRepository.DeleteAsync(id, ct);
		}
	}
}
