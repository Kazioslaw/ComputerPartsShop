using AutoMapper;
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
		private readonly IMapper _mapper;

		public OrderService(IOrderRepository orderRepository, ICustomerRepository customerRepository,
			IProductRepository productRepository, IAddressRepository addressRepository,
			IMapper mapper)
		{
			_orderRepository = orderRepository;
			_customerRepository = customerRepository;
			_productRepository = productRepository;
			_addressRepository = addressRepository;
			_mapper = mapper;
		}

		public async Task<List<OrderResponse>> GetListAsync(CancellationToken ct)
		{
			var result = await _orderRepository.GetListAsync(ct);

			var orderList = _mapper.Map<IEnumerable<OrderResponse>>(result);

			return orderList.ToList();
		}

		public async Task<OrderResponse> GetAsync(int id, CancellationToken ct)
		{
			var result = await _orderRepository.GetAsync(id, ct);

			if (result == null)
			{
				return null;
			}

			var order = _mapper.Map<OrderResponse>(result);

			return order;
		}

		public async Task<OrderResponse> CreateAsync(OrderRequest entity, CancellationToken ct)
		{
			var customer = await _customerRepository.GetByUsernameOrEmailAsync(entity.Username! ?? entity.Email!, ct);
			var address = await _addressRepository.GetAsync(entity.AddressId, ct);

			if (customer == null || address == null)
			{
				return null;
			}
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

			var newOrder = _mapper.Map<Order>(entity);
			newOrder.CustomerId = customer.Id;
			newOrder.Customer = customer;
			newOrder.OrdersProducts = productList;
			newOrder.DeliveryAddressId = address.Id;
			newOrder.DeliveryAddress = address;
			newOrder.Payments = new List<Payment>();

			var result = await _orderRepository.CreateAsync(newOrder, ct);
			var order = _mapper.Map<OrderResponse>(result);

			return order;
		}

		public async Task<OrderResponse> UpdateStatusAsync(int id, UpdateOrderRequest entity, CancellationToken ct)
		{
			var existingOrder = await _orderRepository.GetAsync(id, ct);

			if (existingOrder == null)
			{
				return null;
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

			if (result == null)
			{
				return null;
			}

			var updatedOrder = _mapper.Map<OrderResponse>(result);

			return updatedOrder;
		}

		public async Task<bool> DeleteAsync(int id, CancellationToken ct)
		{
			return await _orderRepository.DeleteAsync(id, ct);
		}
	}
}
