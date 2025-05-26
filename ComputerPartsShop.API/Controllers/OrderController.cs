using ComputerPartsShop.Domain.DTO;
using ComputerPartsShop.Infrastructure;
using ComputerPartsShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerPartsShop.API.Controllers
{
	[ApiController]
	[Route("/[controller]")]
	public class OrderController : ControllerBase
	{
		private readonly IOrderService _orderService;
		private readonly ICustomerRepository _customerRepository;
		private readonly IAddressService _addressService;


		public OrderController(IOrderService orderService,
			ICustomerRepository customerRepository,
			IAddressService addressService)
		{
			_orderService = orderService;
			_customerRepository = customerRepository;
			_addressService = addressService;
		}

		[HttpGet]
		public async Task<ActionResult<List<OrderResponse>>> GetOrderListAsync()
		{
			var orderList = await _orderService.GetListAsync();

			return Ok(orderList);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<DetailedOrderResponse>> GetOrderAsync(int id)
		{
			var order = await _orderService.GetAsync(id);

			if (order == null)
			{
				return NotFound();
			}

			return Ok(order);
		}

		[HttpPost]
		public async Task<ActionResult<OrderResponse>> CreateOrderAsync(OrderRequest request)
		{
			if (string.IsNullOrWhiteSpace(request.Username) && string.IsNullOrWhiteSpace(request.Email))
			{
				return BadRequest();
			}

			var customer = await _customerRepository.GetByUsernameOrEmailAsync(request.Username! ?? request.Email!);

			if (customer == null)
			{
				return BadRequest();
			}

			var address = await _addressService.GetAsync(request.AddressID);

			if (address == null)
			{
				return BadRequest();
			}

			var order = await _orderService.CreateAsync(request);


			return CreatedAtAction(nameof(CreateOrderAsync), order);
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<OrderResponse>> UpdateOrderAsync(int id, OrderRequest request)
		{
			var order = await _orderService.GetAsync(id);

			if (order == null)
			{
				return NotFound();
			}

			var updatedOrder = _orderService.UpdateAsync(id, request);

			return Ok(updatedOrder);
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult> DeleteOrderAsync(int id)
		{
			var order = await _orderService.GetAsync(id);

			if (order == null)
			{
				return NotFound();
			}

			await _orderService.DeleteAsync(id);

			return Ok();
		}
	}
}
