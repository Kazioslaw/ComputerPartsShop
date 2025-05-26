using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Domain.Models;
using ComputerPartsShop.Infrastructure;

namespace ComputerPartsShop.Services
{
	public class OrderService : IService<OrderRequest, OrderResponse, DetailedOrderResponse, int>
	{
		private readonly IRepository<Order, int> _orderRepository;
		private readonly ICustomerRepository _customerRepository;
		private readonly IRepository<Product, int> _productRepository;
		private readonly IRepository<Address, Guid> _addressRepository;
		private readonly IRepository<CustomerPaymentSystem, Guid> _cpsRepository;

		public OrderService(IRepository<Order, int> orderRepository, ICustomerRepository customerRepository,
			IRepository<Product, int> productRepository, IRepository<Address, Guid> addressRepository,
			IRepository<CustomerPaymentSystem, Guid> cpsRepository)
		{
			_orderRepository = orderRepository;
			_customerRepository = customerRepository;
			_productRepository = productRepository;
			_addressRepository = addressRepository;
			_cpsRepository = cpsRepository;
		}

		public async Task<List<OrderResponse>> GetList()
		{
			var orderList = await _orderRepository.GetList();

			return orderList.Select(o => new OrderResponse(o.ID, o.CustomerID, o.OrdersProducts.Select(p => p.ProductID).ToList(), o.Total, o.DeliveryAddress.ID,
				o.Status, o.OrderedAt, o.SendAt, o.Payments.Select(p => p.ID).ToList())).ToList();
		}

		public async Task<DetailedOrderResponse> Get(int id)
		{
			var order = await _orderRepository.Get(id);
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

		public async Task<OrderResponse> Create(OrderRequest order)
		{
			var customer = await _customerRepository.GetByUsernameOrEmail(order.Username! ?? order.Email!);
			var address = await _addressRepository.Get(order.AddressID);
			var productsID = order.ProductIDList;
			var productsIDDistinct = productsID.Distinct();
			var productList = new List<OrderProduct>();
			foreach (var productID in productsIDDistinct)
			{
				var product = await _productRepository.Get(productID);
				var orderProduct = new OrderProduct()
				{
					ProductID = productID,
					Product = product,
					Quantity = order.ProductIDList.Count(id => id == productID),
				};
				productList.Add(orderProduct);
			}

			var newOrder = new Order()
			{
				CustomerID = customer.ID,
				Customer = customer,
				OrdersProducts = productList,
				Total = order.Total,
				DeliveryAddressID = order.AddressID,
				DeliveryAddress = address,
				Status = "Pending",
				OrderedAt = DateTime.Now,
			};

			var orderID = await _orderRepository.Create(newOrder);

			return new OrderResponse(orderID, newOrder.CustomerID, order.ProductIDList, order.Total, order.AddressID, newOrder.Status, newOrder.OrderedAt, null, newOrder.Payments.Select(p => p.ID).ToList());
		}

		public async Task<OrderResponse> Update(int id, OrderRequest updatedOrder)
		{
			var customer = await _customerRepository.GetByUsernameOrEmail(updatedOrder.Username! ?? updatedOrder.Email!);
			var address = await _addressRepository.Get(updatedOrder.AddressID);

			var productIDList = updatedOrder.ProductIDList;
			var productsIDDistinct = productIDList.Distinct();
			var productList = new List<OrderProduct>();

			var cps = await _cpsRepository.Get(updatedOrder.CustomerPaymentSystemID);

			var payments = cps.Payments.Where(x => x.OrderID == id).ToList();

			foreach (var productID in productsIDDistinct)
			{
				var product = await _productRepository.Get(productID);
				var orderProduct = new OrderProduct()
				{
					ProductID = productID,
					Product = product,
					Quantity = updatedOrder.ProductIDList.Count(id => id == productID),
				};
				productList.Add(orderProduct);
			}

			Order order = null!;

			switch (updatedOrder.Status)
			{
				case "Pending":
				case "Processing":
					order = new Order()
					{
						CustomerID = customer.ID,
						Customer = customer,
						OrdersProducts = productList,
						Total = updatedOrder.Total,
						DeliveryAddressID = updatedOrder.AddressID,
						DeliveryAddress = address,
						Status = updatedOrder.Status!,
						OrderedAt = (DateTime)updatedOrder.OrderedAt!,
						Payments = payments ?? new List<Payment>(),
					};
					break;
				case "Shipped":
					order = new Order()
					{
						CustomerID = customer.ID,
						Customer = customer,
						OrdersProducts = productList,
						Total = updatedOrder.Total,
						DeliveryAddressID = updatedOrder.AddressID,
						DeliveryAddress = address,
						Status = updatedOrder.Status!,
						OrderedAt = (DateTime)updatedOrder.OrderedAt!,
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
						Total = updatedOrder.Total,
						DeliveryAddressID = updatedOrder.AddressID,
						DeliveryAddress = address,
						Status = updatedOrder.Status!,
						OrderedAt = (DateTime)updatedOrder.OrderedAt!,
						SendAt = (DateTime)updatedOrder.SendAt!,
						Payments = payments ?? new List<Payment>()
					};
					break;
			}
			if (order != null)
			{
				await _orderRepository.Update(id, order);
			}
			return new OrderResponse(id, customer.ID, updatedOrder.ProductIDList, updatedOrder.Total, updatedOrder.AddressID,
				updatedOrder.Status!, (DateTime)updatedOrder.OrderedAt!, (DateTime)updatedOrder.SendAt!, payments.Select(x => x.ID).ToList() ?? new List<int>());
		}

		public async Task Delete(int id)
		{
			await _orderRepository.Delete(id);
		}
	}
}
