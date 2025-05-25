using ComputerPartsShop.Domain.DTOs;
using ComputerPartsShop.Infrastructure;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class OrderController : ControllerBase
	{
		private readonly OrderService _orderService;
		private readonly CustomerRepository _customerRepository;
		private readonly AddressService _addressService;


		public OrderController(OrderService orderService, CustomerRepository customerRepository, AddressService addressService)
		{
			_orderService = orderService;
			_customerRepository = customerRepository;
			_addressService = addressService;
		}

		[HttpGet]
		public async Task<ActionResult<List<OrderResponse>>> GetOrderList()
		{
			var orderList = await _orderService.GetList();

			return Ok(orderList);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<DetailedOrderResponse>> GetOrder(int id)
		{
			var order = await _orderService.Get(id);

			if (order == null)
			{
				return NotFound();
			}

			return Ok(order);
		}

		[HttpPost]
		public async Task<ActionResult<OrderResponse>> CreateOrder(OrderRequest request)
		{
			if (string.IsNullOrWhiteSpace(request.Username) && string.IsNullOrWhiteSpace(request.Email))
			{
				return BadRequest();
			}

			var customer = await _customerRepository.GetByUsernameOrEmail(request.Username! ?? request.Email!);

			if (customer == null)
			{
				return BadRequest();
			}

			var address = await _addressService.Get(request.AddressID);

			if (address == null)
			{
				return BadRequest();
			}

			var order = await _orderService.Create(request);


			return CreatedAtAction(nameof(CreateOrder), order);
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<OrderResponse>> UpdateOrder(int id, OrderRequest request)
		{
			var order = await _orderService.Get(id);

			if (order == null)
			{
				return NotFound();
			}

			var updatedOrder = _orderService.Update(id, request);

			return Ok(updatedOrder);
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult> DeleteOrder(int id)
		{
			var order = await _orderService.Get(id);

			if (order == null)
			{
				return NotFound();
			}

			await _orderService.Delete(id);

			return Ok();
		}
	}
}
